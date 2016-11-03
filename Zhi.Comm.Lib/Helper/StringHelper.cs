using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Zhi.Comm.Lib
{
    public static class StringHelper
    {
        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }
        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;
            //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") ||
                System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
            {
                //当截取的起始位置超出字段串长度时
                if (p_StartIndex >= p_SrcString.Length)
                {
                    return "";
                }
                else
                {
                    return p_SrcString.Substring(p_StartIndex,
                                                   ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }


            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }



                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 转换SQL Like语句，文本中含有“[”、“%”等匹配符，成普通字符
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static string ConvertSQLLikeText(this String rawString)
        {
            return rawString.Replace("[", "[[]").Replace("%", "[%]").Replace("^", "[^]").Replace("_", "[_]");
        }

        /// <summary>
        /// 对手机号进行掩码处理
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static string MaskCellPhone(this String rawString)
        {
            string result = rawString;
            if (String.IsNullOrEmpty(rawString))
            {
                result = "";
            }
            else
            {
                // 手机号
                if (rawString.Length >= 11)
                {
                    // 13300000012
                    result = rawString.Substring(0, rawString.Length - 8) + "****" + rawString.Substring(rawString.Length - 4, 4);
                }
            }
            return result;
        }

        /// <summary>
        /// 过滤HTML代码
        /// </summary>
        /// <param name="rawString">原始字符串</param>
        /// <returns></returns>
        public static string FilterHTMLCode(this String rawString)
        {
            return FilterHTMLCode(rawString, false);
        }

        /// <summary>
        /// 过滤HTML代码
        /// </summary>
        /// <param name="rawString">原始字符串</param>
        /// <param name="needHTMLDecode">需要HTML解码</param>
        /// <returns></returns>
        public static string FilterHTMLCode(this String rawString, bool needHTMLDecode)
        {
            if (String.IsNullOrEmpty(rawString))
            {
                return "";
            }
            else
            {
                string result = rawString;
                if (needHTMLDecode)
                {
                    //TODO:
                    //result = HttpUtility.HtmlDecode(result);
                }
                result = Regex.Replace(rawString, @"<[^>]+>", "", RegexOptions.IgnoreCase);
                return result;
            }
        }

        /// <summary>
        /// 获取特定长度的字符串
        /// </summary>
        /// <param name="rawString">原始字符串</param>
        /// <param name="maxCharSize">字符长度，长度至少取2个字符</param>
        /// <returns></returns>
        public static string SpecifiedLengthString(this String rawString, int maxCharSize)
        {
            return SpecifiedLengthString(rawString, maxCharSize, true);
        }

        /// <summary>
        /// 获取特定长度的字符串
        /// </summary>
        /// <param name="rawString">原始字符串</param>
        /// <param name="maxCharSize">字符长度，长度至少取2个字符</param>
        /// <param name="showPoint">是否显示...</param>
        /// <returns></returns>
        public static string SpecifiedLengthString(this String rawString, int maxCharSize, bool showPoint)
        {
            if (String.IsNullOrEmpty(rawString))
            {
                return "";
            }
            else
            {
                // 长度至少取2个字符
                if (maxCharSize < 2)
                {
                    maxCharSize = 2;
                }
                string result = rawString;
                int currentByteLength = 0;
                for (int i = 0; i < result.Length; i++)
                {
                    currentByteLength += IsASCII(result[i]) ? 1 : 2;
                    // 截断字符后还有字符未显示出来，则减去
                    if (i > 1 && i < result.Length - 1)
                    {
                        if (showPoint)
                        {
                            if (currentByteLength >= maxCharSize * 2)
                            {
                                int newByteLength = currentByteLength;
                                int delCharCount = 0;
                                while (currentByteLength - newByteLength <= 3)
                                {
                                    newByteLength -= (IsASCII(result[i - delCharCount]) ? 1 : 2);
                                    delCharCount++;
                                }
                                result = result.Substring(0, i - delCharCount + 1) + "...";
                                break;
                            }
                        }
                        else
                        {
                            if (currentByteLength == maxCharSize * 2)
                            {
                                result = result.Substring(0, i + 1);
                            }
                            else if (currentByteLength > maxCharSize * 2)
                            {
                                result = result.Substring(0, i);
                            }
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 是否为ASCII码
        /// </summary>
        /// <param name="rawChar"></param>
        /// <returns></returns>
        private static bool IsASCII(char rawChar)
        {
            return Convert.ToInt32(rawChar) < 256 && rawChar != '—';
        }

        /// <summary>
        /// 将字符转成16进制表示。如&#9c8a;&#f3;
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static string ToHtmlHex(this String rawString)
        {
            string result = "";
            if (!String.IsNullOrEmpty(rawString))
            {
                foreach (var currentChar in rawString.ToCharArray())
                {
                    result += CharToHtmlHex(currentChar);
                }
                return rawString;
            }
            return result;
        }

        private static string CharToHtmlHex(char rawChar)
        {
            string result = "";
            int tmp = Convert.ToInt32(rawChar);
            if (tmp < 256)
            {
                result = "&#" + Convert.ToByte(tmp).ToString("H") + ";";
            }
            else
            {
                int highNum = tmp >> 8;
                int lowNum = tmp << 8;
                result = "&#" + Convert.ToByte(highNum).ToString("H") + Convert.ToByte(lowNum).ToString("H") + ";";
            }
            return result;
        }
    }
}