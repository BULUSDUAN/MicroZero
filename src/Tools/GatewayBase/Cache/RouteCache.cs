using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Agebull.MicroZero.ZeroApis;

namespace MicroZero.Http.Gateway
{
    /// <summary>
    ///     ����
    /// </summary>
    internal class RouteCache
    {
        #region ����

        /// <summary>
        ///     ��������
        /// </summary>
        internal static Dictionary<string, CacheData> Cache = new Dictionary<string, CacheData>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     ˢ��
        /// </summary>
        internal static void Flush()
        {
            lock (Cache)
            {
                Cache.Clear();
            }
        }

        /// <summary>
        ///     ��黺��
        /// </summary>
        /// <returns>ȡ�����棬����ֱ�ӷ���</returns>
        internal static bool LoadCache(RouteData data)
        {
            if (!CacheMap.TryGetValue(data.Uri.LocalPath, out data.CacheSetting))
            {
                data.CacheKey = null;
                return false;
            }
            var kb = new StringBuilder();
            kb.Append(data.Uri.LocalPath);
            if (data.CacheSetting.Feature.HasFlag(CacheFeature.Bear))
            {
                kb.Append(data.Token);
            }
            if (data.CacheSetting.Feature.HasFlag(CacheFeature.QueryString))
            {
                foreach (var kv in data.Arguments.OrderBy(p => p.Key))
                {
                    kb.Append($"&{kv.Key}={kv.Value}");
                }
                if (!string.IsNullOrWhiteSpace(data.HttpContext))
                {
                    kb.Append(data.HttpContext);
                }
            }
            CacheData cacheData;
            data.CacheKey = kb.ToString();
            lock (Cache)
            {
                if (!Cache.TryGetValue(data.CacheKey, out cacheData))
                {
                    Cache.Add(data.CacheKey, new CacheData
                    {
                        IsLoading = 1,
                        Content = ApiResultIoc.NoReadyJson
                    });
                    return false;
                }
            }
            if (cacheData.Success)
            {
                if (cacheData.UpdateTime > DateTime.Now)
                {
                    data.ResultMessage = cacheData.Content;
                    return true;
                }
            }
            //һ�����룬�����ĵȴ����óɹ�
            if (Interlocked.Increment(ref cacheData.IsLoading) == 0)
            {
                return false;
            }

            //�ȴ����óɹ�
            int cnt = 0;
            while (++cnt < 10)
            {
                Thread.Sleep(50);
                lock (Cache)
                {
                    if (!Cache.TryGetValue(data.CacheKey, out var newData) || cacheData == newData)
                        continue;
                }
                data.ResultMessage = cacheData.Content;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     ���淵��ֵ
        /// </summary>
        /// <param name="data"></param>
        internal static void CacheResult(RouteData data)
        {
            if (data.CacheSetting == null || !data.IsSucceed || data.CacheKey == null)
                return;
            if (!data.IsSucceed && !data.CacheSetting.Feature.HasFlag(CacheFeature.NetError))
            {
                return;
            }
            CacheData cacheData = new CacheData
            {
                Content = data.ResultMessage,
                Success = data.IsSucceed,
                UpdateTime = DateTime.Now.AddSeconds(data.CacheSetting.FlushSecond)
            };

            lock (Cache)
            {
                Cache[data.CacheKey] = cacheData;
            }
        }

        #endregion

        #region ����

        /// <summary>
        ///     ·������
        /// </summary>
        public static Dictionary<string, CacheOption> CacheMap { get; set; }


        /// <summary>
        ///     ��ʼ��·��
        /// </summary>
        /// <returns></returns>
        public static void InitCache()
        {
            CacheMap = new Dictionary<string, CacheOption>(StringComparer.OrdinalIgnoreCase);
            if (RouteOption.Option.CacheSettings == null)
                return;
            foreach (var setting in RouteOption.Option.CacheSettings)
            {
                setting.Initialize();
                if (!CacheMap.ContainsKey(setting.Api))
                    CacheMap.Add(setting.Api, setting);
                else
                    CacheMap[setting.Api] = setting;
            }
        }

        #endregion
    }
}