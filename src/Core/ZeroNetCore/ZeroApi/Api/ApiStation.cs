using ZeroMQ;


namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     Api站点
    /// </summary>
    public class ApiStation : ApiStationBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ApiStation() : base(ZeroStationType.Api, true)
        {

        }

        /// <summary>
        /// 构造Pool
        /// </summary>
        /// <returns></returns>
        protected override IZmqPool PrepareLoop(byte[] identity, out ZSocket socket)
        {
            socket = ZSocket.CreatePoolSocket(Config.WorkerResultAddress, ZSocketType.DEALER, identity);
            socket.ServiceKey = Config.ServiceKey;
            var pSocket = ZeroApplication.Config.ApiRouterModel
                    ? ZSocket.CreatePoolSocket(Config.WorkerCallAddress, ZSocketType.DEALER, identity)
                    : ZSocket.CreatePoolSocket(Config.WorkerCallAddress, ZSocketType.PULL, identity);
            pSocket.ServiceKey = Config.ServiceKey;
            var pool = ZmqPool.CreateZmqPool();
            pool.Prepare(ZPollEvent.In, pSocket, socket);
            return pool;
        }


        /// <summary>
        ///     配置校验
        /// </summary>
        protected override ZeroStationOption GetApiOption()
        {
            var option = ZeroApplication.GetApiOption(StationName);
            if (option.SpeedLimitModel == SpeedLimitType.None)
                option.SpeedLimitModel = SpeedLimitType.ThreadCount;
            return option;
        }



    }
}