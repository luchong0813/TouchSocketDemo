using TouchSocket.Sockets;

namespace Service
{
    /// <summary>
    /// ClassName：  PacketRequestInfo
    /// Description：
    /// Author：     luc
    /// CreatTime：  2023/2/6 9:43:59  
    /// </summary>
    public class PacketRequestInfo : IRequestInfo
    {
        /// <summary>
        /// 头，固定内容，1字节
        /// </summary>
        public byte Header { get; set; }

        /// <summary>
        /// 数据长度，1字节
        /// </summary>
        public byte DataLength { get; set; }

        /// <summary>
        /// 数据体，N字节
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// 尾，校验位，2字节
        /// </summary>
        public byte[] Tail { get; set; }
    }
}
