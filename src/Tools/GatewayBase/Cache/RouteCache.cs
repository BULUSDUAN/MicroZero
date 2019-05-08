using System;
using System.Collections.Generic;
using Agebull.MicroZero.ZeroApis;

namespace MicroZero.Http.Gateway
{
    /// <summary>
    ///     ����
    /// </summary>
    internal class RouteCache
    {
        /// <summary>
        ///     ��������
        /// </summary>
        internal static Dictionary<string, CacheData> Cache =
            new Dictionary<string, CacheData>(StringComparer.OrdinalIgnoreCase);

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
        internal static bool LoadCache(Uri uri, string bearer, out CacheOption setting, out string key,
            ref string resultMessage)
        {
            if (!CacheMap.TryGetValue(uri.LocalPath, out setting))
            {
                key = null;
                return false;
            }

            if (setting.Feature.HasFlag(CacheFeature.Bear) && bearer.Substring(0, setting.Bear.Length) != setting.Bear)
            {
                setting = null;
                key = null;
                return false;
            }

            CacheData cacheData;

            lock (setting)
            {
                key = setting.OnlyName ? uri.LocalPath : uri.PathAndQuery;
                if (!Cache.TryGetValue(key, out cacheData))
                    return false;
                if (cacheData.UpdateTime <= DateTime.Now)
                {
                    Cache.Remove(key);
                    return false;
                }
            }

            resultMessage = cacheData.Content;
            return true;
        }

        /// <summary>
        ///     ���淵��ֵ
        /// </summary>
        /// <param name="data"></param>
        internal static void CacheResult(RouteData data)
        {
            if (data.CacheSetting == null || !data.IsSucceed)
                return;
            CacheData cacheData;
            if (data.CacheSetting.Feature.HasFlag(CacheFeature.NetError) &&
                data.UserState == UserOperatorStateType.RemoteError)
                cacheData = new CacheData
                {
                    Content = data.ResultMessage,
                    UpdateTime = DateTime.Now.AddSeconds(30)
                };
            else
                cacheData = new CacheData
                {
                    Content = data.ResultMessage,
                    UpdateTime = DateTime.Now.AddSeconds(data.CacheSetting.FlushSecond)
                };

            lock (data.CacheSetting)
            {
                if (!Cache.ContainsKey(data.CacheKey))
                    Cache.Add(data.CacheKey, cacheData);
                else
                    Cache[data.CacheKey] = cacheData;
            }
        }

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
            if (RouteOption.Option._cacheSettings == null)
                return;
            foreach (var setting in RouteOption.Option._cacheSettings)
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