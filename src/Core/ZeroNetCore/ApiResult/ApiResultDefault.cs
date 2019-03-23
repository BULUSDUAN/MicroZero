
using Agebull.Common.Context;
using Agebull.EntityModel.Common;
using Agebull.MicroZero.ZeroApis;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>API返回基类</summary>
    public class ApiResultDefault : IApiResultDefault
    {

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IApiResult DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject< ApiResult>(json);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IApiResult<T> DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
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

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
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

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
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

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
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

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="point">错误点</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
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

        /// <summary>生成一个成功的标准返回</summary>
        /// <returns></returns>
        public IApiResult<TData> Succees<TData>(TData data)
        {
            return new ApiResult<TData>
            {
                Success = true,
                ResultData = data
            };
        }

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
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

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
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

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
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

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
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

        /// <summary>生成一个包含错误码的标准返回</summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="point">错误点</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
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

        /// <summary>生成一个成功的标准返回</summary>
        /// <returns></returns>
        public IApiResult Error()
        {
            return new ApiResult
            {
                Success = false,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>生成一个成功的标准返回</summary>
        /// <returns></returns>
        public IApiResult<TData> Error<TData>()
        {
            return new ApiResult<TData>
            {
                Success = false,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>生成一个成功的标准返回</summary>
        /// <returns></returns>
        public IApiResult Succees()
        {
            return new ApiResult
            {
                Success = true,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>生成一个成功的标准返回</summary>
        /// <returns></returns>
        public IApiResult<TData> Succees<TData>()
        {
            return new ApiResult<TData>
            {
                Success = true,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>成功</summary>
        /// <remarks>成功</remarks>
        public IApiResult Ok => Succees();

        /// <summary>页面不存在</summary>
        public IApiResult NoFind => Error(404, "*页面不存在*");

        /// <summary>不支持的操作</summary>
        public IApiResult NotSupport => Error(404, "*页面不存在*");

        /// <summary>参数错误字符串</summary>
        public IApiResult ArgumentError => Error(-2, "参数错误");

        /// <summary>逻辑错误字符串</summary>
        public IApiResult LogicalError => Error(-2, "逻辑错误");

        /// <summary>拒绝访问</summary>
        public IApiResult DenyAccess => Error(-13);

        /// <summary>服务器无返回值的字符串</summary>
        public IApiResult RemoteEmptyError => Error(-3, "*服务器无返回值*");

        /// <summary>服务器访问异常</summary>
        public IApiResult NetworkError => Error(-5);

        /// <summary>本地错误</summary>
        public IApiResult LocalError => Error(-4);

        /// <summary>本地访问异常</summary>
        public IApiResult LocalException => Error(-1);

        /// <summary>系统未就绪</summary>
        public IApiResult NoReady => Error(-10);

        /// <summary>暂停服务</summary>
        public IApiResult Pause => Error(-10, "暂停服务");

        /// <summary>未知错误</summary>
        public IApiResult UnknowError => Error(-4, "未知错误");

        /// <summary>网络超时</summary>
        /// <remarks>调用其它Api时时抛出未处理异常</remarks>
        public IApiResult TimeOut => Error(-5, "网络超时");

        /// <summary>内部错误</summary>
        /// <remarks>执行方法时抛出未处理异常</remarks>
        public IApiResult InnerError => Error(-4, "内部错误");

        /// <summary>服务不可用</summary>
        public IApiResult Unavailable => Error(503, "服务不可用");
    }
}