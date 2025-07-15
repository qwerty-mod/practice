using System;

namespace task17
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _serverThread;

        public HardStopCommand(ServerThread serverThread)
            => _serverThread = serverThread ?? throw new ArgumentNullException(nameof(serverThread));

        public void Execute() => _serverThread.HardStop();
    }

    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _serverThread;

        public SoftStopCommand(ServerThread serverThread)
            => _serverThread = serverThread ?? throw new ArgumentNullException(nameof(serverThread));

        public void Execute() => _serverThread.SoftStop();
    }
}
