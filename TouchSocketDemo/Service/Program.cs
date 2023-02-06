using System;
using System.Text;

using TouchSocket.Core;
using TouchSocket.Sockets;

namespace Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================服务端[端口：7790]==================");

            TcpService service = new TcpService();
            service.Connecting = (client, e) =>
            {
                Console.WriteLine($"客户端[{client.IP}:{client.Port}]正在连接");
            };
            service.Connected = (client, e) =>
            {
                Console.WriteLine($"客户端[{client.IP}:{client.Port}]已连接");
            };
            service.Disconnected = (client, e) =>
            {
                Console.WriteLine($"客户端[{client.IP}:{client.Port}]已断开");
            };

            service.Received = (client, byteBlock, requestInfo) =>
            {
                if (requestInfo is PacketRequestInfo packet)
                {
                    string body = Encoding.UTF8.GetString(packet.Body, 0, packet.DataLength);
                    Console.WriteLine($"收到消息：{body}");
                }
            };

            service.Setup(new TouchSocketConfig()//载入配置     
                .SetListenIPHosts(new IPHost[] { new IPHost(7790) })
                .SetDataHandlingAdapter(() => { return new PacketDataReceiveAdapter(); }))
                .Start();//启动

            Console.ReadLine();
        }
    }
}
