using System;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Rpc;
using Agebull.ZeroNet.Core;

namespace RpcTest
{
    internal abstract class Tester
    {
        public CancellationToken Token => Cancel.Token;
        public CancellationTokenSource Cancel {get;set;}

        public long ExCount;
        public long BlError;
        public long NetError;
        public long ExError;
        public long TmError;
        public long WkError;
        public long RunTime;
        public DateTime Start;
        public int WaitCount;

        public static readonly string Api;
        public static readonly string Arg;
        public static readonly string Host;

        public static readonly string Station;

        static Tester()
        {
            var sec = ConfigurationManager.Get("ApiTest");
            if (sec.IsEmpty)
                throw new Exception("�Ҳ���ApiTest���ý�");
            Station = sec["Station"];
            Api = sec["Api"];
            Arg = sec["Argument"];
            Host = sec["HttpRouteAddress"];
        }

        public abstract bool Init();

        public static void OnZeroEvent(object sender, ZeroNetEventArgument e)
        {
            switch (e.Event)
            {
                case ZeroNetEventType.AppRun:
                    ZeroTrace.SystemLog("RpcTest", "Test is start");
                    var test = IocHelper.Create<Tester>();
                    test.StartTest();
                    break;
                case ZeroNetEventType.AppStop:
                    ZeroTrace.SystemLog("RpcTest", "Test is stop");
                    test = IocHelper.Create<Tester>();
                    test.Cancel.Cancel();
                    break;
            }
        }

        private void StartTest()
        {
            WaitToEnd();
            if (!Init())
                return;

            ZeroTrace.SystemLog("RpcTest", "Test is start");
            Start = DateTime.Now;
            Cancel = new CancellationTokenSource();
            var option = ZeroApplication.GetClientOption(Station);
            switch (option.SpeedLimitModel)
            {
                case SpeedLimitType.Single:
                    new Thread(Test).Start();
                    break;
                case SpeedLimitType.ThreadCount:
                    int max = (int)(Environment.ProcessorCount * option.TaskCpuMultiple);
                    if (max < 1)
                        max = 1;
                    for (int idx = 0; idx < max; idx++)
                    {
                        new Thread(Test).Start();
                    }
                    break;
                default:
                    Task.Factory.StartNew(TestSync, Cancel.Token);
                    break;
            }
        }

        void Async()
        {
            Interlocked.Increment(ref WaitCount);
            GlobalContext.SetRequestContext(ZeroApplication.Config.ServiceName, Guid.NewGuid().ToString("N"));
            DateTime s = DateTime.Now;

            DoAsync();

            Interlocked.Decrement(ref WaitCount);
            var sp = (DateTime.Now - s);
            Interlocked.Add(ref RunTime, sp.Ticks);
            if (sp.TotalMilliseconds > 500)
                Interlocked.Increment(ref TmError);
            if ((Interlocked.Increment(ref ExCount) % 100) == 0)
                Count();
        }
        protected abstract void DoAsync();

        private int testerCount = 0;

        void WaitToEnd()
        {
            while (Cancel != null)
                Thread.Sleep(10);
        }

        void OnTestStar()
        {
            Interlocked.Increment(ref testerCount);
        }
        void OnTestEnd()
        {
            if (Interlocked.Decrement(ref testerCount) == 0)
            {
                Cancel.Dispose();
                Cancel = null;
            }
        }

        public void TestSignle()
        {
            Thread.Sleep(3000);
            ZeroTrace.SystemLog("RpcTest", "Tester.Test", Task.CurrentId, "Start");
            Start = DateTime.Now;
            OnTestStar();
            while (!Token.IsCancellationRequested && ZeroApplication.InRun)
            {
                if (WaitCount > ZeroApplication.Config.MaxWait)
                {
                    Thread.Sleep(10);
                    continue;
                }
                Async();
            }
            Count();
            OnTestEnd();
        }
        public void Test()
        {
            Thread.Sleep(1000);
            ZeroTrace.SystemLog("RpcTest", "Tester.Test", Task.CurrentId, "Start");
            Start = DateTime.Now;
            OnTestStar();
            while (!Token.IsCancellationRequested && ZeroApplication.InRun)
            {
                Thread.Sleep(10);
                Async();
            }
            Count();
            OnTestEnd();
        }

        public void Test1()
        {
            Thread.Sleep(100);
            ZeroTrace.SystemLog("RpcTest", "Tester.Test", Task.CurrentId, "Start");
            Start = DateTime.Now;
            OnTestStar();
            while (!Token.IsCancellationRequested && ZeroApplication.InRun)
            {
                if (WaitCount > ZeroApplication.Config.MaxWait)
                {
                    Thread.Sleep(10);
                    continue;
                }
                Async();
            }
            Count();
            OnTestEnd();
        }
        public void TestSync()
        {
            Thread.Sleep(100);
            ZeroTrace.SystemLog("RpcTest", "Tester.TestSync", Task.CurrentId, "Start");

            OnTestStar();
            while (!Token.IsCancellationRequested && ZeroApplication.InRun)
            {
                if (WaitCount > ZeroApplication.Config.MaxWait)
                {
                    Thread.Sleep(10);
                    continue;
                }
                Task.Factory.StartNew(Async, CancellationToken.None);
            }
            Count();
            OnTestEnd();
        }

        public void Count()
        {
            TimeSpan ts = TimeSpan.FromTicks(RunTime);
            GC.Collect();
            ZeroTrace.SystemLog("Count", ExCount,
                $"{ts.TotalMilliseconds / ExCount}ms | {ExCount / (DateTime.Now - Start).TotalSeconds}/s",
                "Error", $"net:{NetError:D8} | worker:{WkError:D8} | time out:{TmError:D8} | bug:{BlError:D8}");
        }
    }
}