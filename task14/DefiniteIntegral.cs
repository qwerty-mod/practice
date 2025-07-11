using System;
using System.Threading;

namespace Task14
{
    public static class DefiniteIntegral
    {
        public static double Solve(double leftgr, double rightgr, Func<double, double> function, double step, int kolvo_potokov)
        {
            if (kolvo_potokov <= 0)
                throw new ArgumentException("threadsNumber должен быть положительным", nameof(kolvo_potokov));

            if (step <= 0)
                throw new ArgumentException("шаг должен быть положительным", nameof(step));

            if (leftgr > rightgr)
                throw new ArgumentException(" левая граница(leftgr) должна быть ≤ правая граница(rightgr) ");

            double totalIntegral = 0.0;
            double segmentLength = (rightgr - leftgr) / kolvo_potokov;

            using (var barrier = new Barrier(kolvo_potokov + 1)) // +1 — основной поток
            {
                for (int i = 0; i < kolvo_potokov; i++)
                {
                    double start = leftgr + i * segmentLength;
                    double end = (i == kolvo_potokov - 1) ? rightgr : start + segmentLength;

                    Thread thread = new Thread(() =>
                    {
                        double partial = CalculatePartialIntegral(start, end, function, step);
                        AtomicAdd(ref totalIntegral, partial);
                        barrier.SignalAndWait();
                    });

                    thread.IsBackground = true;
                    thread.Start();
                }

                barrier.SignalAndWait(); // ожидаем завершения всех потоков
            }

            return totalIntegral;
        }

        private static void AtomicAdd(ref double target, double value)
        {
            double initial, computed;
            do
            {
                initial = target;
                computed = initial + value;
            }
            while (Interlocked.CompareExchange(ref target, computed, initial) != initial);
        }

        private static double CalculatePartialIntegral(double leftgr, double rightgr, Func<double, double> f, double step)
        {
            double integral = 0.0;
            double x = leftgr;

            while (x < rightgr)
            {
                double xNext = Math.Min(x + step, rightgr);
                integral += (f(x) + f(xNext)) * (xNext - x) / 2.0;
                x = xNext;
            }

            return integral;
        }
    }
}


