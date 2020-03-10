
using Agebull.Common.Context;
using Agebull.EntityModel.Common;
using Agebull.MicroZero.ZeroApis;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>API���ػ���</summary>
    public class ApiResultDefault : IApiResultDefault
    {

        /// <summary>
        /// �����л�
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IApiResult DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject< ApiResult>(json);
        }

        /// <summary>
        /// �����л�
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IApiResult<T> DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <returns></returns>
        public IApiResult Error(int errCode)
        {
            return new ApiResult
            {
                Success = false,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <param name="message">������Ϣ</param>
        /// <returns></returns>
        public IApiResult Error(int errCode, string message)
        {
            return new ApiResult
            {
                Success = errCode == 0,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <param name="message">������Ϣ</param>
        /// <param name="innerMessage">�ڲ�˵��</param>
        /// <returns></returns>
        public IApiResult Error(int errCode, string message, string innerMessage)
        {
            return new ApiResult
            {
                Success = errCode == 0,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage
                }
            };
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <param name="message">������Ϣ</param>
        /// <param name="innerMessage">�ڲ�˵��</param>
        /// <param name="guide">����ָ��</param>
        /// <param name="describe">�������</param>
        /// <returns></returns>
        public IApiResult Error(int errCode, string message, string innerMessage, string guide, string describe)
        {
            return new ApiResult
            {
                Success = errCode == 0,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <param name="message">������Ϣ</param>
        /// <param name="innerMessage">�ڲ�˵��</param>
        /// <param name="point">�����</param>
        /// <param name="guide">����ָ��</param>
        /// <param name="describe">�������</param>
        /// <returns></returns>
        public IApiResult Error(int errCode, string message, string innerMessage, string point, string guide, string describe)
        {
            return new ApiResult
            {
                Success = errCode == 0,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    Point = point,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }

        /// <summary>����һ���ɹ��ı�׼����</summary>
        /// <returns></returns>
        public IApiResult<TData> Succees<TData>(TData data)
        {
            return new ApiResult<TData>
            {
                Success = true,
                ResultData = data
            };
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <returns></returns>
        public IApiResult<TData> Error<TData>(int errCode)
        {
            return new ApiResult<TData>
            {
                Success = false,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public IApiResult<TData> Error<TData>(int errCode, string message)
        {
            return new ApiResult<TData>
            {
                Success = false,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <param name="message">������Ϣ</param>
        /// <param name="innerMessage">�ڲ�˵��</param>
        /// <returns></returns>
        public IApiResult<TData> Error<TData>(int errCode, string message, string innerMessage)
        {
            return new ApiResult<TData>
            {
                Success = errCode == 0,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage
                }
            };
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <param name="message">������Ϣ</param>
        /// <param name="innerMessage">�ڲ�˵��</param>
        /// <param name="guide">����ָ��</param>
        /// <param name="describe">�������</param>
        /// <returns></returns>
        public IApiResult<TData> Error<TData>(int errCode, string message, string innerMessage, string guide, string describe)
        {
            return new ApiResult<TData>
            {
                Success = errCode == 0,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }

        /// <summary>����һ������������ı�׼����</summary>
        /// <param name="errCode">������</param>
        /// <param name="message">������Ϣ</param>
        /// <param name="innerMessage">�ڲ�˵��</param>
        /// <param name="point">�����</param>
        /// <param name="guide">����ָ��</param>
        /// <param name="describe">�������</param>
        /// <returns></returns>
        public IApiResult<TData> Error<TData>(int errCode, string message, string innerMessage, string point, string guide, string describe)
        {
            return new ApiResult<TData>
            {
                Success = errCode == 0,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    Point = point,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }

        /// <summary>����һ���ɹ��ı�׼����</summary>
        /// <returns></returns>
        public IApiResult Error()
        {
            return new ApiResult
            {
                Success = false,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>����һ���ɹ��ı�׼����</summary>
        /// <returns></returns>
        public IApiResult<TData> Error<TData>()
        {
            return new ApiResult<TData>
            {
                Success = false,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>����һ���ɹ��ı�׼����</summary>
        /// <returns></returns>
        public IApiResult Succees()
        {
            return new ApiResult
            {
                Success = true,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>����һ���ɹ��ı�׼����</summary>
        /// <returns></returns>
        public IApiResult<TData> Succees<TData>()
        {
            return new ApiResult<TData>
            {
                Success = true,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>�ɹ�</summary>
        /// <remarks>�ɹ�</remarks>
        public IApiResult Ok => Succees();

        /// <summary>ҳ�治����</summary>
        public IApiResult NoFind => Error(404, "*ҳ�治����*");

        /// <summary>��֧�ֵĲ���</summary>
        public IApiResult NotSupport => Error(404, "*ҳ�治����*");

        /// <summary>���������ַ���</summary>
        public IApiResult ArgumentError => Error(-2, "��������");

        /// <summary>�߼������ַ���</summary>
        public IApiResult LogicalError => Error(-2, "�߼�����");

        /// <summary>�ܾ�����</summary>
        public IApiResult DenyAccess => Error(-13);

        /// <summary>�������޷���ֵ���ַ���</summary>
        public IApiResult RemoteEmptyError => Error(-3, "*�������޷���ֵ*");

        /// <summary>�����������쳣</summary>
        public IApiResult NetworkError => Error(-5);

        /// <summary>���ش���</summary>
        public IApiResult LocalError => Error(-4);

        /// <summary>���ط����쳣</summary>
        public IApiResult LocalException => Error(-1);

        /// <summary>ϵͳδ����</summary>
        public IApiResult NoReady => Error(-10);

        /// <summary>��ͣ����</summary>
        public IApiResult Pause => Error(-10, "��ͣ����");

        /// <summary>δ֪����</summary>
        public IApiResult UnknowError => Error(-4, "δ֪����");

        /// <summary>���糬ʱ</summary>
        /// <remarks>��������Apiʱʱ�׳�δ�����쳣</remarks>
        public IApiResult TimeOut => Error(-5, "���糬ʱ");

        /// <summary>�ڲ�����</summary>
        /// <remarks>ִ�з���ʱ�׳�δ�����쳣</remarks>
        public IApiResult InnerError => Error(-4, "�ڲ�����");

        /// <summary>���񲻿���</summary>
        public IApiResult Unavailable => Error(503, "���񲻿���");
    }
}