using System;
using System.Collections.Generic;
using System.Text;

namespace LesoftWuye2.Application.YsPay
{
    /// <summary>
    /// Common 的摘要说明
    /// </summary>
    public class Common
    {
        public static bool isEmpty(string src)
        {
            if (null == src || "".Equals(src) || "-1".Equals(src))
            {
                return true;
            }
            return false;
        }
        public static Dictionary<string, string> SortDictionary(Dictionary<string, string> dic)
        {
            List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>(dic);
            myList.Sort(delegate (KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
            {
                return s1.Key.CompareTo(s2.Key);
            });
            dic.Clear();
            foreach (KeyValuePair<string, string> pair in myList)
            {
                dic.Add(pair.Key, pair.Value);
            }
            return dic;
        }

        #region RSA
        public static string GetPem(string pem, string type= "RSA PRIVATE KEY")
        {
            string header = String.Format("-----BEGIN {0}-----", type);
            string footer = String.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header) + header.Length;
            int end = pem.IndexOf(footer, start);

            string base64 = pem.Substring(start, (end - start));

            return base64;
            //return Convert.FromBase64String(base64);
        }
        private static byte[] GetPem( byte[] data,string type)
        {
            string pem = Encoding.UTF8.GetString(data);
            string header = String.Format("-----BEGIN {0}-----", type);
            string footer = String.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header) + header.Length;
            int end = pem.IndexOf(footer, start);

            string base64 = pem.Substring(start, (end - start));

            return Convert.FromBase64String(base64);
        }
        #endregion

        #region Base64
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            try
            {
                source = Convert.ToBase64String(bytes);
            }
            catch
            {
                source = source;
            }
            return source;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }
        #endregion
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
                //prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value,Encoding.UTF8) + "&");
            }

            //去掉最后一个&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }
        public static string CreateString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                if(temp.Value.Trim()!="")
                    prestr.Append(temp.Key + "=" + temp.Value + "&");
                //prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value,Encoding.UTF8) + "&");
            }

            //去掉最后一个&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }
    }
}