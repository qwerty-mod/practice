using System.Collections.Generic;
using NUnit.Framework;

public class StudentServiceTests
{
    private List<Student> _testStudents;
    private StudentService _service;

    [SetUp]
    public void Setup()
    {
        _testStudents = new List<Student>
        {
            new Student { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new Student { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new Student { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Test]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.TrueForAll(s => s.Faculty == "ФИТ"));
    }

    [Test]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.AreEqual("Экономика", result);
    }

    [Test]
    public void GetStudentsWithMinAverageGrade_ReturnsFilteredStudents()
    {
        var result = _service.GetStudentsWithMinAverageGrade(4.5).ToList();
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.Exists(s => s.Name == "Иван"));
        Assert.IsTrue(result.Exists(s => s.Name == "Петр"));
        Assert.IsFalse(result.Exists(s => s.Name == "Анна"));
    }

    [Test]
    public void GetStudentsOrderedByName_ReturnsAlphabeticalOrder()
    {
        var result = _service.GetStudentsOrderedByName().ToList();
        Assert.AreEqual("Анна", result[0].Name);
        Assert.AreEqual("Иван", result[1].Name);
        Assert.AreEqual("Петр", result[2].Name);
    }

    [Test]
    public void GroupStudentsByFaculty_ReturnsCorrectGroups()
    {
        var result = _service.GroupStudentsByFaculty();
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(2, result["ФИТ"].Count());
        Assert.AreEqual(1, result["Экономика"].Count());
    }
}