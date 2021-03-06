using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Agebull.Common.Rpc;
using Agebull.Common.Tson;
using Agebull.ZeroNet.Core;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;
using ZeroMQ;

namespace Agebull.ZeroNet.PubSub
{
    /// <summary>
    ///     消息发布器
    /// </summary>
    public abstract class SignlePublisher<TData> : ZeroStation
        where TData : class, IPublishData
    {
        #region 广播
        /// <summary>
        /// 构造
        /// </summary>
        protected SignlePublisher() : base(ZeroStationType.Publish, false)
        {
        }

        private readonly List<ZSocket> sockets = new List<ZSocket>();

        private readonly Random random = new Random((int)DateTime.Now.Ticks % int.MaxValue);

        private List<TData> datas;
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="data"></param>
        public void Publish(TData data)
        {
            if (data == null)
                return;
            if (!CanLoop)
            {
                datas.Add(data);
                return;
            }
            PushToLocalQueue(data);
        }
        /// <summary>
        /// 数据写到本地发布队列中
        /// </summary>
        /// <param name="data"></param>
        protected virtual void PushToLocalQueue(TData data)
        {
            int idx = random.Next(0, 64);
            var socket = sockets[idx];
            lock (socket)
            {
                try
                {
                    if (TsonOperator != null)
                    {
                        byte[] buf;
                        using (TsonSerializer serializer = new TsonSerializer())
                        {
                            TsonOperator.ToTson(serializer, data);
                            buf = serializer.Close();
                        }

                        socket.SendTo(ZeroPublishExtend.PubDescriptionTson,
                            data.Title.ToZeroBytes(),
                            GlobalContext.RequestInfo.RequestId.ToZeroBytes(),
                            ZeroApplication.Config.RealName.ToZeroBytes(),
                            buf);
                    }
                    else
                    {
                        socket.SendTo(ZeroPublishExtend.PubDescriptionJson,
                            data.Title.ToZeroBytes(),
                            GlobalContext.RequestInfo.RequestId.ToZeroBytes(),
                            ZeroApplication.Config.RealName.ToZeroBytes(),
                            data.ToZeroBytes());
                    }
                }
                catch (Exception e)
                {
                    ZeroTrace.WriteException("Pub", e, socket.Connects.LinkToString(','),
                        $"Socket Ptr:{socket.SocketPtr}");
                    datas.Add(data);
                }
            }
        }

        /// <inheritdoc />
        protected override bool OnStart()
        {
            _inporcName = $"inproc://{StationName}_{RandomOperate.Generate(8)}.pub";
            return base.OnStart();

        }
        /// <summary>
        /// TSON序列化操作器
        /// </summary>
        protected ITsonOperator<TData> TsonOperator { get; set; }

        /// <inheritdoc />
        protected override void OnRunStop()
        {
            foreach (var socket in sockets)
                socket.TryClose();
            sockets.Clear();
            base.OnRunStop();
        }

        /// <inheritdoc />
        protected override void DoDestory()
        {
            if (datas.Count > 0)
            {
                var file = CacheFileName;
                try
                {
                    File.WriteAllText(file, JsonConvert.SerializeObject(datas));
                }
                catch (Exception e)
                {
                    ZeroTrace.WriteException(StationName, e, file);
                }
            }
            base.DoDestory();
        }

        #endregion

        #region Task

        /// <summary>
        /// 缓存文件名称
        /// </summary>
        private string CacheFileName => Path.Combine(ZeroApplication.Config.DataFolder, $"{StationName}.{Name}.sub.json");


        /// <summary>
        /// 初始化
        /// </summary>
        protected sealed override void Initialize()
        {
            var file = CacheFileName;
            if (File.Exists(file))
            {
                try
                {
                    var json = File.ReadAllText(file);
                    datas = JsonConvert.DeserializeObject<List<TData>>(json);
                }
                catch (Exception e)
                {
                    ZeroTrace.WriteException(StationName, e, file);
                }
            }

            if (datas == null)
                datas = new List<TData>();
        }

        private string _inporcName;


        /// <summary>
        /// 具体执行
        /// </summary>
        /// <returns>返回False表明需要重启</returns>
        protected sealed override bool RunInner(CancellationToken token)
        {
            using (var socket = ZSocket.CreateRequestSocket(Config.RequestAddress, Identity))
            {
                using (var poll = ZmqPool.CreateZmqPool())
                {
                    poll.Prepare(ZPollEvent.In,ZSocket.CreateServiceSocket(_inporcName, ZSocketType.PULL));
                    for (int i = 0; i < 64; i++)
                    {
                        sockets.Add(ZSocket.CreateClientSocket(_inporcName, ZSocketType.PUSH));
                    }
                    SystemManager.Instance.HeartReady(StationName, RealName);
                    State = StationState.Run;
                    Thread.Sleep(5);
                    //历史数据重新入列
                    foreach (var data in datas)
                    {
                        PushToLocalQueue(data);
                    }
                    datas.Clear();
                    File.Delete(CacheFileName);
                    while (true)
                    {
                        if (poll.Poll() && poll.CheckIn(0, out var message))
                            Send(socket, message);
                        else if (!CanLoop)//保证发送完成
                            break;
                    }
                    SystemManager.Instance.HeartLeft(StationName, RealName);
                }
            }
            return true;
        }
        /// <summary>
        /// 执行发送
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        protected virtual void Send(ZSocket socket,ZMessage message)
        {
            if (!socket.SendTo(message))
            {
                ZeroTrace.WriteError(StationName, "Pub", socket.LastError.Text, socket.Connects.LinkToString(','));
            }
        }
        #endregion
    }
}