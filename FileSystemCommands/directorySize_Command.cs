using CommandLib;
using System;
using System.IO;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _path;

    public DirectorySizeCommand(string path)
    {
        _path = path;
    }

    public void Execute()
    {
        long size = GetDirectorySize(new DirectoryInfo(_path));
        Console.WriteLine($"размер каталога \"{_path}\": {size} байт");
    }

    private long GetDirectorySize(DirectoryInfo dir)
    {
        long size = 0;
        foreach (FileInfo file in dir.GetFiles())
        {
            size += file.Length;
        }
        foreach (DirectoryInfo subDir in dir.GetDirectories())
        {
            size += GetDirectorySize(subDir);
        }
        return size;
    }
}

