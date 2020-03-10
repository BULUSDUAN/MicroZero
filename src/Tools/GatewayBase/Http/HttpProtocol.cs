using System.Linq;
using Agebull.Common.Configuration;
using Microsoft.AspNetCore.Http;

namespace MicroZero.Http.Gateway
{
    /// <summary>
    /// HTTPЭ����ص�֧��
    /// </summary>
    public class HttpProtocol
    {
        /// <summary>
        ///     ����֧��
        /// </summary>
        internal static void CrosOption(HttpResponse response)
        {
            response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET", "POST" });
            response.Headers.Add("Access-Control-Allow-Headers", new[] { "x-requested-with", "content-type", "authorization", "*" });
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        ///     ����֧��
        /// </summary>
        internal static void CrosCall(HttpResponse response)
        {
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        ///     ��������
        /// </summary>
        internal static void FormatResponse(HttpRequest request, HttpResponse response)
        {
            if (GatewayOption.Option.SystemConfig.IsTest && request.Headers["USER-AGENT"].LinkToString("|")?.IndexOf("PostmanRuntime") == 0)
                response.Headers["Content-Type"] = response.ContentType = "application/json; charset=UTF-8";
            else
                response.Headers["Content-Type"] = response.ContentType = GatewayOption.Option.SystemConfig.ContentType;
        }
    }
}