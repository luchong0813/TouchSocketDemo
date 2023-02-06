using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ByteUtil
    {
        private static char[] HEX_CHARS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        private static byte[] BITS = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

        /// <summary>
        /// 将字节数组转换为HEX形式的字符串, 使用指定的间隔符
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="separator"></param>
        public static string ByteToHex(byte[] buf, string separator)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < buf.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(separator);
                }
                sb.Append(HEX_CHARS[buf[i] >> 4]).Append(HEX_CHARS[buf[i] & 0x0F]);
            }
            return sb.ToString();
        }

        /// <summary>
		/// 将字节数组转换为HEX形式的字符串, 使用指定的间隔符
		/// </summary>
		/// <param name="buf"></param>
		/// <param name="separator"></param>
		public static string ByteToHex(byte[] buf, char c)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < buf.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(c);
                }
                sb.Append(HEX_CHARS[buf[i] >> 4]).Append(HEX_CHARS[buf[i] & 0x0F]);
            }
            return sb.ToString();
        }

        /// <summary>
		/// 将字节数组转换为HEX形式的字符串, 没有间隔符
		/// </summary>
		/// <param name="buf"></param>
		/// <returns></returns>
		public static string ByteToHex(byte[] buf)
        {
            return ByteToHex(buf, string.Empty);
        }

        /// <summary>
		/// 将字节数组转换为HEX形式的字符串
		/// 转换后的字符串长度为字节数组长度的两倍
		/// 如: 1, 2 转换为 0102
		/// </summary>
		/// <param name="buf"></param>
		public static string ByteToHex(byte b)
        {
            return string.Empty + HEX_CHARS[b >> 4] + HEX_CHARS[b & 0x0F];
        }

        /// <summary>
		/// 将字节流信息显示在Console上
		/// </summary>
		/// <param name="bytes"></param>
		public static void DumpBytes(byte[] bytes)
        {
            DumpBytes(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 将字节流信息显示在Console上
        /// </summary>
        /// <param name="bytes"></param>
        public static void DumpBytes(byte[] bytes, int offset, int len)
        {
            for (int i = 0; i < len; i++)
            {
                if (i > 0 && i % 16 == 0)
                {
                    Console.WriteLine();
                }
                Console.Write(ByteToHex(bytes[i + offset]));
                Console.Write(' ');
            }
            Console.WriteLine();
        }

        /// <summary>
		/// 计算字节块的模256校验和
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="offset"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		public static byte SumBytes(byte[] bytes, int offset, int len)
        {
            int sum = 0;
            for (int i = 0; i < len; i++)
            {
                sum += bytes[i + offset];
                if (sum >= 256)
                {
                    sum = sum % 256;
                }
            }
            return (byte)sum;
        }

        /// <summary>
		/// 计算字节块的异或校验和
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="offset"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		public static byte XorSumBytes(byte[] bytes, int offset, int len)
        {
            byte sum = bytes[0 + offset];
            for (int i = 1; i < len; i++)
            {
                sum = (byte)(sum ^ bytes[i + offset]);
            }
            return sum;
        }

        /// <summary>
        /// 计算字节块的校验和
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns>返回两字节的校验和</returns>
        public static byte[] XorSumToInt16(byte[] bytes, int offset, int len)
        {
            short sum = bytes[0 + offset];
            for (int i = 1; i < len; i++)
            {
                sum = (short)(sum + bytes[i + offset]);
            }
            return GetBytes(sum);
        }

        /// <summary>
        /// 计算字节块的异或校验和
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte XorSumBytes(byte[] bytes)
        {
            return XorSumBytes(bytes, 0, bytes.Length);
        }

        /// <summary>
		/// 比较两个字节块是否相等。相等返回true否则false
		/// </summary>
		/// <param name="bytes1"></param>
		/// <param name="offset1"></param>
		/// <param name="bytes2"></param>
		/// <param name="offset2"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		public static bool CompareBytes(byte[] bytes1, int offset1, byte[] bytes2, int offset2, int len)
        {
            for (int i = 0; i < len; i++)
            {
                if (bytes1[i + offset1] != bytes2[i + offset2])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将两个字符的hex转换为byte
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static byte HexToByte(char[] hex, int offset)
        {
            byte result = 0;
            for (int i = 0; i < 2; i++)
            {
                char c = hex[i];
                byte b = 0;
                switch (c)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        b = (byte)((int)c - (int)'0');
                        break;
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                        b = (byte)(10 + (int)c - (int)'A');
                        break;
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                        b = (byte)(10 + (int)c - (int)'a');
                        break;
                }
                if (i == 0)
                {
                    b = (byte)(b * 16);
                }
                result += b;
            }

            return result;
        }

        /// <summary>
        /// 将两个字符的hex转换为byte
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static byte HexToByte(byte[] hex, int offset)
        {
            char[] chars = { (char)hex[offset], (char)hex[offset + 1] };
            return HexToByte(chars, 0);
        }

        /// <summary>
        /// 设置某个字节的指定位
        /// </summary>
        /// <param name="b">需要设置的字节</param>
        /// <param name="pos">1-8, 1表示最低位, 8表示最高位</param>
        /// <param name="on">true表示设置1, false表示设置0</param>
        public static void ByteSetBit(ref byte b, int pos, bool on)
        {
            int temp = BITS[pos - 1];

            if (!on)
            {
                //取反
                temp = temp ^ 0xFF;
            }

            b = (byte)(on ? (b | temp) : (b & temp));
        }

        /// <summary>
        /// 判断某个byte的某个位是否为1
        /// </summary>
        /// <param name="b"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool ByteGetBit(byte b, int pos)
        {
            int temp = BITS[pos - 1];
            return (b & temp) != 0;
        }

        /// <summary>
		/// 设置双比特值
		/// </summary>
		/// <param name="b">需要设置的字节</param>
		/// <param name="low">低位, 1-7</param>
		/// <param name="val">值，0-3</param>
		/// <returns></returns>
		public static void ByteSetBitPair(ref byte b, int low, int val)
        {
            if (low < 1 || low > 7)
            {
                throw new ArgumentException(string.Format("无效的low值:{0}", low));
            }

            switch (val)
            {
                case 0:
                    {
                        ByteUtil.ByteSetBit(ref b, low, false);
                        ByteUtil.ByteSetBit(ref b, low + 1, false);
                        break;
                    }
                case 1:
                    {
                        ByteUtil.ByteSetBit(ref b, low, true);
                        ByteUtil.ByteSetBit(ref b, low + 1, false);
                        break;
                    }
                case 2:
                    {
                        ByteUtil.ByteSetBit(ref b, low, false);
                        ByteUtil.ByteSetBit(ref b, low + 1, true);
                        break;
                    }
                case 3:
                    {
                        ByteUtil.ByteSetBit(ref b, low, true);
                        ByteUtil.ByteSetBit(ref b, low + 1, true);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(string.Format("无效的val值:{0}", val));
                    }
            }
        }

        /// <summary>
		/// 读取双比特值
		/// </summary>
		/// <param name="b">需要读取的字节</param>
		/// <param name="low">低位, 0-6</param>
		/// <returns>0-3</returns>
		public static byte ByteGetBitPair(byte b, int low)
        {
            if (low < 0 || low > 7)
            {
                throw new ArgumentException(string.Format("无效的low值:{0}", low));
            }

            int x = 0;
            x += ByteUtil.ByteGetBit(b, low) ? 1 : 0;
            x += ByteUtil.ByteGetBit(b, low + 1) ? 2 : 0;

            return (byte)x;
        }

        public static String ShortSetHign8(int value)
        {
            int v = (value & 0X0FFF) | 0X8000;
            return v.ToString();
        }

        public static String ShortToBCD(int value)
        {
            // 先转换成二进制串
            string ret = string.Empty;
            int m = value > 0 ? value : -value;
            while (m > 0)
            {
                string c = string.Empty;
                int r = m % 10;
                m /= 10;
                while (r > 0)
                {
                    c = (r % 2) + c;
                    r /= 2;
                }
                ret = c.PadLeft(4, '0') + ret;
            }

            // 计算二进制对应的十进制值
            int v = 0;
            int t = 1;
            for (int i = ret.Length - 1; i >= 0; i--)
            {
                v += (ret[i] - '0') * t;
                t *= 2;
            }

            if (value < 0)
            {
                return String.Format("-{0}", v);
            }
            return v.ToString();
        }

        /// <summary>
        /// 将四个字节转换为一个int 默认高位在前
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="isLittleEndian">是否小端模式 即低位在前</param>
        /// <returns></returns>
        public static int BytesToInt32(byte[] data, int startIndex, bool isLittleEndian = false)
        {
            if (isLittleEndian)
            {
                return data[startIndex] | data[startIndex + 1] << 8 | data[startIndex + 2] << 16 | data[startIndex + 3] << 24;
            }
            return data[startIndex + 3] | data[startIndex + 2] << 8 | data[startIndex + 1] << 16 | data[startIndex] << 24;
        }

        /// <summary>
        /// 将int转换成4字节byte 默认高位在前
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isLittleEndian">是否小端模式，即低位在前</param>
        /// <returns></returns>
        public static byte[] GetBytes(int value, bool isLittleEndian = false)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(value & 0x000000FF);
            bytes[1] = (byte)((value & 0x0000FF00) >> 8);
            bytes[2] = (byte)((value & 0x00FF0000) >> 16);
            bytes[3] = (byte)((value & 0xFF000000) >> 24);
            if (!isLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        /// <summary>
        /// 将2字节转成short 默认高位在前
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="isLittleEndian">是否小端模式 即低位在前</param>
        /// <returns></returns>
        public static short BytesToInt16(byte[] data, int startIndex, bool isLittleEndian = false)
        {
            if (isLittleEndian)
            {
                return (short)(data[startIndex] | data[startIndex + 1] << 8);
            }
            return (short)(data[startIndex + 1] | data[startIndex] << 8);
        }

        /// <summary>
        /// 将short转换成2字节byte 默认高位在前
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isLittleEndian">是否小端模式，即低位在前</param>
        /// <returns></returns>
        public static byte[] GetBytes(short value, bool isLittleEndian = false)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(value & 0x000000FF);
            bytes[1] = (byte)((value & 0x0000FF00) >> 8);
            if (!isLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
    }
}
