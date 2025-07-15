using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task18
{
    
    public class CommandProcessor
    {
        private readonly IScheduler _scheduler;
        private readonly BlockingCollection<ICommand> _incoming = new();
        private readonly Thread _thread;

        public CommandProcessor(IScheduler scheduler)
        {
            _scheduler = scheduler;
            _thread = new Thread(Run)
            {
                IsBackground = true
            };
        }

        public void Enqueue(ICommand cmd) => _incoming.Add(cmd);

        public void Start() => _thread.Start();

        public void Stop() => _incoming.CompleteAdding();

        private void Run()
        {
            while (!_incoming.IsCompleted || _scheduler.HasCommand())
            {
                // попытка взятия команды без блокировки
                if (_incoming.TryTake(out var newCmd, TimeSpan.FromMilliseconds(10)))
                {
                    _scheduler.Add(newCmd);
                }

                // обработка одной итерации long command
                if (_scheduler.HasCommand())
                {
                    var cmd = _scheduler.Select();
                    bool finished = cmd.Execute();

                    if (!finished)
                        _scheduler.Add(cmd);
                }
                else
                {
                    // сон
                    Thread.Sleep(5);
                }
            }
        }
    }
}
