using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentScheduler;

namespace TaskScheduler.Services.BackGroundService
{
    public class JobRegistry : Registry
    {
        
        public JobRegistry(IServiceProvider serviceProvider)
        {
            //    Schedule<SayHiJob>().ToRunNow().AndEvery(21).Seconds();
            Schedule(new SayHiJob(serviceProvider, "Запуск каждую секунду")).ToRunNow().AndEvery(1).Seconds();
            Schedule(new SayHiJob(serviceProvider, "Запуск каждые две секунды")).ToRunNow().AndEvery(2).Seconds();

            Schedule(new SayHiJob(serviceProvider, "Запуск каждую минуту")).ToRunNow().AndEvery(1).Minutes();

            Schedule(new SayHiJob(serviceProvider, "Запуск в 14 50")).ToRunEvery(1).Days().At(14, 50);


            Schedule(new SayHiJob(serviceProvider, "Запуск в 14 52 каэжый четверг")).ToRunNow().AndEvery(1).Months().OnTheSecond(DayOfWeek.Thursday).At(14, 52);
            //Schedule<TestTwoSehdular>().ToRunNow().AndEvery(1).Seconds();
            //Schedule<TestTwoSehdular>().ToRunNow().AndEvery(1).Minutes();
            //Schedule<TestTwoSehdular>().ToRunNow().AndEvery(21).Seconds();

            //// Schedule an IJob to run once, delayed by a specific time interval
            //Schedule<SayHiJob>().ToRunOnceIn(5).Seconds();

            //// Schedule a simple job to run at a specific time
            //Schedule<TestTwoSehdular>().ToRunEvery(1).Days().At(21, 15);

            //// Schedule a more complex action to run immediately and on an monthly interval
            //Schedule<SayHiJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);
        }
    }
}
