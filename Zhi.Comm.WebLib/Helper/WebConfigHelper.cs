using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Zhi.Comm.WebLib
{
    public static class WebConfigHelper
    {
        public static string GetConfigValueByName(string name)
        {
            string value = "";
            if (ConfigurationManager.AppSettings[name] != null)
            {
                value = ConfigurationManager.AppSettings[name].ToString(); 
            }
            return value;
        }
    }
}