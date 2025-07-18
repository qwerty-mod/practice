using System;
using System.Collections.Generic;
using System.Threading;

namespace task19
{
    public class Program
    {
        public static void Main()
        {
            var commands = new List<TestCommand>();
            for (int i = 1; i <= 5; i++)
            {
                commands.Add(new TestCommand(i));
            }

            var scheduler = new CommandScheduler(commands);

            while (!commands.All(c => c.IsCompleted))
            {
                Thread.Sleep(100);
            }

            var grafProcessor = new GrafDibProcessor(commands);
            grafProcessor.GenerateStatisticsAndChart();

            Console.WriteLine("Готово! Проверьте файлы statistics.txt и chart.png в папке программы.");
            Console.ReadLine();
        }
    }
}


