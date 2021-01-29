using System;
using Autofac;

namespace Deploy.Appliction.Extensions
{
    public class Utils
    {
        public static ILifetimeScope Current { get; set; }
        
        public static Action<string> TextBoxCallback { get; set; }
    }
}