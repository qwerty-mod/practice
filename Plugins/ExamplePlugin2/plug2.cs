using PluginSystem;
using System;

namespace ExamplePlugin2;

[PluginLoad]
[DependsOn("ExamplePlugin1.Plugin1")]
public class Plugin2 : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("plug2 из ExamplePlugin2 выполнен");
    }
}
