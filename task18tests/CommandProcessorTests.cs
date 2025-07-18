using System;
using System.Threading.Tasks;
using NUnit.Framework;
using task18;
using System.Linq;
using ScottPlot;

namespace task18tests  
{
    [TestFixture]
    public class CommandProcessorTests
    {
        private sealed class CountingCommand : ICommand
        {
            private readonly int _stepsToFinish;
            private int _step;
            private readonly TaskCompletionSource<bool> _tcs;

            public CountingCommand(int stepsToFinish, TaskCompletionSource<bool> tcs)
            {
                _stepsToFinish = stepsToFinish;
                _tcs = tcs;
            }

            public bool Execute()
            {
                _step++;
                if (_step >= _stepsToFinish)
                {
                    _tcs.TrySetResult(true);
                    return true;
                }
                return false;
            }
        }

        private sealed class InstantCommand : ICommand
        {
            private readonly TaskCompletionSource<bool> _tcs;
            public InstantCommand(TaskCompletionSource<bool> tcs) => _tcs = tcs;

            public bool Execute()
            {
                _tcs.TrySetResult(true);
                return true;
            }
        }

        [Test]
        public async Task LongCommand_v_itoge_zavershaetsa()
        {
            var finished = new TaskCompletionSource<bool>();
            var scheduler = new RoundRobinScheduler();
            var processor = new CommandProcessor(scheduler);

            processor.Start();
            processor.Enqueue(new CountingCommand(5, finished));

            Assert.That(await finished.Task.WaitAsync(TimeSpan.FromSeconds(2)), Is.True);
            processor.Stop();
        }

        [Test]
        public async Task RoundRobin()
        {
            var done1 = new TaskCompletionSource<bool>();
            var done2 = new TaskCompletionSource<bool>();

            var scheduler = new RoundRobinScheduler();
            var processor = new CommandProcessor(scheduler);
            processor.Start();

            processor.Enqueue(new CountingCommand(7, done1));
            processor.Enqueue(new CountingCommand(7, done2));

            bool[] results = await Task.WhenAll(done1.Task, done2.Task).WaitAsync(TimeSpan.FromSeconds(3));
            Assert.That(results.All(r => r), Is.True);

            processor.Stop();
        }

        [Test]
        public async Task InstantCommand_finish_do_LongCommand()
        {
            var instantDone = new TaskCompletionSource<bool>();
            var longDone = new TaskCompletionSource<bool>();

            var scheduler = new RoundRobinScheduler();
            var processor = new CommandProcessor(scheduler);
            processor.Start();

            processor.Enqueue(new InstantCommand(instantDone));
            processor.Enqueue(new CountingCommand(10, longDone));

            Assert.That(await instantDone.Task.WaitAsync(TimeSpan.FromMilliseconds(200)), Is.True);
            Assert.That(longDone.Task.IsCompleted, Is.False);

            Assert.That(await longDone.Task.WaitAsync(TimeSpan.FromSeconds(3)), Is.True);
            processor.Stop();
        }

        [Test]
        public async Task Processor_ContinuesWhenQueueEmpty()
        {
            var done = new TaskCompletionSource<bool>();

            var scheduler = new RoundRobinScheduler();
            var processor = new CommandProcessor(scheduler);
            processor.Start();

            processor.Enqueue(new CountingCommand(8, done));

            Assert.That(await done.Task.WaitAsync(TimeSpan.FromSeconds(3)), Is.True);
            processor.Stop();
        }

    }
}

