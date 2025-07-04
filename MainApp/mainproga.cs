using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginSystem;

public partial class Program
{
    public static void Main()
    {
        // путь к папке Plugins
        var pluginDirectory = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Plugins");
        pluginDirectory = Path.GetFullPath(pluginDirectory);

        Console.WriteLine("AppContext.BaseDirectory: " + AppContext.BaseDirectory);
        Console.WriteLine("путь к плагинам: " + pluginDirectory);
        Console.WriteLine("существует ли папка плагинов? " + Directory.Exists(pluginDirectory));

        if (!Directory.Exists(pluginDirectory))
        {
            Console.WriteLine($"папка с плагинами не найдена: {pluginDirectory}");
            return;
        }

        
        var pluginAssemblies = Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories);

        var pluginTypes = new Dictionary<string, Type>();
        var dependencies = new Dictionary<string, List<string>>();

        // прогрузка плагинов
        foreach (var dll in pluginAssemblies)
        {
            Assembly asm;
            try
            {
                asm = Assembly.LoadFrom(dll);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка загрузки сборки {dll}: {ex.Message}");
                continue;
            }

            foreach (var type in asm.GetTypes())
            {
                if (!typeof(IPlugin).IsAssignableFrom(type)) continue;
                if (type.GetCustomAttribute<PluginLoadAttribute>() == null) continue;

                var name = type.FullName!;
                pluginTypes[name] = type;

                var depends = type.GetCustomAttributes<DependsOnAttribute>()
                                  .Select(attr => attr.PluginName)
                                  .ToList();

                dependencies[name] = depends;
            }
        }

        // проверка зависимостей
        foreach (var (plugin, deps) in dependencies)
        {
            var toRemove = new List<string>();
            foreach (var dep in deps)
            {
                if (!pluginTypes.ContainsKey(dep))
                {
                    Console.WriteLine($"внимание: плагин '{plugin}' зависит от отсутствующего плагина '{dep}'");
                    toRemove.Add(dep);
                }
            }
            // чтобы не ломать сортировку
            foreach (var dep in toRemove)
            {
                deps.Remove(dep);
            }
        }

        List<string> sorted;
        try
        {
            sorted = TopoSort(pluginTypes.Keys.ToList(), dependencies);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ошибка при топ.сортировке: " + ex.Message);
            return;
        }

        Console.WriteLine("загрузка плагинов в следующем порядке:");
        foreach (var name in sorted)
        {
            var type = pluginTypes[name];
            var instance = Activator.CreateInstance(type) as IPlugin;
            instance?.Execute();
            Console.WriteLine($"{name} выполнен.");
        }
    }

    // toposort
    static List<string> TopoSort(List<string> nodes, Dictionary<string, List<string>> edges)
    {
        var result = new List<string>();
        var incoming = nodes.ToDictionary(n => n, n => 0);

        
        foreach (var deps in edges.Values)
        {
            foreach (var dep in deps)
            {
                if (incoming.ContainsKey(dep))
                    incoming[dep]++;
                else
                    throw new Exception($"плагин-зависимость '{dep}' отсутствует в списке плагинов");
            }
        }

        var queue = new Queue<string>(incoming.Where(kv => kv.Value == 0).Select(kv => kv.Key));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);

            foreach (var neighbor in edges.GetValueOrDefault(current, new List<string>()))
            {
                incoming[neighbor]--;
                if (incoming[neighbor] == 0)
                    queue.Enqueue(neighbor);
            }
        }

        if (result.Count != nodes.Count)
            throw new Exception("цикл в зависимостях плагинов");

        return result;
    }
}


