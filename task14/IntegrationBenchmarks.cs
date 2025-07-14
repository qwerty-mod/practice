
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Task14;          
using ScottPlot;       

namespace Task14        
{
    
    public static class IntegrationBenchmarks
    {
       
        private static readonly double[] Steps =
            { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };

        private const double RequiredAccuracy = 1e-4;
        private const int    Repeats          = 10;   

        
        public static double PickMinimalStep()
        {
            const double reference = 0.0;            
            foreach (double h in Steps)
            {
                double approx =
                    DefiniteIntegral.SolveSingleThread(-100, 100, Math.Sin, h);

                if (Math.Abs(approx - reference) <= RequiredAccuracy)
                    return h;
            }

            throw new InvalidOperationException("ни один шаг не удовлетворил требуемой точности 1e‑4");
        }

       
        public static double BenchmarkSingleThread(double step)
        {
            double total = 0;
            for (int i = 0; i < Repeats; i++)
            {
                var sw = Stopwatch.StartNew();
                DefiniteIntegral.SolveSingleThread(-100, 100, Math.Sin, step);
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / Repeats;
        }

        public static List<(int threads, double timeMs)> BenchmarkThreads(double step)
        {
            int maxThreads = Environment.ProcessorCount * 2;
            var data       = new List<(int, double)>(maxThreads);

            for (int t = 1; t <= maxThreads; t++)
            {
                double sum = 0;
                for (int i = 0; i < Repeats; i++)
                {
                    var sw = Stopwatch.StartNew();
                    DefiniteIntegral.Solve(-100, 100, Math.Sin, step, t);
                    sw.Stop();
                    sum += sw.Elapsed.TotalMilliseconds;
                }
                data.Add((t, sum / Repeats));
            }
            return data;
        }

        
        public static void SavePerformancePlot(
            List<(int threads, double timeMs)> data,
            string filePath)
        {
            if (data is null || data.Count == 0)
                throw new ArgumentException("нет данных для построения графика", nameof(data));

            var plt = new Plot();
            double[] xs = data.Select(p => (double)p.threads).ToArray();
            double[] ys = data.Select(p => p.timeMs).ToArray();

            plt.Add.Scatter(xs, ys);

            plt.Title("производительность вычисления интеграла");
            plt.XLabel("кол-во потоков");
            plt.YLabel("время (мс)");

            plt.SavePng(filePath, 800, 600);  
        }

        
        public static void SaveBenchmarkResults(
            double step,
            int    optimalThreads,
            double singleThreadTime,
            double multiThreadTime)
        {
            double speedup = (singleThreadTime - multiThreadTime) / singleThreadTime * 100.0;

            if (speedup < 15.0)
                throw new InvalidOperationException($"ускорение {speedup:F1}% < 15% - оптимизируйте многопоточную версию");

            string report = $"""
                РЕЗУЛЬТАТЫ ТЕСТИРОВАНИЯ
                -----------------------
                требуемая точность      : 1e-4
                подобранный шаг         : {step:e3}
                оптимальное # потоков   : {optimalThreads}
                время (1 поток)         : {singleThreadTime:F2} мс
                время (многопоточное)   : {multiThreadTime:F2} мс
                Ускорение               : {speedup:F1} %
                """;

            File.WriteAllText("benchmark_results.txt", report);
        }
    }
}

