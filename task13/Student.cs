using System;
using System.Collections.Generic;

namespace task13
{
    public class Subject
    {
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; }
    }

    public class Student
    {
        public Student()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Grades = new List<Subject>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Subject> Grades { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new InvalidOperationException("имя не может быть пустым");

            if (string.IsNullOrWhiteSpace(LastName))
                throw new InvalidOperationException("фамилия не может быть пустой");

            if (BirthDate > DateTime.Now)
                throw new InvalidOperationException("дата рождения не может быть в будущем");

            if (Grades == null)
                throw new InvalidOperationException("список оценок пуст");

            foreach (var subject in Grades)
            {
                if (subject == null)
                    throw new InvalidOperationException("список оценок содержит ноль");

                if (subject.Grade < 1 || subject.Grade > 5)
                    throw new InvalidOperationException($"оценка {subject.Grade} выходит за пределы диапазона");
            }
        }
    }
}
