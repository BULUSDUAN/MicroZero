using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Agebull.ZeroNet.ZeroApi;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;

namespace Agebull.ZeroNet.Core
{
    /// <summary>
    /// 站点配置
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), DataContract, Serializable]
    public class StationConfig : SimpleConfig, IApiResultData
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, JsonProperty("station_name")]
        public string StationName { get => _name; set => _name = value; }
        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, JsonProperty("short_name")]
        public string ShortName { get; set; }
        /// <summary>
        /// 站点别名
        /// </summary>
        [DataMember, JsonProperty("station_alias")]
        public List<string> StationAlias { get; set; }
        /// <summary>
        /// 站点类型
        /// </summary>
        [DataMember, JsonProperty("station_type")]
        public int StationType { get; set; }
        /// <summary>
        /// 入站端口
        /// </summary>
        [DataMember, JsonProperty("request_port")]
        public int RequestPort { get; set; }

        /// <summary>
        /// 入站地址
        /// </summary>
        [DataMember, JsonProperty]
        public string RequestAddress => ZeroIdentityHelper.GetRequestAddress(StationName, RequestPort);

        /// <summary>
        /// 入站端口
        /// </summary>
        [DataMember, JsonProperty("worker_out_port")]
        public int WorkerCallPort { get; set; }

        /// <summary>
        /// 入站端口
        /// </summary>
        [DataMember, JsonProperty("worker_in_port")]
        public int WorkerResultPort { get; set; }

        /// <summary>
        /// 出站地址
        /// </summary>
        [DataMember, JsonProperty]
        public string WorkerResultAddress => ZeroIdentityHelper.GetWorkerAddress(StationName, WorkerResultPort);
        /// <summary>
        /// 出站地址
        /// </summary>
        [DataMember, JsonProperty]
        public string WorkerCallAddress => ZeroIdentityHelper.GetWorkerAddress(StationName, WorkerCallPort);

        /// <summary>
        /// 出站地址
        /// </summary>
        [DataMember, JsonProperty]
        public string SubAddress => ZeroIdentityHelper.GetSubscriberAddress(StationName, WorkerCallPort);

        /// <summary>
        /// 请求入
        /// </summary>
        [DataMember, JsonProperty("request_in")]
        public long RequestIn { get; set; }
        /// <summary>
        /// 请求出
        /// </summary>
        [DataMember, JsonProperty("request_out")]
        public long RequestOut { get; set; }
        /// <summary>
        /// 请求错误
        /// </summary>
        [DataMember, JsonProperty("request_err")]
        public long RequestErr { get; set; }

        /// <summary>
        /// 调用回
        /// </summary>
        [DataMember, JsonProperty("worker_in")]
        public long WorkerIn { get; set; }
        /// <summary>
        /// 调用出
        /// </summary>
        [DataMember, JsonProperty("worker_out")]
        public long WorkerOut { get; set; }
        /// <summary>
        /// 调用错
        /// </summary>
        [DataMember, JsonProperty("worker_err")]
        public long WorkerErr { get; set; }

        /// <summary>
        ///     运行状态
        /// </summary>
        [DataMember, JsonProperty("station_state")]
        public ZeroCenterState State { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, JsonProperty("state")]
        public string _ => State.ToString();

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, JsonProperty("workers")]
        public List<ZeroWorker> Workers { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, JsonProperty("worker_count")]
        public int WorkersCount => Workers?.Count ?? 0;

        /// <summary>
        /// 站点类型
        /// </summary>
        [DataMember, JsonProperty]
        public string TypeName
        {
            get
            {
                switch (StationType)
                {
                    default:
                        return "Error";
                    case ZeroStation.StationTypeApi:
                        return "API";
                    case ZeroStation.StationTypeDispatcher:
                        return "Dispatcher";
                    case ZeroStation.StationTypePublish:
                        return "Publish";
                    case ZeroStation.StationTypeVote:
                        return "Vote";
                }
            }
        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="src"></param>
        public void Copy(StationConfig src)
        {
            StationName = src.StationName;
            StationAlias = src.StationAlias;
            StationType = src.StationType;
            RequestPort = src.RequestPort;
            WorkerCallPort = src.WorkerCallPort;
            WorkerResultPort = src.WorkerResultPort;
            State = src.State;
            RequestIn = src.RequestIn;
            RequestOut = src.RequestOut;
            RequestErr = src.RequestErr;
            WorkerIn = src.WorkerIn;
            WorkerOut = src.WorkerOut;
            WorkerErr = src.WorkerErr;
            Workers = src.Workers;
        }

        #region 性能统计


        /// <summary>
        /// 设置计数值
        /// </summary>
        public void CheckValue(StationConfig src)
        {
            switch (Count)
            {
                //小于0表示ZeroCenter重启
                case 0:
                    TotalQps = 0;
                    TotalTps = 0;
                    AvgQps = 0;
                    AvgTps = 0;
                    MaxQps = 0;
                    MinQps = 0;
                    MaxTps = 0;
                    MinTps = 0;
                    break;
                case 1:
                    TotalQps = src.RequestOut - RequestOut;
                    AvgQps = TotalQps;
                    MaxQps = TotalQps;
                    MinQps = TotalQps;
                    LastQps = TotalQps;
                    TotalTps = src.WorkerOut - WorkerOut;
                    AvgTps = TotalTps;
                    MaxTps = TotalTps;
                    MinTps = TotalTps;
                    LastTps = TotalTps;
                    break;
                default:
                    var last = src.RequestOut - RequestOut;
                    if (last >= 0)
                    {
                        LastQps = last;
                        TotalQps += LastQps;
                        AvgQps = TotalQps / Count;
                        if (last > 0)
                        {
                            if (MaxQps < LastQps)
                                MaxQps = LastQps;
                            if (MinQps > LastQps)
                                MinQps = LastQps;
                        }

                        LastTps = src.WorkerOut - WorkerOut;
                        TotalTps += LastTps;
                        AvgTps = TotalTps / Count;
                        if (LastTps > 0)
                        {
                            if (MaxTps < LastTps)
                                MaxTps = LastTps;
                            if (MinTps > LastTps)
                                MinTps = LastTps;
                        }
                    }
                    break;
            }
            Count += 1;
            State = src.State;
            RequestIn = src.RequestIn;
            RequestOut = src.RequestOut;
            RequestErr = src.RequestErr;
            WorkerIn = src.WorkerIn;
            WorkerOut = src.WorkerOut;
            WorkerErr = src.WorkerErr;
            Workers = src.Workers;
        }

        /// <summary>
        /// 心跳数（即秒数）
        /// </summary>
        [DataMember, JsonProperty]
        public int Count { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        [DataMember, JsonProperty]
        public long TotalQps { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        [DataMember, JsonProperty]

        public long TotalTps { get; set; }

        /// <summary>
        /// 平均
        /// </summary>
        [DataMember, JsonProperty]
        public long AvgQps { get; set; }

        /// <summary>
        /// 平均
        /// </summary>
        [DataMember, JsonProperty]
        public long AvgTps { get; set; }

        /// <summary>
        /// 最后
        /// </summary>
        [DataMember, JsonProperty]
        public long LastTps { get; set; }

        /// <summary>
        /// 最后
        /// </summary>
        [DataMember, JsonProperty]
        public long LastQps { get; set; }

        /// <summary>
        /// 最大
        /// </summary>
        [DataMember, JsonProperty]
        public long MaxTps { get; set; }

        /// <summary>
        /// 最大
        /// </summary>
        [DataMember, JsonProperty]
        public long MaxQps { get; set; }

        /// <summary>
        /// 最小
        /// </summary>
        [DataMember, JsonProperty]
        public long MinTps { get; set; }

        /// <summary>
        /// 最小
        /// </summary>
        [DataMember, JsonProperty]
        public long MinQps { get; set; }

        #endregion
    }


    /// <summary>
    /// 工作对象
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), DataContract, Serializable]
    public class ZeroWorker
    {
        /// <summary>
        ///  实名
        /// </summary>
        [DataMember, JsonProperty("real_name")]
        public string RealName { get; set; }

        /// <summary>
        ///  上报的IP地址
        /// </summary>
        [DataMember, JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        /// <summary>
        ///  上次心跳的时间
        /// </summary>
        [DataMember, JsonProperty("pre_time")] public string pre_time;

        /// <summary>
        ///  健康等级
        /// </summary>
        [DataMember, JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        ///  状态 -1 已失联 0 正在准备中 1 已就绪 3 已退出
        /// </summary>
        [DataMember, JsonProperty("state")]
        public int State { get; set; }

        /// <summary>
        ///  状态
        /// </summary>
        [DataMember, JsonProperty("state_text")]
        public string StateText
        {
            get
            {
                switch (State)
                {
                    case 0:
                        return "Prepare";
                    case 1:
                        return "Ready";
                    case 2:
                        return "Left";
                    default:
                        return "Lost";
                }
            }
        }
    }
}
