using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MicroZero.Http.Gateway
{

    /// <summary>
    ///     ��������
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]
    public class CacheOption
    {
        /// <summary>
        ///     API����
        /// </summary>
        [DataMember]
        [JsonProperty]
        public string Api { get; set; }

        /// <summary>
        ///     ����У������ͷ
        /// </summary>
        [DataMember]
        [JsonProperty]
        public string Bear { get; set; }

        /// <summary>
        ///     ������µ�����
        /// </summary>
        [DataMember]
        [JsonProperty]
        public int FlushSecond { get; set; }

        /// <summary>
        ///     ����ʱ��ʹ�����ƣ����������ѯ�ַ�����
        /// </summary>
        [DataMember]
        [JsonProperty]
        public bool OnlyName { get; set; }

        /// <summary>
        ///     �����������ʱ����
        /// </summary>
        [DataMember]
        [JsonProperty]
        public bool ByNetError { get; set; }

        /// <summary>
        ///     ��������
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore] public CacheFeature Feature { get; private set; }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        public void Initialize()
        {
            //Ĭ��5����
            if (FlushSecond <= 0)
                FlushSecond = 300;
            else if (FlushSecond > 3600)
                FlushSecond = 3600;
            if (!string.IsNullOrWhiteSpace(Bear))
                Feature |= CacheFeature.Bear;
            if (!OnlyName)
                Feature |= CacheFeature.QueryString;
            if (ByNetError)
                Feature |= CacheFeature.NetError;
        }
    }
}