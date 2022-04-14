using System;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TaskScheduler.Services.BackGroundService
{
    public class SayHiJob : IJob
    {
        private readonly ISayHiService _sayHiService;
        private string TextInfo;

        public SayHiJob(IServiceProvider serviceProvider,string textInfo)
        {
            _sayHiService = serviceProvider.GetRequiredService<ISayHiService>();
            TextInfo = textInfo;
        }
        public void Execute()
        {
            _sayHiService.SayHi(TextInfo);
        }
    }
}
