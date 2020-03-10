using Agebull.ZeroNet.ZeroApi;
using Gboxt.Common.DataModel;
using System.Text.RegularExpressions;

namespace ApiTest
{
    /// <summary>
    /// ��¼����
    /// </summary>
    public class MachineEventArg : IApiArgument
    {
        /// <summary>
        /// �¼�����
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// ������ʶ
        /// </summary>
        public string MachineId { get; set; }

        /// <summary>
        /// �¼�����
        /// </summary>
        public string JsonStr { get; set; }
        
        /// <summary>
        /// У��
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool IApiArgument.Validate(out string message)
        {
            message = null;
            return true;
        }
    }
}
