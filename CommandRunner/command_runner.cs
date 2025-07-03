using System.Reflection;
using CommandLib;

namespace CommandRunner;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Использование: CommandRunner <путь_к_библиотеке.dll> <имя_класса> [аргументы_конструктора]");
            return;
        }

        string dllPath = args[0];
        string className = args[1];
        string[] constructorArgs = args.Skip(2).ToArray();

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Файл \"{dllPath}\" не найден.");
            return;
        }

        try
        {
            Assembly assembly = Assembly.LoadFrom(dllPath);
            Type? type = assembly.GetType(className);

            if (type == null)
            {
                Console.WriteLine($"Класс \"{className}\" не найден в сборке.");
                return;
            }

            if (!typeof(ICommand).IsAssignableFrom(type))
            {
                Console.WriteLine($"Класс \"{className}\" не реализует ICommand.");
                return;
            }

            
            object[] parameters = constructorArgs.Cast<object>().ToArray();

            
            var instance = Activator.CreateInstance(type, parameters) as ICommand;

            if (instance == null)
            {
                Console.WriteLine("Не удалось создать экземпляр команды.");
                return;
            }

            instance.Execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}


