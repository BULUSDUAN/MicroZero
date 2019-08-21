﻿using Agebull.MicroZero;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.Serialization;
using MicroZero.Http.Route;

namespace WebMonitor.Models
{
    /// <summary>
    ///     站点配置
    /// </summary>
    [DataContract]
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class StationInfo
    {
        /// <summary>
        ///     站点名称
        /// </summary>
        [DataMember, JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        [DataMember, JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///     站点名称
        /// </summary>
        [DataMember, JsonProperty("short_name")]
        public string ShortName { get; set; }

        /// <summary>
        ///     站点别名
        /// </summary>
        [DataMember, JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        ///     入站地址
        /// </summary>
        [DataMember, JsonProperty("clientCallAddress")]
        public string RequestAddress { get; set; }

        /// <summary>
        ///     出站地址
        /// </summary>
        [DataMember, JsonProperty("workerResultAddress")]
        public string WorkerResultAddress { get; set; }

        /// <summary>
        ///     出站地址
        /// </summary>
        [DataMember, JsonProperty("workerCallAddress")]
        public string WorkerCallAddress { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        [DataMember, JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        ///     站点类型
        /// </summary>
        [DataMember, JsonProperty("type")]
        public string Type { get; set; }


        /// <summary>
        ///     是否基础站点
        /// </summary>
        [DataMember]
        [JsonProperty("is_sys")]
        public bool IsSystem { get; set; }


        /// <summary>
        ///     是否基础站点
        /// </summary>
        [DataMember]
        [JsonProperty("is_base")]
        public bool IsBaseStation { get; set; }

        /// <summary>
        ///     是否基础站点
        /// </summary>
        [DataMember]
        [JsonProperty("is_general")]
        public bool IsGeneralStation { get; set; }

        /// <summary>
        ///     是否基础站点
        /// </summary>
        [DataMember]
        [JsonProperty("status")]
        public StationCountItem Status { get; set; } = new StationCountItem();

        /// <summary>
        ///     构造
        /// </summary>
        public StationInfo()
        {
        }

        /// <summary>
        ///     复制
        /// </summary>
        /// <param name="src"></param>
        public StationInfo(StationConfig src)
        {
            Name = src.StationName;
            Description = src.Description;
            ShortName = src.ShortName;
            Alias = src.StationAlias.LinkToString(',');

            //switch (src.StationType)
            //{
            //    case ZeroStationType.Vote:
            //    case ZeroStationType.Api:
            //    case ZeroStationType.Queue:
            //    case ZeroStationType.RouteApi:
            //        Type = "API"; break;
            //    case ZeroStationType.Trace:
            //    case ZeroStationType.Proxy:
            //    case ZeroStationType.Plan:
            //    case ZeroStationType.Dispatcher:
            //    case ZeroStationType.Notify:
            //        Type = "Pub"; break;
            //}
            Type = src.StationType.ToString();
            RequestAddress = src.RequestAddress;
            WorkerCallAddress = src.WorkerCallAddress;
            IsGeneralStation = src.IsGeneral;
            IsSystem = src.IsSystem;
            IsBaseStation = src.IsBaseStation;
            WorkerResultAddress = src.WorkerResultAddress;
            State = src.State.ToString();
        }

    }
}