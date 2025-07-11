using System;
using Task14;
using Xunit;

public class DefiniteIntegralTests
{
    [Fact]
    public void Linear_TwoThreads()
    {
        double result = DefiniteIntegral.Solve(-1, 1, x => x, 1e-4, 2);
        Assert.Equal(0, result, 4);
    }

    [Fact]
    public void Sin_EightThreads()
    {
        double result = DefiniteIntegral.Solve(-1, 1, Math.Sin, 1e-5, 8);
        Assert.Equal(0, result, 4);
    }

    [Fact]
    public void Linear_EightThreads_BigInterval()
    {
        double result = DefiniteIntegral.Solve(0, 5, x => x, 1e-6, 8);
        Assert.Equal(12.5, result, 5);          // ∫₀⁵ x dx = 12.5
    }

    [Fact]
    public void InvalidArguments()
    {
        Func<double, double> f = x => x;

        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(1, -1, f, 1e-4, 2));
        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(0,  1, f, -1e-4, 2));
        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(0,  1, f, 1e-4,  0));
    }
}
