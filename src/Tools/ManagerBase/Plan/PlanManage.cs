using System;
using System.Collections.Generic;
using System.Linq;

using Agebull.MicroZero;
using Agebull.EntityModel.Common;
using Newtonsoft.Json;
using WebMonitor;
using Agebull.MicroZero.ZeroApis;

namespace MicroZero.Http.Route
{

    /// <summary>
    /// 计划管理器
    /// </summary>
    public class PlanManage : ZSimpleCommand
    {
        #region 实例

        /// <summary>
        /// 构造路由计数器
        /// </summary>
        public PlanManage()
        {
            ManageAddress = ZeroApplication.Config["PlanDispatcher"]?.RequestAddress;
            ServiceKey = ZeroApplication.Config.Master.ServiceKey.ToZeroBytes();
            FlushList();
        }

        #endregion

        public static Dictionary<long, ZeroPlan> Plans = new Dictionary<long, ZeroPlan>();

        public static void OnPlanEvent(ZeroNetEventType eventType, ZeroPlan plan)
        {
            if (plan == null)
                return;
            switch (eventType)
            {
                case ZeroNetEventType.PlanAdd:
                    SyncPlan(plan);
                    break;
                case ZeroNetEventType.PlanRemove:
                    RemovePlan(plan);
                    break;
                default:
                    UpdatePlan(plan);
                    break;
            }
        }
        public static void RemovePlan(ZeroPlan plan)
        {

            if (!Plans.TryGetValue(plan.plan_id, out _)) return;
            Plans.Remove(plan.plan_id);
            plan.plan_state = plan_message_state.remove;
            WebSocketNotify.Publish("plan_notify", "remove", JsonHelper.SerializeObject(plan));
        }
        public static void UpdatePlan(ZeroPlan plan)
        {
            if (!Plans.TryGetValue(plan.plan_id, out var old)) return;
            old.exec_time = plan.exec_time;
            old.exec_state = plan.exec_state;
            old.plan_state = plan.plan_state;
            old.plan_time = plan.plan_time;
            old.real_repet = plan.real_repet;
            old.skip_set = plan.skip_set;
            old.skip_num = plan.skip_num;
            WebSocketNotify.Publish("plan_notify", "update", JsonHelper.SerializeObject(plan));
        }

        public static void SyncPlan(ZeroPlan plan)
        {
            if (Plans.ContainsKey(plan.plan_id))
            {
                Plans[plan.plan_id] = plan;
            }
            else
            {
                Plans.Add(plan.plan_id, plan);
            }

            WebSocketNotify.Publish("plan_notify", "add", JsonHelper.SerializeObject(plan));
        }


        public ApiResult Pause(string id)
        {
            if (!long.TryParse(id, out var pid) || !Plans.TryGetValue(pid, out var plan))
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            var result = CallCommand("pause", $"msg:{plan.station}:{plan.plan_id:x}");
            if (result.State != ZeroOperatorStateType.Ok)
            {
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            }
            return ApiResult.Succees();
        }

        public ApiResult Reset(string id)
        {
            if (!long.TryParse(id, out var pid) || !Plans.TryGetValue(pid, out var plan))
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            var result = CallCommand("reset", $"msg:{plan.station}:{plan.plan_id:x}");
            if (result.State != ZeroOperatorStateType.Ok)
            {
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            }
            return ApiResult.Succees();
        }


        public ApiResult Clear()
        {
            int cnt = 0;
            foreach (var plan in Plans.Where(p => p.Value.plan_state == plan_message_state.close).ToArray())
            {
                var result = CallCommand("remove", $"msg:{plan.Value.station}:{plan.Value.plan_id:x}");
                if (result.State != ZeroOperatorStateType.Ok)
                    continue;
                Plans.Remove(plan.Key);
                cnt++;
            }
            return ApiResult.Succees(cnt);
        }


        public ApiResult Test()
        {
            return ApiResult.Succees(Plans.Values.ToList());
        }

        public ApiResult Remove(string id)
        {
            if (!long.TryParse(id, out var pid) || !Plans.TryGetValue(pid, out var plan))
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            var result = CallCommand("remove", $"msg:{plan.station}:{plan.plan_id:x}");
            if (result.State != ZeroOperatorStateType.Ok)
            {
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            }
            return ApiResult.Succees();
        }


        public ApiResult Close(string id)
        {
            if (!long.TryParse(id, out var pid) || !Plans.TryGetValue(pid, out var plan))
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            var result = CallCommand("close", $"msg:{plan.station}:{plan.plan_id:x}");
            if (result.State != ZeroOperatorStateType.Ok)
            {
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            }
            return ApiResult.Succees();
        }
        public IApiResult History()
        {
            return new ApiArrayResult<ZeroPlan>
            {
                ResultData = Plans.Values.Where(p => p.plan_state >= plan_message_state.close).ToList()
            };
        }
        public IApiResult Station(string station)
        {
            if (string.IsNullOrEmpty(station))
            {
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            }
            station = station.Split('-').LastOrDefault();
            return new ApiArrayResult<ZeroPlan>
            {
                ResultData = Plans.Values.Where(p => p.station.Equals(station, StringComparison.OrdinalIgnoreCase)).ToList()
            };
        }

        public IApiResult Active()
        {
            return new ApiArrayResult<ZeroPlan>
            {
                ResultData = Plans.Values.Where(p => p.plan_state < plan_message_state.close).ToList()
            };
        }
        public IApiResult Filter(string type)
        {
            if (type == "delay")
            {
                return new ApiArrayResult<ZeroPlan>
                {
                    ResultData = Plans.Values.Where(p => p.plan_type >= plan_date_type.second && p.plan_type <= plan_date_type.day).ToList()
                };
            }
            if (!Enum.TryParse<plan_date_type>(type, out var planType))
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            return new ApiArrayResult<ZeroPlan>
            {
                ResultData = Plans.Values.Where(p => p.plan_type == planType).ToList()
            };
        }
        public ApiResult FlushList()
        {
            var result = CallCommand("list");
            if (result.State != ZeroOperatorStateType.Ok)
            {
                return ApiResult.Error(ErrorCode.LogicalError, "参数错误");
            }
            if (!result.TryGetString(ZeroFrameType.Status, out var json))
            {
                ZeroTrace.WriteError("FlushList", "Empty");
                return ApiResult.Error(ErrorCode.LogicalError, "空数据");
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<ZeroPlan>>(json);
                Plans.Clear();
                foreach (var plan in list)
                {
                    if (plan.command != null)
                    {
                        SyncPlan(plan);
                        continue;
                    }

                    var res = CallCommand("remove", plan.name);
                    if (res.State != ZeroOperatorStateType.Ok)
                    {
                        ApiResult.Error(ErrorCode.LogicalError, "参数错误");
                    }
                }
                return ApiResult.Succees();
            }
            catch (Exception ex)
            {
                ZeroTrace.WriteException("FlushList", ex, json);
                return ApiResult.Error(ErrorCode.LocalException, "内部服务");
            }

        }



        /// <summary>
        ///     请求格式说明
        /// </summary>
        private readonly byte[] _planApiDescription =
        {
            5,
            (byte)ZeroByteCommand.Plan,
            ZeroFrameType.Plan,
            ZeroFrameType.Context,
            ZeroFrameType.Command,
            ZeroFrameType.Argument,
            ZeroFrameType.SerivceKey,
            ZeroFrameType.End
        };

        /// <summary>
        ///     请求格式说明
        /// </summary>
        private readonly byte[] _planPubDescription =
        {
            5,
           (byte) ZeroByteCommand.Plan,
            ZeroFrameType.Plan,
            ZeroFrameType.Context,
            ZeroFrameType.PubTitle,
            ZeroFrameType.TextContent,
            ZeroFrameType.SerivceKey,
            ZeroFrameType.End
        };

        /// <summary>
        ///     命令格式说明
        /// </summary>
        private readonly byte[] commandDescription =
        {
            4,
           (byte) ZeroByteCommand.Plan,
            ZeroFrameType.Plan,
            ZeroFrameType.Command,
            ZeroFrameType.Argument,
            ZeroFrameType.SerivceKey,
            ZeroFrameType.End
        };

        public ApiResult NewPlan(ClientPlan clientPlan)
        {
            if (string.IsNullOrWhiteSpace(clientPlan.station) || string.IsNullOrWhiteSpace(clientPlan.command))
                return ApiResult.Error(ErrorCode.LogicalError, "命令不能为空");
            var config = ZeroApplication.Config[clientPlan.station];
            if (config == null)
                return ApiResult.Error(ErrorCode.LogicalError, "站点名称错误");
            if (config.IsSystem)
                return ApiResult.Error(ErrorCode.LogicalError, "不允许对基础站点设置计划");

            clientPlan.station = config.StationName;


            var plan = new ZeroPlanInfo
            {
                plan_type = clientPlan.plan_type,
                plan_value = clientPlan.plan_value,
                plan_repet = clientPlan.plan_repet,
                description = clientPlan.description,
                no_skip = clientPlan.no_skip,
                plan_time = clientPlan.plan_time < new DateTime(1970, 1, 1) ? 0 : (int)((clientPlan.plan_time.ToUniversalTime().Ticks - 621355968000000000) / 10000000),
                skip_set = clientPlan.skip_set
            };
            if (clientPlan.plan_type == plan_date_type.week && plan.plan_value == 7)
            {
                plan.plan_value = 0;
            }
            if (clientPlan.plan_type > plan_date_type.time)
            {
                if (clientPlan.skip_set > 0)
                    plan.plan_repet += clientPlan.skip_set;
            }
            else
            {
                plan.skip_set = 0;
                plan.plan_repet = 1;
            }

            var socket = ApiProxy.GetSocket(clientPlan.station, null);
            if (socket == null)
                return ApiResult.Error(ErrorCode.LocalError, "无法联系ZeroCenter");

            using (socket)
            {
                bool success;
                switch (config.StationType)
                {
                    case ZeroStationType.Api:
                    case ZeroStationType.Vote:
                        success = socket.SendByServiceKey(_planApiDescription,
                            plan.ToZeroBytes(),
                            clientPlan.context.ToZeroBytes(),
                            clientPlan.command.ToZeroBytes(),
                            clientPlan.argument.ToZeroBytes());
                        break;
                    //Manage
                    case ZeroStationType.Notify:
                        success = socket.SendByServiceKey(_planPubDescription,
                            plan.ToZeroBytes(),
                            clientPlan.context.ToZeroBytes(),
                            clientPlan.command.ToZeroBytes(),
                            clientPlan.argument.ToZeroBytes());
                        break;
                    default:
                        clientPlan.command = clientPlan.command.ToLower();
                        if (clientPlan.command != "pause" && clientPlan.command != "close" && clientPlan.command != "resume")
                            return ApiResult.Error(ErrorCode.LogicalError, "系统命令仅支持暂停(pause)关闭(close)和恢复(resume) 非系统站点");
                        config = ZeroApplication.Config[clientPlan.station];
                        if (config == null)
                            return ApiResult.Error(ErrorCode.LogicalError, "站点名称无效");
                        if (config.IsSystem)
                            return ApiResult.Error(ErrorCode.LogicalError, "不允许对内置站点设置计划");

                        success = socket.SendByServiceKey(commandDescription,
                            plan.ToZeroBytes(),
                            clientPlan.command.ToZeroBytes(),
                            clientPlan.argument.ToZeroBytes());
                        break;
                }
                if (!success)
                {
                    ZeroTrace.SystemLog("NewPlan", "Send", socket.LastError);

                    return ApiResult.Error(ErrorCode.NetworkError, socket.LastError?.Text);
                }
                if (!socket.Recv(out var message))
                {
                    ZeroTrace.SystemLog("NewPlan", "Recv", socket.LastError);
                    return ApiResult.Error(ErrorCode.NetworkError, socket.LastError?.Text);
                }

                PlanItem.UnpackResult(message, out var item);
                return item.State == ZeroOperatorStateType.Plan
                    ? ApiResult.Succees()
                    : ApiResult.Error(ErrorCode.LogicalError, item.State.Text());

            }

        }
    }
}