using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class ClassAnalyzer
{
    private readonly Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public IEnumerable<string> GetPublicMethods()
        => _type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
               .Select(m => m.Name);

    public IEnumerable<string> GetMethodParams(string methodName)
        => _type.GetMethod(methodName)?
               .GetParameters()
               .Select(p => p.Name)
            ?? Enumerable.Empty<string>();

    public IEnumerable<string> GetAllFields()
        => _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
               .Select(f => f.Name);

    public IEnumerable<string> GetProperties()
        => _type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Select(p => p.Name);

    public bool HasAttribute<T>() where T : Attribute
        => _type.GetCustomAttribute<T>() != null;
}