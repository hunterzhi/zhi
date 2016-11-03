using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Collections;

namespace Zhi.Comm.WebLib
{
    /// <summary>
    /// CacheHelper 的摘要说明
    /// </summary>
    public class CacheHelper
    {
        public CacheHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region 从配置文件中读取缓存时间
        //缓存时间
        private static string _CacheTime = string.Empty;
        public static string CacheTime
        {
            get
            {
                try
                {
                    _CacheTime = ConfigurationManager.AppSettings["CacheTime"].ToString();
                }
                catch (Exception)
                {
                    _CacheTime = "0";
                }
                return _CacheTime;
            }
            set { _CacheTime = value; }
        }
        #endregion

        #region 插入Cache
        /// <summary> 
        /// 插入Cache 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="o"></param> 
        /// <param name="key"></param> 
        public static void Add<T>(T o, string key)
        {
            HttpRuntime.Cache.Insert(key, 0, null, DateTime.Now.AddMinutes(Convert.ToDouble(CacheTime)), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary> 
        /// 插入Cache 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="o"></param> 
        /// <param name="key"></param> 
        public static void AddNoExpire<T>(T o, string key)
        {
            HttpRuntime.Cache.Insert(key, o, null, DateTime.Now.AddYears(100), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        #endregion

        #region 删除指定的Cache
        /// <summary> 
        /// 删除指定的Cache 
        /// </summary> 
        /// <param name="key">Cache的key</param> 
        public static void Clear(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
        #endregion

        #region 判断Cache是否存在
        /// <summary> 
        /// 判断Cache是否存在 
        /// </summary> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static bool Exists(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }
        #endregion

        #region 取得Cache值，带类型 T
        /// <summary> 
        /// 取得Cache值，带类型 T 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        /// <returns></returns> 
        public static bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T); // 
                    return false;
                }

                value = (T)HttpRuntime.Cache[key];
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }
        #endregion

        #region 清除所有缓存
        /// <summary>
        /// 有时可能需要立即更新,这里就必须手工清除一下Cache 
        /// Cache类有一个Remove方法,但该方法需要提供一个CacheKey,但整个网站的CacheKey我们是无法得知的 
        /// 只能经过遍历 
        /// </summary>
        public static void RemoveAllCache()
        {


            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }

            foreach (string key in al)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }
        #endregion

        #region 显示所有缓存

        //显示所有缓存 
        public static string show()
        {
            string str = "";
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();

            while (CacheEnum.MoveNext())
            {
                str += "<br />缓存名<b>[" + CacheEnum.Key + "]</b><br />";
            }
            return "当前服务总缓存数:" + HttpRuntime.Cache.Count + "<br />" + str;
        }
        #endregion
    }

}