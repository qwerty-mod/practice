using Xunit;
using dynamic_ClGenerator;

namespace calculatorTests
{
    public class CalculatorFactoryTests
    {
        [Fact]
        public void TestAdd()
        {
            var factory = new CalculatorFactory();
            factory.Generate();
            Assert.Equal(5, factory.Add(2, 3));
        }

        [Fact]
        public void TestMinus()
        {
            var factory = new CalculatorFactory();
            factory.Generate();
            Assert.Equal(1, factory.Minus(3, 2));
        }

        [Fact]
        public void TestMul()
        {
            var factory = new CalculatorFactory();
            factory.Generate();
            Assert.Equal(6, factory.Mul(2, 3));
        }

        [Fact]
        public void TestDiv()
        {
            var factory = new CalculatorFactory();
            factory.Generate();
            Assert.Equal(2, factory.Div(6, 3));
        }
    }
}

