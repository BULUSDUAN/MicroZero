using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Agebull.ZeroNet.ZeroApi
{
    /// <summary>
    /// �������
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Argument : IApiArgument
    {
        /// <summary>
        /// AT
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>
        /// ����У��
        /// </summary>
        /// <param name="message">���ص���Ϣ</param>
        /// <returns>�ɹ��򷵻���</returns>
        public bool Validate(out string message)
        {
            message = null;
            return true;
        }
    }
    /// <summary>
    /// �������
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Argument<T> : IApiArgument
    {
        /// <summary>
        /// AT
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Value { get; set; }

        /// <summary>
        /// ����У��
        /// </summary>
        /// <param name="message">���ص���Ϣ</param>
        /// <returns>�ɹ��򷵻���</returns>
        public bool Validate(out string message)
        {
            message = null;
            return true;
        }
    }
}