using System;
using Agebull.MicroZero;
using ZeroMQ;

namespace RpcTest
{
    internal class Tester
    {
        const string address = "tcp://192.168.123.129:8000";

        static readonly byte[] serviceKey = "agebull".ToZeroBytes();
        public static void StartTest()
        {
            //address = "tcp://192.168.240.132:8000";
            TestFrame("��֡", ZeroOperatorStateType.FrameInvalid);
            TestFrame("��֡", ZeroOperatorStateType.FrameInvalid, new byte[0]);
            TestFrame("��֡", ZeroOperatorStateType.FrameInvalid, "".ToZeroBytes());
            TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[] { 1 });
            TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[] { 1 });
            TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[] { 1 }, new byte[] { 1 });
            TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[] { 1, 1 }, new byte[] { 0 });
            TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[] { 1, 1 }, new byte[] { 1 });
            //TestFrame("����֡", ZeroOperatorStateType.FrameInvalid, new byte[] { 1, 1 }, new byte[0]);
            TestFrame("��������֡", ZeroOperatorStateType.ArgumentInvalid, new byte[] { 1, 1, (byte)'c', 0 }, new byte[] { 1 });
            //״̬����
            for (byte state = 1; state < byte.MaxValue; state++)
            {
                TestFrame($"״̬֡({(ZeroByteCommand)state})", ZeroOperatorStateType.ArgumentInvalid, new byte[] { 0, state, 0 });
            }
            TestFrame("��֡", ZeroOperatorStateType.ArgumentInvalid, new byte[] { 1, 1, (byte)'c', 0 }, new byte[0]);

            //address = "tcp://192.168.123.129:8010";
            ////״̬����
            //for (byte state = 1; state < byte.MaxValue; state++)
            //{
            //    TestFrame($"״̬֡({(ZeroOperatorStateType)state})", ZeroOperatorStateType.ArgumentInvalid,"agebull".ToZeroBytes(), new byte[] { 0, state, 0 });
            //}
            Console.ReadLine();
        }
        static void TestFrame(string title, ZeroOperatorStateType state, params byte[][] frames)
        {
            Console.Error.Write(title);
            Console.ForegroundColor = ConsoleColor.Red;
            TestFrameInner(state, frames);
            Console.Error.WriteLine();
            Console.Error.WriteLine("**----**");
            Console.ResetColor();
        }
        static void TestFrameInner(ZeroOperatorStateType state, byte[][] frames)
        {
            using (var socket = ZSocketEx.CreateOnceSocket(address, serviceKey, ZSocket.CreateIdentity()))
            {
                socket.SetOption(ZSocketOption.RCVTIMEO, 30000);
                if (!socket.SendByServiceKey(frames))
                {
                    Console.Error.Write(" : Send Error");
                    return;
                }
                var result = socket.Receive<ZeroResult>();
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