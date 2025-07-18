using System.Collections.Generic;

namespace task18
{
    public class RoundRobinScheduler : IScheduler
    {
        private readonly Queue<ICommand> _queue = new();

        public bool HasCommand() => _queue.Count > 0;

        public ICommand Select() => _queue.Dequeue();

        public void Add(ICommand cmd)
        {
            _queue.Enqueue(cmd);
        }
    }
}
