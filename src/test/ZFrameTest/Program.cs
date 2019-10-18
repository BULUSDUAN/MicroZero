﻿using Agebull.MicroZero;
using RpcTest;
using ZeroMQ;

namespace ZFrameTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ZContext.Initialize();
            Tester.StartTest();
            ZContext.Destroy();

            //ZeroApplication.CheckOption();
            //ZeroApplication.RegistZeroObject<HisSummarySubscribe>();
            //ZeroApplication.Initialize();
            //ZeroApplication.RunAwaite();
        }
    }
}
