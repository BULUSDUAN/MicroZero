using System;
using System.Threading.Tasks;
using Agebull.MicroZero;
using ZeroMQ;

namespace RpcTest
{
    internal class Tester
    {
        static string address = "tcp://192.168.123.129:8000";
        public static async Task StartTest()
        {
            //address = "tcp://192.168.240.132:8000";
            await TestFrame("��֡", ZeroOperatorStateType.FrameInvalid);
            await TestFrame("��֡", ZeroOperatorStateType.FrameInvalid, new byte[0]);
            await TestFrame("��֡", ZeroOperatorStateType.FrameInvalid, "".ToZeroBytes());
            await TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[1] { 1 });
            await TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[1] { 1 });
            await TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[1] { 1 }, new byte[1] { 1 });
            await TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[] { 1, 1 }, new byte[1] { 0 });
            await TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[] { 1, 1 }, new byte[1] { 1 });
            //TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[] { 1, 1 }, new byte[0]);
            await TestFrame("��������֡", ZeroOperatorStateType.ArgumentInvalid, new byte[] { 1, 1, (byte)'c', 0 }, new byte[1] { 1 });
            //״̬����
            for (byte state = 1; state < byte.MaxValue; state++)
            {
                await TestFrame($"״̬֡({(ZeroByteCommand)state})", ZeroOperatorStateType.ArgumentInvalid, new byte[] { 0, state, 0 });
            }
            await TestFrame("��֡", ZeroOperatorStateType.ArgumentInvalid, new byte[] { 1, 1, (byte)'c', 0 }, new byte[0]);

            //address = "tcp://192.168.123.129:8010";
            ////״̬����
            //for (byte state = 1; state < byte.MaxValue; state++)
            //{
            //    TestFrame($"״̬֡({(ZeroOperatorStateType)state})", ZeroOperatorStateType.ArgumentInvalid,"agebull".ToZeroBytes(), new byte[] { 0, state, 0 });
            //}
            Console.ReadLine();
        }
        static async Task TestFrame(string title, ZeroOperatorStateType state, params byte[][] frames)
        {
            Console.Error.Write(title);
            Console.ForegroundColor = ConsoleColor.Red;
            await TestFrameInner(state, frames);
            Console.Error.WriteLine();
            Console.Error.WriteLine("**----**");
            Console.ResetColor();
        }
        static async Task TestFrameInner(ZeroOperatorStateType state, byte[][] frames)
        {
            using (var socket = ZSocket.CreateOnceSocket(address, ZSocket.CreateIdentity(), ZSocketType.DEALER))
            {
                socket.SetOption(ZSocketOption.RCVTIMEO, 30000);
                if (!socket.SendTo(frames, new byte[][] { }))
                {
                    Console.Error.Write(" : Send Error");
                    return;
                }
                var result = await socket.Receive<ZeroResult>();
                if (!result.InteractiveSuccess)
                    Console.Error.WriteLine(" : Receive Error");
                if (result.State != state)
                    Console.Error.Write($"(bad) : {result.State}");
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"(success) : {state}");
                }
            }
        }
    }
}