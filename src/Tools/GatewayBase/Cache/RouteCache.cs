using System;
using System.Collections.Generic;
using System.Text;
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
                foreach (var kv in data.Arguments)
                {
                    kb.Append($"&{kv.Key}={kv.Value}");
                }
            }
            data.CacheKey = kb.ToString();
            if (!Cache.TryGetValue(data.CacheKey, out var cacheData))
                return false;
            if (cacheData.UpdateTime <= DateTime.Now)
            {
                Cache.Remove(data.CacheKey);
                return false;
            }
            data.ResultMessage = cacheData.Content;
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