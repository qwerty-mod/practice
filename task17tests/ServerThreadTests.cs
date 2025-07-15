using System;
using Xunit;
using task17;

public class ServerThreadTests : IDisposable
{
    private readonly ServerThread _server;

    public ServerThreadTests() => _server = new ServerThread();

    public void Dispose() => _server.Dispose();

    [Fact]
        public void HardStopCommand_stop_nemedlya()
        {
            var work1 = new TestCommand();
            var hardStop = new HardStopCommand(_server);
            var workAfterStop = new TestCommand();

            _server.EnqueueCommand(work1);
            _server.EnqueueCommand(hardStop);
    

            _server.WaitForCompletion();

            Assert.True(work1.Executed);
            Assert.False(workAfterStop.Executed);  
        }

    [Fact]
    public void SoftStopCommand_stop_posle_opustasheniya()
    {
        var work1 = new TestCommand();
        var work2 = new TestCommand();

        _server.EnqueueCommand(work1);
        _server.EnqueueCommand(new SoftStopCommand(_server));
        _server.EnqueueCommand(work2);

        _server.WaitForCompletion();

        Assert.True(work1.Executed);
        Assert.True(work2.Executed); 
    }

    [Fact]
    public void StopMethods_ThrowWhenCalledFromWrongThread()
    {
        Assert.Throws<InvalidOperationException>(() => _server.HardStop());
        Assert.Throws<InvalidOperationException>(() => _server.SoftStop());
    }

    private class TestCommand : ICommand
    {
        public bool Executed { get; private set; }
        public void Execute() => Executed = true;
    }
}

