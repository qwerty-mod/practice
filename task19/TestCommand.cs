namespace task19
{
    public class TestCommand : ICommand
    {
        public int Id { get; }
        public int Counter { get; private set; }

        public bool IsCompleted => Counter >= 3;

        public TestCommand(int id)
        {
            Id = id;
            Counter = 0;
        }

        public void Execute()
        {
            Counter++;
            Console.WriteLine($"Поток {Id} вызов {Counter}");
        }
    }
}
