using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace dynamic_ClGenerator
{
    public class CalculatorFactory
    {
        private object calculatorInstance = null!;
        private Type calculatorType = null!;

        public Func<int, int, int> Add { get; private set; } = null!;
        public Func<int, int, int> Minus { get; private set; } = null!;
        public Func<int, int, int> Mul { get; private set; } = null!;
        public Func<int, int, int> Div { get; private set; } = null!;

        private const string ClassCode = @"
        public class Calculator
        {
            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => a / b;
        }";

        public void Generate()
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(ClassCode);

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .Cast<MetadataReference>();

            var assemblyName = "DynamicCalculator_" + Guid.NewGuid().ToString("N");

            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = string.Join("\n", result.Diagnostics.Select(d => d.ToString()));
                throw new Exception("Компиляция не удалась:\n" + errors);
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
            calculatorType = assembly.GetType("Calculator")!;
            calculatorInstance = Activator.CreateInstance(calculatorType)!;

            CreateDelegates();
        }

        private void CreateDelegates()
        {
            Add = CreateDelegate("Add");
            Minus = CreateDelegate("Minus");
            Mul = CreateDelegate("Mul");
            Div = CreateDelegate("Div");
        }

        private Func<int, int, int> CreateDelegate(string methodName)
        {
            var method = calculatorType.GetMethod(methodName)!;
            return (Func<int, int, int>)Delegate.CreateDelegate(
                typeof(Func<int, int, int>), calculatorInstance, method
            );
        }
    }
}

