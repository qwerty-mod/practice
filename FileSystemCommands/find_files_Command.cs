using CommandLib;
using System;
using System.IO;

namespace FileSystemCommands;

public class FindFilesCommand : ICommand
{
    private readonly string _path;
    private readonly string _pattern;

    public FindFilesCommand(string path, string pattern)
    {
        _path = path;
        _pattern = pattern;
    }

    public void Execute()
    {
        var files = Directory.GetFiles(_path, _pattern);
        Console.WriteLine($"нашлось {files.Length} файл(ов) по маске \"{_pattern}\" в \"{_path}\":");
        foreach (var file in files)
        {
            Console.WriteLine(file);
        }
    }
}
