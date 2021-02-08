using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerManagementSystem.Library
{
    public class Common
    {
        /// <summary>
        /// Gets the base url of the project
        /// </summary>
        /// <returns>the base url defined</returns>
        public static string GetBaseUrl()
        {
            string baseUrl = string.Empty;
            string[] urlArray = HttpContext.Current.Request.Url.AbsoluteUri.Split(new string[] { "//" }, StringSplitOptions.None);

            if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("localhost:"))
            {
                baseUrl = urlArray[0] + "//" + urlArray[1].Split(new string[] { "/" }, StringSplitOptions.None)[0] + "/";
            }
            else if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("localhost"))
            {
                baseUrl = urlArray[0] + "//" + urlArray[1].Split(new string[] { "/" }, StringSplitOptions.None)[0] + "/" + urlArray[1].Split(new string[] { "/" }, StringSplitOptions.None)[1] + "/";
            }
            else
            {
                baseUrl = urlArray[0] + "//" + urlArray[1].Split(new string[] { "/" }, StringSplitOptions.None)[0] + "/";
            }

            return baseUrl;
        }
    }
}