using System;
using task09.Attributes;

namespace task09
{
    [DisplayName("Пример класса")]
    [Version(1, 0)]
    public class SampleClass
    {
        [DisplayName("Числовое свойство")]
        public int Number { get; set; }

        [DisplayName("Тестовый метод")]
        public void TestMethod()
        {
        }
    }
}