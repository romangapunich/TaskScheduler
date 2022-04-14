using System;
using Microsoft.Extensions.Logging;

namespace TaskScheduler.Services.BackGroundService
{
    public class SayHiService : ISayHiService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;

        public SayHiService(ILogger<SayHiService> logger)
        {
            _logger = logger;
        }

        public void SayHi(string TextInfo)
        {
            {
                executionCount++;

                _logger.LogInformation(
                    TextInfo +
                    executionCount + "Time" + DateTime.Now.ToLongDateString());

            }

        }
    }
}
