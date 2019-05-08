using Microsoft.AspNetCore.Http;

namespace MicroZero.Http.Gateway
{
    public class HttpProtocol
    {
        /// <summary>
        ///     ����֧��
        /// </summary>
        internal static void Cros(HttpResponse response)
        {
            response.Headers.Add("Access-Control-Allow-Methods", new[] {"GET", "POST"});
            response.Headers.Add("Access-Control-Allow-Headers",
                new[] {"x-requested-with", "content-type", "authorization", "*"});
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        ///     ����֧��
        /// </summary>
        internal static void FormatResponse(HttpResponse response)
        {
            response.ContentType = "text/plain; charset=utf-8";
            response.Headers["Content-Type"] = "text/plain; charset=utf-8";
        }
    }
}