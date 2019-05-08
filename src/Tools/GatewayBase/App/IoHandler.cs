using System;
using System.Text;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;

using Agebull.MicroZero;
using Agebull.MicroZero.PubSub;
using Agebull.MicroZero.ZeroApis;
using Agebull.EntityModel.Common;
using Newtonsoft.Json;
using Agebull.Common.Context;

namespace MicroZero.Http.Gateway
{
    /// <summary>
    ///     Http进站出站的处理类
    /// </summary>
    public sealed class IoHandler
    {
        /// <summary>
        ///     开始时的处理
        /// </summary>
        /// <returns>如果返回内容不为空，直接返回,后续的处理不再继续</returns>
        public static void OnBegin(RouteData data)
        {
            data.Start = DateTime.Now;
            BeginZeroTrace(data);
            BeginMonitor(data);
        }

        /// <summary>
        ///     结束时的处理
        /// </summary>
        public static void OnEnd(RouteData data)
        {
            data.End = DateTime.Now;
            EndMonitor(data);
            //CountApi(data);
            EndZeroTrace(data);
        }

        #region LogMonitor

        /// <summary>
        ///     开始日志监测
        /// </summary>
        /// <param name="data"></param>
        private static void BeginMonitor(RouteData data)
        {
            if (!LogRecorderX.LogMonitor)
                return;
            try
            {
                LogRecorderX.BeginMonitor(data.Uri.ToString());
                LogRecorderX.BeginStepMonitor("HTTP");
                var args = new StringBuilder();
                args.Append("Headers：");
                args.Append(JsonHelper.SerializeObject(data.Headers));
                LogRecorderX.MonitorTrace(args.ToString());
                LogRecorderX.MonitorTrace($"Method：{data.HttpMethod}");
                LogRecorderX.MonitorTrace($"RequestId：{GlobalContext.RequestInfo.RequestId }");
                LogRecorderX.MonitorTrace($"Command:{data.ApiHost}/{data.ApiName}");
                if (data.Arguments.Count > 0)
                    LogRecorderX.MonitorTrace($"Arguments：{JsonHelper.SerializeObject(data.Arguments)}");
                if (!string.IsNullOrWhiteSpace(data.HttpContext))
                    LogRecorderX.MonitorTrace("Context:" + data.HttpContext);
            }
            catch (Exception e)
            {
                LogRecorderX.MonitorTrace(e.Message);
                LogRecorderX.Exception(e);
            }
            finally
            {
                LogRecorderX.EndStepMonitor();
            }
        }

        /// <summary>
        ///     开始计数
        /// </summary>
        /// <returns></returns>
        private static void EndMonitor(RouteData data)
        {
            if (!LogRecorderX.LogMonitor)
                return;
            try
            {
                LogRecorderX.MonitorTrace($"UserState : {data.UserState}");
                LogRecorderX.MonitorTrace($"ZeroState : {data.ZeroState}");
                LogRecorderX.MonitorTrace($"Result：{data.ResultMessage}");
            }
            catch (Exception e)
            {
                LogRecorderX.MonitorTrace(e.Message);
                LogRecorderX.Exception(e);
            }
            finally
            {
                LogRecorderX.EndMonitor();
            }
        }

        #endregion

        #region ZeroTrace


        /// <summary>
        ///     订阅时的标准网络数据说明
        /// </summary>
        private static readonly byte[] Description =
        {
            6,
            (byte)ZeroByteCommand.General,
            ZeroFrameType.Command,
            ZeroFrameType.TextContent,
            ZeroFrameType.RequestId,
            ZeroFrameType.Station,
            ZeroFrameType.Requester,
            ZeroFrameType.SerivceKey,
            ZeroFrameType.End,
            1
        };

        private static readonly byte[] WebNameBytes = "WebClient".ToZeroBytes();


        

        /// <summary>
        /// 发送广播
        /// </summary>
        /// <returns></returns>
        static void BeginZeroTrace(RouteData data)
        {
            var socket = ZeroConnectionPool.GetSocket("TraceDispatcher", RandomOperate.Generate(8));
            if (socket?.Socket == null)
            {
                return;
            }
            using (socket)
            {
                var result = socket.Socket.Publish("Http", Description,
                    $"{data.ApiHost}/{data.ApiName}".ToZeroBytes(),
                    data.ToZeroBytes(),
                    GlobalContext.RequestInfo.RequestId.ToZeroBytes(),
                    ZeroCommandExtend.AppNameBytes,
                    WebNameBytes,
                    ZeroCommandExtend.ServiceKeyBytes);
                if (result.InteractiveSuccess && result.State == ZeroOperatorStateType.Ok)
                {
                    GlobalContext.Current.Request.LocalGlobalId = result.GlobalId;
                }
            }
        }

        /// <summary>
        /// 发送广播
        /// </summary>
        /// <returns></returns>
        static void EndZeroTrace(RouteData data)
        {
            var socket = ZeroConnectionPool.GetSocket("TraceDispatcher", RandomOperate.Generate(8));
            if (socket?.Socket == null)
            {
                return;
            }

            var desc = new byte[]
            {
                9,
                (byte)data.ZeroState,
                ZeroFrameType.Command,
                ZeroFrameType.TextContent,
                ZeroFrameType.TextContent,
                ZeroFrameType.Station,
                ZeroFrameType.Requester,
                ZeroFrameType.RequestId,
                ZeroFrameType.GlobalId,
                ZeroFrameType.CallId,
                ZeroFrameType.SerivceKey,
                3
            };
            var result = data.ResultMessage;
            data.ResultMessage = null;//拒绝重复传输
            data.Headers = null;//拒绝重复传输
            using (socket)
            {
                socket.Socket.Publish("Http", desc,
                    $"{data.ApiHost}/{data.ApiName}".ToZeroBytes(),
                    result.ToZeroBytes(),
                    data.ToZeroBytes(),
                    ZeroCommandExtend.AppNameBytes,
                    WebNameBytes,
                    GlobalContext.RequestInfo.RequestId.ToZeroBytes(),
                    GlobalContext.RequestInfo.LocalGlobalId.ToZeroBytes(),
                    GlobalContext.RequestInfo.CallGlobalId.ToZeroBytes(),
                    ZeroCommandExtend.ServiceKeyBytes);
            }
        }
        #endregion

        //private static int count, error, success;
        /// <summary>
        ///     开始计数
        /// </summary>
        /// <returns></returns>
        private static void CountApi(RouteData data)
        {
            var counter = IocHelper.Create<IApiCounter>();
            if (counter == null || !counter.IsEnable)
                return;
            counter.Count(new CountData
            {
                Start = data.Start.Ticks,
                End = DateTime.Now.Ticks,
                HostName = data.ApiHost,
                ApiName = data.ApiName,
                Status = data.UserState,
                Requester = $"http_route={GlobalContext.RequestInfo.Ip}:{GlobalContext.RequestInfo.Port}"
            });
            //ZeroTrace.WriteLoop("Run", $"count:{count} success{success} error{error}");
        }
    }
}