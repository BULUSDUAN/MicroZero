﻿using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.MicroZero;
using ApiTest;
using MicroZero.Http.Gateway;


namespace QueueTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ZeroApplication.CheckOption();
            ZeroApplication.RegistZeroObject<PayCallbackController>();
            ZeroApplication.Initialize();

            var senparcStartup = new SenparcStartup
            {
                Configuration = ConfigurationManager.Root
            };
            senparcStartup.ConfigureServices(IocHelper.ServiceCollection);
            ZeroApplication.RunAwaite();
        }
    }
}
