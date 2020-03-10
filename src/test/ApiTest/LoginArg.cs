using Gboxt.Common.DataModel;

namespace ApiTest
{ 
    /// <summary>
    /// ��¼����
    /// </summary>
    public class LoginArg : IApiArgument
    {
        /// <summary>
        /// �ֻ���
        /// </summary>
        /// <value>11λ�ֻ���,����Ϊ��</value>
        /// <example>15618965007</example>
        [DataRule(CanNull = false, Max = 11, Min = 11, Regex = "1[3-9]\\d{9,9}")]
        public string MobilePhone { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        /// <value>6-16λ�����ַ�\��ĸ\�������,�����ַ�\��ĸ\���ֶ���Ҫһ����,����Ϊ��</value>
        /// <example>pwd#123</example>
        [DataRule(CanNull = false, Max = 6, Min = 16, Regex = "[\\da-zA-Z~!@#$%^&*]{6,16}")]
        public string UserPassword { get; set; }
        /// <summary>
        /// ��֤��
        /// </summary>
        /// <value>6λ��ĸ������,����Ϊ��</value>
        /// <example>123ABC</example>
        [DataRule(CanNull = false, Max = 6, Min = 6, Regex = "[a-zA-Z\\d]{6,6}")]
        public string VerificationCode { get; set; }
        
        /// <summary>
        /// ״̬
        /// </summary>
        public Entertype State { get; set; }

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

    /// <summary>
    /// ���뷽ʽ
    /// </summary>
    /// <remark>
    /// 1:ˢ��,2:��֤,3:������,4:��ά��,5:IC��,6:��ʻ֤,7:���� 8:�۰�,9:�ֶ���д
    /// </remark>
    public enum Entertype
    {
        /// <summary>
        /// ˢ��
        /// </summary>
        ˢ�� = 0x1,
        /// <summary>
        /// ��֤
        /// </summary>
        ��֤ = 0x2,
        /// <summary>
        /// ������
        /// </summary>
        ������ = 0x3,
        /// <summary>
        /// ��ά��
        /// </summary>
        ��ά�� = 0x4,
        /// <summary>
        /// IC��
        /// </summary>
        IC�� = 0x5,
        /// <summary>
        /// ��ʻ֤
        /// </summary>
        ��ʻ֤ = 0x6,
        /// <summary>
        /// ����
        /// </summary>
        ���� = 0x7,
        /// <summary>
        /// �۰�
        /// </summary>
        �۰� = 0x8,
        /// <summary>
        /// �ֶ���д
        /// </summary>
        �ֶ���д = 0x9,
    }
}
