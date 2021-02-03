using System;
using Renci.SshNet;

namespace Deploy.Appliction.Internal
{
    public interface ISsh
    {
        void SendCommands(string cmd);

        void SendCommand(SshClient ssh, string cmd);

        void ExecuteFrontCmd(string dockerName, string imageName);
    }
}