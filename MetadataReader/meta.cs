using System;
using System.Reflection;

if (args.Length == 0)
{
    Console.WriteLine("укажите путь к сборке в параметрах командной строки.");
    return;
}

string assemblyPath = args[0];

if (!File.Exists(assemblyPath))
{
    Console.WriteLine($"файл не найден: {assemblyPath}");
    return;
}

try
{
    Assembly assembly = Assembly.LoadFrom(assemblyPath);

    Console.WriteLine($"сборка: {assembly.FullName}");

    foreach (Type type in assembly.GetTypes())
    {
        Console.WriteLine($"\n класс: {type.FullName}");

        // атр. класса
        foreach (var attr in type.GetCustomAttributes())
        {
            Console.WriteLine($"  атрибут: {attr.GetType().Name}");
        }

        // констр.
        foreach (ConstructorInfo ctor in type.GetConstructors())
        {
            Console.WriteLine($"\nконструктор:");
            foreach (var param in ctor.GetParameters())
            {
                Console.WriteLine($"{param.ParameterType.Name} {param.Name}");
            }
        }

        // методы
        foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            Console.WriteLine($"\nметод: {method.Name}");

            // атр. метода
            foreach (var attr in method.GetCustomAttributes())
            {
                Console.WriteLine($"атрибут: {attr.GetType().Name}");
            }

            foreach (var param in method.GetParameters())
            {
                Console.WriteLine($"{param.ParameterType.Name} {param.Name}");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ошибка: {ex.Message}");
}

