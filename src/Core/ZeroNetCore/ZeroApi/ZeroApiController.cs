using Agebull.Common.OAuth;
using Agebull.Common.Rpc;

namespace Agebull.ZeroNet.ZeroApi
{
    /// <summary>
    /// ZeroApi����������
    /// </summary>
    public class ApiController
    {
        /// <summary>
        /// ��ǰ��¼�û�
        /// </summary>
        public ILoginUserInfo UserInfo => GlobalContext.Customer;
        /// <summary>
        /// �����ߣ���������
        /// </summary>
        public string Caller => GlobalContext.RequestInfo.ServiceKey;
        /// <summary>
        /// ���ñ�ʶ
        /// </summary>
        public string RequestId => GlobalContext.RequestInfo.RequestId;
        /// <summary>
        /// HTTP����ʱ��UserAgent
        /// </summary>
        public string UserAgent => GlobalContext.RequestInfo.UserAgent;

    }
    /// <summary>
    /// ZeroApi����������
    /// </summary>
    public class ZeroApiController : ApiController
    {
    }
}