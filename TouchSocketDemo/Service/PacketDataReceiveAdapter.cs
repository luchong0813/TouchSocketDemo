using Common;

using System.Linq;

using TouchSocket.Core;
using TouchSocket.Sockets;

namespace Service
{
    /// <summary>
    /// ClassName：  PacketDataReceiveAdapter
    /// Description：
    /// Author：     luc
    /// CreatTime：  2023/2/6 9:43:39  
    /// </summary>
    public class PacketDataReceiveAdapter : CustomDataHandlingAdapter<PacketRequestInfo>
    {
        private const byte HEAD = 0xAF;

        protected override FilterResult Filter(ByteBlock byteBlock, bool beCached, ref PacketRequestInfo request, ref int tempCapacity)
        {
            var srcData = byteBlock.ToArray();
            //不满足协议最短长度
            if (srcData.Length < 4)
            {
                return FilterResult.GoOn;
            }
            //包头不符合协议
            if (srcData.First() != HEAD)
            {
                return FilterResult.GoOn;
            }
            //校验
            byte[] xor = ByteUtil.XorSumToInt16(srcData, 0, srcData.Length - 2);
            if (srcData[srcData.Length - 2] != xor[0] && srcData[srcData.Length - 1] != xor[1])
            {
                return FilterResult.GoOn;
            }
            //取包体数据
            byte[] body = new byte[srcData[1]];
            srcData.ToList().CopyTo(2, body, 0, srcData[1]);
            PacketRequestInfo packet = new PacketRequestInfo
            {
                Header = HEAD,
                DataLength = srcData[1],
                Body = body,
                Tail = xor
            };
            request = packet;
            return FilterResult.Success;
        }
    }
}
