using System;

namespace NetMQ.WebSockets
{
    /// <summary>
    /// WebSocket·��
    /// </summary>
    public class WsRouter : WsSocket
    {
        /// <summary>
        /// ����
        /// </summary>
        public WsRouter(string addr)
            : base(addr,(id,add) => new RouterShimHandler(add,id))
        {
        }

        /// <summary>
        /// ·�ɵ�Ƭ
        /// </summary>
        private class RouterShimHandler : BaseShimHandler
        {
            /// <summary>
            /// ����
            /// </summary>
            public RouterShimHandler(string addr,int id)
                : base(addr, id)
            {
            }

            protected override void OnOutgoingMessage(NetMQMessage message)
            {
                byte[] identity = message.Pop().ToByteArray();

                //  Each frame is a full ZMQ message with identity frame
                while (message.FrameCount > 0)
                {
                    var data = message.Pop().ToByteArray(false);
                    bool more = message.FrameCount > 0;

                    WriteOutgoing(identity, data, more);
                }
            }

            protected override void OnIncomingMessage(byte[] identity, NetMQMessage message)
            {
                message.Push(identity);

                WriteIngoing(message);
            }

            protected override void OnNewClient(byte[] identity)
            {
                var name = BitConverter.ToString(identity);
                Console.WriteLine($"{name} is join");
            }

            protected override void OnClientRemoved(byte[] identity)
            {
                var name = BitConverter.ToString(identity);
                Console.WriteLine($"{name} is left");
            }
        }
    }

}