using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Zhi.Comm.Lib;
namespace Zhi.Comm.WebLib
{
    public static class EncryptHelper
    {

    }
    /// <summary>
    /// AES 加密
    /// </summary>
    public static class AESEncrypt
    {
        /// <summary>
        /// 加密key, 可以自定义
        /// </summary>
        public static string encryptKey = "zhi.hunter";
        /// <summary>
        /// 默认密钥向量
        /// </summary>
        private static byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };
        /// <summary>
        /// AES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = StringHelper.GetSubString(encryptKey, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = Keys;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// AES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = StringHelper.GetSubString(decryptKey, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');
                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return "";
            }

        }

    }

    /// <summary>
    /// MD5加密类
    /// </summary>
    public static class MD5Encrypt
    {
        private static string decryptStr = null;
        private static string encryptStr = null;
        private static string md5Str = null;
        private static string messAge = null;
        private static string MD5desKey = null;
        private static string MD5desStr = null;

        public static void DesDecrypt()
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(decryptStr);
                byte[] bytes = Encoding.UTF8.GetBytes(MD5desKey);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = bytes;
                provider.IV = bytes;
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.Flush();
                stream2.FlushFinalBlock();
                stream2.Close();
                MD5desStr = Encoding.UTF8.GetString(stream.ToArray());
            }
            catch (Exception exception)
            {
                messAge = "DES解密出错：" + exception.Message;
            }
        }

        public static void DesEncrypt()
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(encryptStr);
                byte[] rgbKey = Encoding.UTF8.GetBytes(MD5desKey);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = rgbKey;
                provider.IV = rgbKey;
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(rgbKey, rgbKey), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                stream2.Flush();
                stream2.Close();
                MD5desStr = Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception exception)
            {
                messAge = "DES加密出错：" + exception.Message;
            }
        }

        public static void MD5Des()
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            try
            {
                byte[] bytes = provider.ComputeHash(Encoding.UTF8.GetBytes(md5Str));
                MD5desStr = Encoding.UTF8.GetString(bytes);
            }
            catch (Exception exception)
            {
                messAge = "MD5加密出错：" + exception.Message;
            }
        }

    }

    /// <summary>   
    /// 对称加密算法类   
    /// </summary>   
    public class SymmetricEncrypt
    {
        private SymmetricAlgorithm mObjCryptoService;
        private string key;
        /// <summary>   
        /// 对称加密类的构造函数   
        /// </summary>   
        public SymmetricEncrypt()
        {
            mObjCryptoService = new RijndaelManaged();
            key = "guz(%&hj7x89h$yubi0456ftmat5&fvhufcy76*h%(hilj$lhj!y6&(*jkp87jh7";
        }
        /// <summary>   
        /// 获得密钥   
        /// </summary>   
        /// <returns>密钥</returns>   
        private byte[] GetLegalKey()
        {
            string sTemp = key;
            mObjCryptoService.GenerateKey();
            byte[] byttemp = mObjCryptoService.Key;
            int keylength = byttemp.Length;
            if (sTemp.Length > keylength)
                sTemp = sTemp.Substring(0, keylength);
            else if (sTemp.Length < keylength)
                sTemp = sTemp.PadRight(keylength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>   
        /// 获得初始向量iv   
        /// </summary>   
        /// <returns>初试向量iv</returns>   
        private byte[] GetLegalIV()
        {
            string sTemp = "e4ghj*ghg7!rnifb&95guy86gfghub#er57hbh(u%g6hj($jhwk7&!hg4ui%$hjk";
            mObjCryptoService.GenerateIV();
            byte[] bytTemp = mObjCryptoService.IV;
            int ivlength = bytTemp.Length;
            if (sTemp.Length > ivlength)
                sTemp = sTemp.Substring(0, ivlength);
            else if (sTemp.Length < ivlength)
                sTemp = sTemp.PadRight(ivlength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>   
        /// 加密方法   
        /// </summary>   
        /// <param name="source">待加密的串</param>   
        /// <returns>经过加密的串</returns>   
        public string Encrypto(string source)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(source);
            MemoryStream ms = new MemoryStream();
            mObjCryptoService.Key = GetLegalKey();
            mObjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mObjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytout = ms.ToArray();
            return Convert.ToBase64String(bytout);
        }

        /// <summary>   
        /// 解密方法   
        /// </summary>   
        /// <param name="source">待解密的串</param>   
        /// <returns>经过解密的串</returns>
        public string Decrypto(string source)
        {
            string result = "";
            if (!String.IsNullOrEmpty(source))
            {
                try
                {
                    byte[] bytIn = Convert.FromBase64String(source);
                    MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                    mObjCryptoService.Key = GetLegalKey();
                    mObjCryptoService.IV = GetLegalIV();
                    ICryptoTransform encrypto = mObjCryptoService.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                    StreamReader sr = new StreamReader(cs);
                    result = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                   
                }
            }
            return result;
        }
    }

    /// <summary>
    /// Base64简单加密
    /// </summary>
    public static class Base64Encrypt
    {
        private static string code_type = "unicode";
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EnCodeBase64(string code)
        {

            string encode = "";

            if (code != null && code.Length > 0)
            {
                byte[] bytes = System.Text.Encoding.GetEncoding(code_type).GetBytes(code);
                try
                {
                    encode = Convert.ToBase64String(bytes);
                }
                catch
                {
                }
            }
            return encode;
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DeCodeBase64(string code)
        {
            string decode = "";

            if (code != null && code.Length > 0)
            {
                try
                {
                    decode = Encoding.GetEncoding(code_type).GetString(Convert.FromBase64String(code));
                }
                catch (Exception ex)
                {
                   
                }
            }
            return decode;
        }
    }

    /// <summary>
    /// Base32简单加密
    /// </summary>
    public sealed class Base32Encrypt
    {
        // the valid chars for the encoding
        private static string ValidChars = "QAZ2WSX3" + "EDC4RFV5" + "TGB6YHN7" + "UJM8K9LP";

        /// <summary>
        /// 将 8 位无符号整数数组转换为它的等效 String 表示形式（使用 Base 32 数字编码）。
        /// </summary>
        /// <param name="inArray">一个 8 位无符号整数数组。</param>
        /// <returns>inArray 内容的 String 表示形式，以基数为 32 的数字表示。</returns>
        public static string ToBase32String(byte[] inArray)
        {
            return ToBase32String(inArray, 0, inArray.Length);
        }

        /// <summary>
        /// 将 8 位无符号整数数组的子集转换为其等效的、用 Base 32 数字编码的 String 表示形式。
        /// </summary>
        /// <param name="inArray">一个 8 位无符号整数数组。</param>
        /// <param name="offset">inArray 中的偏移量。</param>
        /// <param name="length">要转换的 inArray 的元素数。</param>
        /// <returns>inArray 中从位置 offset 开始的 length 元素的 String 表示形式，以 Base 32 表示。</returns>
        public static string ToBase32String(byte[] inArray, int offset, int length)
        {
            StringBuilder sb = new StringBuilder();         // holds the base32 chars
            byte index;
            int hi = 5;
            int currentByte = offset;

            while (currentByte < offset + length)
            {
                // do we need to use the next byte?
                if (hi > 8)
                {
                    // get the last piece from the current byte, shift it to the right
                    // and increment the byte counter
                    index = (byte)(inArray[currentByte++] >> (hi - 5));
                    if (currentByte != offset + length)
                    {
                        // if we are not at the end, get the first piece from
                        // the next byte, clear it and shift it to the left
                        index = (byte)(((byte)(inArray[currentByte] << (16 - hi)) >> 3) | index);
                    }

                    hi -= 3;
                }
                else if (hi == 8)
                {
                    index = (byte)(inArray[currentByte++] >> 3);
                    hi -= 3;
                }
                else
                {

                    // simply get the stuff from the current byte
                    index = (byte)((byte)(inArray[currentByte] << (8 - hi)) >> 3);
                    hi += 5;
                }

                sb.Append(ValidChars[index]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将指定的 String（它将二进制数据编码为 base 32 数字）转换成等效的 8 位无符号整数数组。
        /// </summary>
        /// <param name="s"></param>
        /// <returns>等效于 s 的 8 位无符号整数数组。</returns>
        public static byte[] FromBase32String(string s)
        {
            int numBytes = s.Length * 5 / 8;
            byte[] bytes = new Byte[numBytes];

            // all UPPERCASE chars
            string str = s.ToUpper();

            int bit_buffer;
            int currentCharIndex;
            int bits_in_buffer;

            if (str.Length < 3)
            {
                bytes[0] = (byte)(ValidChars.IndexOf(str[0]) | ValidChars.IndexOf(str[1]) << 5);
                return bytes;
            }

            bit_buffer = (ValidChars.IndexOf(str[0]) | ValidChars.IndexOf(str[1]) << 5);
            bits_in_buffer = 10;
            currentCharIndex = 2;
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)bit_buffer;
                bit_buffer >>= 8;
                bits_in_buffer -= 8;
                while (bits_in_buffer < 8 && currentCharIndex < str.Length)
                {
                    bit_buffer |= ValidChars.IndexOf(str[currentCharIndex++]) << bits_in_buffer;
                    bits_in_buffer += 5;
                }
            }

            return bytes;
        }

        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="s"></param>
        /// <param name="crypto">用于对称算法的密钥</param>
        /// <returns></returns>
        public static string Encrypt(string s, string crypto)
        {
            Byte[] rgbKey = { };
            Byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                rgbKey = System.Text.Encoding.UTF8.GetBytes(crypto.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inArray = System.Text.Encoding.UTF8.GetBytes(s);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cs.Write(inArray, 0, inArray.Length);
                cs.FlushFinalBlock();
                return ToBase32String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="s"></param>
        /// <param name="crypto">用于对称算法的密钥</param>
        /// <returns></returns>
        public static string Decrypt(string s, string crypto)
        {
            Byte[] rgbKey = { };
            Byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                rgbKey = System.Text.Encoding.UTF8.GetBytes(crypto.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inArray = FromBase32String(s);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cs.Write(inArray, 0, inArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public static class DESEncrypt
    {

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "litianping");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }



 


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "litianping");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }




    }
}