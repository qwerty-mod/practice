using PluginSystem;
using System;

namespace ExamplePlugin1;

[PluginLoad]
public class Plugin1 : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("plug1 из ExamplePlugin1 выполнен");
    }
}
