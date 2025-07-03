using System;
using System.Reflection;
using task07.Attributes;
namespace task07
{
    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            // проверка DisplayName для класса
            var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttr != null)
            {
                Console.WriteLine($"Отображаемое имя класса: {displayNameAttr.DisplayName}");
            }

            // проверка Version для класса
            var versionAttr = type.GetCustomAttribute<VersionAttribute>();
            if (versionAttr != null)
            {
                Console.WriteLine($"Версия класса: {versionAttr.Major}.{versionAttr.Minor}");
            }

            Console.WriteLine("\nМетоды:");
            // получение всех публичных методов
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                var methodDisplayName = method.GetCustomAttribute<DisplayNameAttribute>();
                Console.WriteLine($"- {method.Name}: {(methodDisplayName != null ? methodDisplayName.DisplayName : "Нет атрибута DisplayName")}");
            }

            Console.WriteLine("\nСвойства:");
            
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var property in properties)
            {
                var propDisplayName = property.GetCustomAttribute<DisplayNameAttribute>();
                Console.WriteLine($"- {property.Name}: {(propDisplayName != null ? propDisplayName.DisplayName : "Нет атрибута DisplayName")}");
            }
        }
    }
}