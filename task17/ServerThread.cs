using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace task17
{
    public class ServerThread : IDisposable
    {
        private readonly BlockingCollection<ICommand> _commandQueue = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly Task _processingTask;
        private IExceptionHandler? _exceptionHandler;
        private int _managedThreadId;

        public ServerThread()
        {
            var threadReady = new ManualResetEventSlim();

            _processingTask = Task.Run(() =>
            {
                _managedThreadId = Thread.CurrentThread.ManagedThreadId;
                threadReady.Set();
                ProcessCommands(_cts.Token);
            });

            threadReady.Wait();
        }

        public void SetExceptionHandler(IExceptionHandler handler) => _exceptionHandler = handler;

        public void EnqueueCommand(ICommand command)
        {
            if (command is null) throw new ArgumentNullException(nameof(command));
            if (_commandQueue.IsAddingCompleted || _cts.IsCancellationRequested)
                throw new InvalidOperationException("невозможно добавить команду после остановки сервера");

            _commandQueue.Add(command, _cts.Token);
        }

        private void ProcessCommands(CancellationToken ct)
        {
            try
            {
                foreach (var command in _commandQueue.GetConsumingEnumerable(ct))
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception ex) when (!ct.IsCancellationRequested)
                    {
                        _exceptionHandler?.Handle(ex, command);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // норм завершение при HardStop
            }
        }

        public void SoftStop()
        {
            EnsureCalledFromServerThread();
            _commandQueue.CompleteAdding();
        }

        public void HardStop()
        {
            EnsureCalledFromServerThread();
            _cts.Cancel();
            _commandQueue.CompleteAdding();    // анлок ожидание
        }

        public void WaitForCompletion(int millisecondsTimeout = 1000) =>
            _processingTask.Wait(millisecondsTimeout);

        private void EnsureCalledFromServerThread()
        {
            if (Thread.CurrentThread.ManagedThreadId != _managedThreadId)
                throw new InvalidOperationException("команда остановки должна выполняться в серверном потоке");
        }

        public void Dispose()
        {
            try
            {
                _cts.Cancel();
                _commandQueue.CompleteAdding();
                _processingTask.Wait(500);
            }
            finally
            {
                _commandQueue.Dispose();
                _cts.Dispose();
            }
        }
    }
}

