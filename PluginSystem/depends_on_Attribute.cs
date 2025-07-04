using System;

namespace PluginSystem;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute : Attribute
{
    public string PluginName { get; }

    public DependsOnAttribute(string pluginName)
    {
        PluginName = pluginName;
    }
}
