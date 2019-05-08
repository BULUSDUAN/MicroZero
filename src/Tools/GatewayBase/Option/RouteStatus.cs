namespace MicroZero.Http.Gateway
{
    /// <summary>
    ///     һ��·��ִ��״̬
    /// </summary>
    public enum RouteStatus
    {
        /// <summary>
        ///     ����
        /// </summary>
        None,

        /// <summary>
        ///     ����
        /// </summary>
        Cache,

        /// <summary>
        ///     Http��OPTIONЭ��
        /// </summary>
        HttpOptions,
        /// <summary>
        /// �Ƿ���ʽ
        /// </summary>
        FormalError,
        /// <summary>
        /// �߼�����
        /// </summary>
        LogicalError,
        /// <summary>
        /// �����쳣
        /// </summary>
        LocalException,
        /// <summary>
        /// ���ش���
        /// </summary>
        LocalError,
        /// <summary>
        /// Զ�̴���
        /// </summary>
        RemoteError,
        /// <summary>
        /// �ܾ�����
        /// </summary>
        Unavailable,
        /// <summary>
        /// ҳ�治����
        /// </summary>
        NotFind,
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        DenyAccess
    }
}