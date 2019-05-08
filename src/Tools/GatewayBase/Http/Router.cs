using System.Threading.Tasks;
using Agebull.Common.Logging;

namespace MicroZero.Http.Gateway
{
    /// <summary>
    ///     ����ӳ�������
    /// </summary>
    internal partial class Router
    {
        /// <summary>
        ///     Զ�̵���
        /// </summary>
        /// <returns></returns>
        private async Task<string> CallHttp()
        {
            if (!(Data.RouteHost is HttpHost host))
            {
                LogRecorderX.MonitorTrace("Host Type Failed");
                return Data.ResultMessage;
            }
            // ��ǰ������õ�ģ�Ͷ�Ӧ����������
            string httpHost;

            // ��ǰ������õ�Api����
            var httpApi = host == HttpHost.DefaultHost
                ? Data.Uri.PathAndQuery
                : $"{Data.ApiName}{Data.Uri.Query}";

            // ��������
            if (host.Hosts.Length == 1)
                httpHost = host.Hosts[0];
            else
                lock (host)
                {
                    //ƽ������
                    httpHost = host.Hosts[host.Next];
                    if (++host.Next >= host.Hosts.Length)
                        host.Next = 0;
                }

            // Զ�̵���
            using (MonitorScope.CreateScope("CallHttp"))
            {
                var caller = new HttpApiCaller(httpHost);
                caller.CreateRequest(httpApi, Data.HttpMethod, Request, Data);

                LogRecorderX.MonitorTrace($"Url:{caller.RemoteUrl} | Token:{Data.Token}");

                Data.ResultMessage = await caller.Call();
                Data.UserState = caller.UserState;
                Data.ZeroState = caller.ZeroState;
                LogRecorderX.MonitorTrace(Data.ResultMessage);
            }
            return Data.ResultMessage;
        }
    }
}