using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QiBuBlog.Util
{
    public static class MD5AndXOREncrypt
    {
        private const string Key = "9$</Zu!j";

        public static string DesEncrypt(string pToEncrypt, string ekey)
        {
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = Encoding.ASCII.GetBytes(ekey);
            des.IV = Encoding.ASCII.GetBytes(ekey);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (var b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        public static string DesDecrypt(string pToDecrypt, string dkey)
        {
            if (string.IsNullOrEmpty(pToDecrypt))
            {
                return "0";
            }
            var des = new DESCryptoServiceProvider();
            var inputByteArray = new byte[pToDecrypt.Length / 2];
            for (var x = 0; x < pToDecrypt.Length / 2; x++)
            {
                var i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.ASCII.GetBytes(dkey);
            des.IV = Encoding.ASCII.GetBytes(dkey);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        public static string Encrypt(string instr)
        {
            return SaltAndHash(instr, Key);
        }

        private static string SaltAndHash(this string rawString, string salt)
        {
            return (rawString + salt).SHA512();
        }

        private static string SHA512(this string text)
        {
            return text.EncryptOneWay<SHA512Cng, UTF8Encoding>();
        }

        private static string EncryptOneWay<TAlgorithm, TStringEncoding>(this string str)
            where TAlgorithm : HashAlgorithm
            where TStringEncoding : Encoding
        {
            var bytes = Activator.CreateInstance<TStringEncoding>().GetBytes(str);
            return BitConverter.ToString(Activator.CreateInstance<TAlgorithm>().ComputeHash(bytes)).Replace("-", "");
        }
    }
}
