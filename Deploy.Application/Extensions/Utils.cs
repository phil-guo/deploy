using System;
using Autofac;
using Microsoft.Extensions.Logging;

namespace Deploy.Appliction.Extensions
{
    public class Utils
    {
        public static ILifetimeScope Current { get; set; }

        public static Action<string> TextBoxCallback { get; set; }


        public static void TryCatchAction(Action action)
        {
            var logger = Current.Resolve<ILogger<Utils>>();
            try
            {
                action();
            }
            catch (Exception e)
            {
                logger.LogInformation(e.Message);
            }
        }
    }
}