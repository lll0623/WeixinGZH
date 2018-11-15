using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LesoftWuye2.Application.YsPay
{
    public class SecurityUtil
    {
        public static string RSASignString(string plaintext, string privateKey,string encode="utf-8")
        {
            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(privateKey);
                //使用MD5进行摘要算法
                byte[] md5Bytes = Encoding.GetEncoding(encode).GetBytes(plaintext);
                //函数内部会再次计算哈希值，第2次哈希算法是SHA1
                byte[] encryptedData = RSAalg.SignData(md5Bytes, new SHA1CryptoServiceProvider());
                return Convert.ToBase64String(encryptedData);
            }
        }
        public static string RSAMD5SignString(string plaintext, string privateKey, string encode = "utf-8")
        {
            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(privateKey);
                //使用MD5进行摘要算法
                byte[] md5Bytes = Encoding.GetEncoding(encode).GetBytes(plaintext);
                //函数内部会再次计算哈希值，第2次哈希算法是SHA1
                byte[] encryptedData = RSAalg.SignData(md5Bytes, new MD5CryptoServiceProvider());
                return Convert.ToBase64String(encryptedData);
            }
        }
        /// <summary>
        /// 数字签名
        /// </summary>
        /// <param name="plaintext">原文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>签名</returns>
        public static string HashAndSignString(string plaintext, string privateKey,string encode="utf-8")
        {
            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(privateKey);
                //使用MD5进行摘要算法
                byte[] md5Bytes= MD5Encrypt(plaintext,encode);
                //函数内部会再次计算哈希值，第2次哈希算法是SHA1
                byte[] encryptedData = RSAalg.SignData(md5Bytes, new SHA1CryptoServiceProvider());
                return Convert.ToBase64String(encryptedData);
            }
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="plaintext">原文</param>
        /// <param name="SignedData">签名</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static bool VerifySigned(string plaintext, string SignedData, string publicKey,string encode="utf-8")
        {
            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(publicKey);
                byte[] md5Bytes = MD5Encrypt(plaintext,encode);
                byte[] signedDataBytes = Convert.FromBase64String(SignedData);
                return RSAalg.VerifyData(md5Bytes, new SHA1CryptoServiceProvider(), signedDataBytes);
            }
        }
        public static bool VerifySignedEncoding(string plaintext, string SignedData, string publicKey,string encode="utf-8")
        {
            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(publicKey);
                byte[] md5Bytes = Encoding.GetEncoding(encode).GetBytes(plaintext);
                byte[] signedDataBytes = Convert.FromBase64String(SignedData);
                return RSAalg.VerifyData(md5Bytes, new SHA1CryptoServiceProvider(), signedDataBytes);
            }
        }
        public static bool VerifyMD5SignedEncoding(string plaintext, string SignedData, string publicKey, string encode = "utf-8")
        {
            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(publicKey);
                byte[] md5Bytes = Encoding.GetEncoding(encode).GetBytes(plaintext);
                byte[] signedDataBytes = Convert.FromBase64String(SignedData);
                return RSAalg.VerifyData(md5Bytes, new MD5CryptoServiceProvider(), signedDataBytes);
            }
        }
        /// <summary>
        /// 加载密钥
        /// </summary>
        /// <param name="path"></param>
        /// <param name="password"></param>
        /// <param name="keyFlag">true:加载私钥，false:加载公钥</param>
        /// <returns></returns>
        public static String loadKey(String path,String password,bool keyFlag)
        {
            X509Certificate2 c3 = DataCertificate.GetCertificateFromPfxFile(path, password);
            string key = "";
            if (keyFlag)
            {
                key = c3.PrivateKey.ToXmlString(keyFlag);
            }else
            {
                key = c3.PublicKey.Key.ToXmlString(keyFlag);
            }
            return key;
        }

        ///   <summary>
        ///   给一个字符串进行MD5加密
        ///   </summary>
        ///   <param   name="strText">待加密字符串</param>
        ///   <returns>加密后的字符串</returns>
        public static byte[] MD5Encrypt(string strText,string encode="utf-8")
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.GetEncoding(encode).GetBytes(strText));
            return result;
        }
    }
}
