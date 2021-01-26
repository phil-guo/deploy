using System.Collections.Concurrent;
using Chilkat;
using Deploy.Appliction.Internal.Ssh;
using Microsoft.Extensions.Logging;

namespace Deploy.Appliction.Internal.Sftp
{
    public class ChilkatSftp
    {
        public static readonly ConcurrentDictionary<string, Chilkat.SFtp> SshDictionary =
            new ConcurrentDictionary<string, Chilkat.SFtp>();

        private readonly ILogger<ChilkatSftp> _logger;

        public ChilkatSftp(ILogger<ChilkatSftp> logger)
        {
            _logger = logger;
        }

        private SFtp CreateSftp()
        {
            return null;
        }
    }
}