using Common;

using Newtonsoft.Json;

using System;
using System.Text;

using TouchSocket.Core;
using TouchSocket.Sockets;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================客户端==================");
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connected = (client, e) =>
            {
                Console.WriteLine($"成功连接至[{client.IP}:{client.Port}]服务器");
            };//成功连接到服务器
            tcpClient.Disconnected = (client, e) =>
            {
                Console.WriteLine($"已从服务器[{client.IP}:{client.Port}]断开会话连接");
            };//从服务器断开连接，当连接不成功时不会触发。
            tcpClient.Received = (client, byteBlock, requestInfo) =>
            {
                //从服务器收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"从服务器接收到信息：{mes}");
            };

            //声明配置
            TouchSocketConfig config = new TouchSocketConfig();
            config.SetRemoteIPHost(new IPHost("127.0.0.1:7790"))
                .UsePlugin()
                .SetBufferLength(1024 * 10);

            //载入配置
            tcpClient.Setup(config);
            tcpClient.Connect();

            while (true)
            {
                Console.WriteLine("输入1发送消息");
                if ("1".Equals(Console.ReadLine()))
                {
                    var bytes = BuildTestMessage();
                    tcpClient.Send(bytes,0,bytes.Length);
                }
            }
        }

        private static byte[] BuildTestMessage()
        {
            SubscribeModel registerMessage = new SubscribeModel();
            registerMessage.ChannelNo = new string[] { "TD1001", "TD1002" };
            registerMessage.MessageType = new int[] { 0, 1 };
            var a = JsonConvert.SerializeObject(registerMessage);
            var b = Encoding.UTF8.GetBytes(a);
            var c = new byte[b.Length + 4];
            c[0] = 0xAF;
            c[1] = (byte)b.Length;
            b.CopyTo(c, 2);
            byte[] xor = ByteUtil.XorSumToInt16(c, 0, c.Length - 2);
            c[c.Length - 2] = (byte)xor[0];
            c[c.Length - 1] = (byte)xor[1];
            return c;
        }
    }
}
