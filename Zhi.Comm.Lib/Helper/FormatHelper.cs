using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zhi.Comm.Lib
{
    public static class FormatHelper
    {
        /// <summary>
        /// 格式化String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            return value;
        }
        /// <summary>
        /// 格式化long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long FormatLong(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            long intuid = 0;
            if (long.TryParse(value, out intuid))
            {
                return long.Parse(value);
            }
            return intuid;
        }
        /// <summary>
        /// 格式化Int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int FormatInt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            int intuid = 0;
            if (int.TryParse(value, out intuid))
            {
                intuid = int.Parse(value);
            }
            else
            {
                intuid = 0;
            }
            return intuid;
        }
        /// <summary>
        /// 格式化Byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte FormatByte(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            byte intuid = 0;
            if (byte.TryParse(value, out intuid))
            {
                intuid = byte.Parse(value);
            }
            else
            {
                intuid = 0;
            }
            return intuid;
        }
        /// <summary>
        /// 格式化Boolen
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool FormatBoolen(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
        
            if (value.Trim().ToLower() == "1" || value.Trim().ToLower() == "true")
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 整形格式化为两位小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntFormatString(int value)
        {
            return value.ToString("D2");
        }
        /// <summary>
        /// 格式化页码
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <returns>有异常时，返回1</returns>
        public static int FormatPageNumber(string PageNumber)
        {
            if (string.IsNullOrEmpty(PageNumber))
            {
                return 1;
            }
            int intPageNumber = 1;
            if (int.TryParse(PageNumber, out intPageNumber))
            {
                intPageNumber = int.Parse(PageNumber);
            }
            else
            {
                intPageNumber = 1;
            }
            return intPageNumber;
        }

        /// <summary>
        /// 将时间ticks 转换成标准时间日期
        /// </summary>
        /// <param name="dateline"></param>
        /// <returns></returns>
        public static DateTime GetLocalDateTimeByUTCTicks(long Ticks)
        {
            if (Ticks > 0)
            {
                try
                {
                    DateTime utcTime = new DateTime(Ticks);
                    TimeZone zone = TimeZone.CurrentTimeZone;
                    DateTime localTime = zone.ToLocalTime(utcTime);
                    if (localTime != null)
                    {
                        return localTime;
                    }
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// 将时间ticks 转换成标准时间日期
        /// </summary>
        /// <param name="dateline"></param>
        /// <returns></returns>
        public static string GetLocalFriendlyDateTimeByUTCTicks(long Ticks)
        {

            if (Ticks > 0)
            {
                try
                {
                    DateTime utcTime = new DateTime(Ticks);
                    TimeZone zone = TimeZone.CurrentTimeZone;
                    DateTime localTime = zone.ToLocalTime(utcTime);
                    if (localTime != null)
                    {
                        return localTime.Year.ToString() + "年" + localTime.Month.ToString() + "月" + localTime.Day.ToString() + "日";
                    }
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }
        /// <summary>
        /// 将时间ticks 转换成当地时间日期
        /// </summary>
        /// <param name="dateline"></param>
        /// <returns></returns>
        public static DateTime GetLocalDateTime(string datetime)
        {
            if (string.IsNullOrEmpty(datetime))
            {
                try
                {
                    DateTime localtime = Convert.ToDateTime(datetime);
                    if (localtime != null)
                    {
                        return localtime;
                    }
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            return DateTime.MinValue;
        }
        /// <summary>
        /// 将时间ticks 转换成日期
        /// </summary>
        /// <param name="dateline"></param>
        /// <returns></returns>
        public static DateTime GetLocalDateTimeFromUTC(string datetime)
        {
            if (string.IsNullOrEmpty(datetime))
            {
                try
                {
                    //IFormatProvider cur = new CultureInfo("zh-CN");
                    //DateTime dtime = DateTime.ParseExact(datetime, "yyyy-MM-dd HH:mm:ss", cur);

                    //Fri Aug 28 00:00:00 +0800 2009
                    DateTime dtime = DateTime.ParseExact(datetime, "ddd MMM dd HH:mm:ss zzz yyyy", new System.Globalization.CultureInfo("en-US"));
                    if (dtime != null)
                    {
                        return dtime;
                    }
                    else
                    {
                        return DateTime.Now;
                    }
                }
                catch
                {
                    return DateTime.Now;
                }
            }
            return DateTime.Now;
        }
    }
}