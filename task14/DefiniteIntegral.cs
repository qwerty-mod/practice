using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task14
{
    public static class DefiniteIntegral
    {
        public static double Solve(double left, double right, Func<double, double> function, double step, int threads)
        {
            ValidateParameters(left, right, step, threads);
            double range = right - left;
            int totalSteps = (int)Math.Ceiling(range / step);
            int stepsPerThread = Math.Max(1, totalSteps / threads);

            double result = 0.0;
            var options = new ParallelOptions { MaxDegreeOfParallelism = threads };

            Parallel.For(0, threads, options, () => 0.0, (i, state, localSum) =>
                {
                    int start = i * stepsPerThread;
                    int end = (i == threads - 1) ? totalSteps : (i + 1) * stepsPerThread;
                    end = Math.Min(end, totalSteps);

                    double x = left + start * step;
                    
                    for (int j = start; j < end; j++)
                    {
                        double nextX = Math.Min(x + step, right);
                        localSum += (function(x) + function(nextX)) * (nextX - x) * 0.5;
                        x = nextX;
                    }

                    return localSum;
                },
                localSum => {
                    double initial, computed;
                    do
                    {
                        initial = result;
                        computed = initial + localSum;
                    } 
                    while (initial != Interlocked.CompareExchange(ref result, computed, initial));
                });

            return result;
        }

        public static double SolveSingleThread(double left, double right, Func<double, double> function, double step)
        {
            ValidateParameters(left, right, step, 1);

            double integral = 0.0;
            double x = left;

            while (x < right)
            {
                double nextX = Math.Min(x + step, right);
                integral += (function(x) + function(nextX)) * (nextX - x) * 0.5;
                x = nextX;
            }

            return integral;
        }

        private static void ValidateParameters(double left, double right, double step, int threads)
        {
            if (step <= 0)
                throw new ArgumentOutOfRangeException(nameof(step), "шаг интегрирования должен быть положительным");
            
            if (left > right)
                throw new ArgumentException("левая граница должна быть меньше или равна правой", nameof(left));
            
            if (threads <= 0)
                throw new ArgumentOutOfRangeException(nameof(threads), "кол-во потоков должно быть положительным");
        }
    }
}


