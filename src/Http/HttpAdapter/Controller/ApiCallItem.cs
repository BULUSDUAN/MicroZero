﻿using System.Collections.Generic;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    /// Api调用节点
    /// </summary>
    public class ApiCallItem
    {
        /// <summary>
        /// 全局ID
        /// </summary>
        public string GlobalId { get; set; }

        /// <summary>
        /// 调用方的全局ID
        /// </summary>
        public string CallerGlobalId { get; set; }

        /// <summary>
        /// 请求者
        /// </summary>
        public string Caller { get; set; }

        /// <summary>
        /// 请求标识
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 请求者
        /// </summary>
        public string Requester { get; set; }
        /// <summary>
        /// API名称
        /// </summary>
        public string ApiName { get; set; }
        /// <summary>
        ///  原始上下文的JSO内容
        /// </summary>
        public string ContextJson { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string Argument { get; set; }
        /// <summary>
        /// 请求参数字典
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 返回
        /// </summary>
        public string Result { get; set; }

    }
}