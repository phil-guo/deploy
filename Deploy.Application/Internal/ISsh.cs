using System;

namespace Deploy.Appliction.Internal
{
    public interface ISsh : IDisposable
    {
        void SendCommands(string cmd);

        void ExecuteFrontCmd(string dockerName, string imageName);
    }
}