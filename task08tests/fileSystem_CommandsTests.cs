using CommandLib;
using FileSystemCommands;
using System;
using System.IO;
using Xunit;

namespace task08tests;

public class FileSystemCommandsTests : IDisposable
{
    private readonly string _testDir;

    public FileSystemCommandsTests()
    {
        _testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(_testDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDir))
            Directory.Delete(_testDir, recursive: true);
    }

    
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        File.WriteAllText(Path.Combine(_testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(_testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(_testDir);
        command.Execute(); // Проверяем, что не возникает исключений
    }

    
    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        File.WriteAllText(Path.Combine(_testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(_testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(_testDir, "*.txt");
        command.Execute(); // Должен найти 1 файл
    }

    

    [Fact]
    public void DirectorySizeCommand_ShouldReturnCorrectSize()
    {
        
        var file1 = Path.Combine(_testDir, "file1.dat");
        var file2 = Path.Combine(_testDir, "file2.dat");
        File.WriteAllBytes(file1, new byte[100]); 
        File.WriteAllBytes(file2, new byte[50]);  

       
        var output = new StringWriter();
        Console.SetOut(output);

        
        var command = new DirectorySizeCommand(_testDir);
        command.Execute();

       
        Assert.Contains("150", output.ToString()); 
    }

    [Fact]
    public void FindFilesCommand_ShouldCorrectlyFilterFiles()
    {
        
        File.WriteAllText(Path.Combine(_testDir, "notes.txt"), "");
        File.WriteAllText(Path.Combine(_testDir, "data.log"), "");
        File.WriteAllText(Path.Combine(_testDir, "readme.md"), "");

      
        var output = new StringWriter();
        Console.SetOut(output);

       
        var command = new FindFilesCommand(_testDir, "*.txt");
        command.Execute();

        
        var result = output.ToString();
        Assert.Contains("notes.txt", result);  
        Assert.DoesNotContain("data.log", result); 
        Assert.DoesNotContain("readme.md", result); 
    }
}