using System.Collections.Concurrent;
using System.Threading;

namespace task19;

public class CommandScheduler
{
    private readonly ConcurrentQueue<ICommand> queue = new();
    private readonly Thread workerThread;
    private readonly AutoResetEvent newCommandEvent = new(false);
    private volatile bool running = true;

    private readonly List<TestCommand> allCommands;

    public CommandScheduler(List<TestCommand> commands)
    {
        allCommands = commands;
        foreach (var cmd in commands)
            queue.Enqueue(cmd);

        workerThread = new Thread(WorkerLoop);
        workerThread.Start();
    }

    private void WorkerLoop()
    {
        while (running)
        {
            if (queue.TryDequeue(out var command))
            {
                command.Execute();

                
                if (command is TestCommand testCmd && !testCmd.IsCompleted)
                {
                    queue.Enqueue(command);
                }

                Thread.Sleep(300);
            }
            else
            {
                // точно ли все комнды завершены?
                if (allCommands.All(c => c.IsCompleted))
                {
                    HardStop();
                }
                else
                {
                    newCommandEvent.WaitOne();
                }
            }
        }
    }

    public void HardStop()
    {
        running = false;
        newCommandEvent.Set(); 
        workerThread.Join();   
    }
}

