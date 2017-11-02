using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZED.Train.Common
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class MD5Encrypt
    {
        public static string GetDigest(string str)
        {
            //通过指定摘要算法，生成摘要信息
            MD5 md5 = MD5.Create();
            byte[] byteStr = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            // 将生成的摘要转成16进制数表示
            string digest = ByteToHexStr(byteStr);
            return digest;
        }

        /// <summary>
        /// 字节转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("x2");
                }
            }
            return returnStr;
        }
    }
}
