using System;
using System.Security.Cryptography.X509Certificates;

namespace LesoftWuye2.Application.YsPay
{
    public class DataCertificate
    {
        /// <summary>
        /// 根据私钥证书得到证书实体，得到实体后可以根据其公钥和私钥进行加解密
        /// 加解密函数使用DEncrypt的RSACryption类
        /// </summary>
        /// <param name="pfxFileName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static X509Certificate2 GetCertificateFromPfxFile(string pfxFileName,string password)
        {
            try
            {
                return new X509Certificate2(pfxFileName, password, X509KeyStorageFlags.Exportable);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}