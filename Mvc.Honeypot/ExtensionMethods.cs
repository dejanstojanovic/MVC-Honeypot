using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Honeypot
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extension methods which returns bool value whether request is coming from bot or not
        /// </summary>
        /// <param name="request">HttpRequestBase made in a controller action</param>
        /// <returns>Returns whethet honeypot is triggered by bot or not</returns>
        public static bool HasHoneypotTrapped(this HttpRequestBase request)
        {
            if (request.Form.AllKeys.Contains("HasHoneypotTrapped") &&
                !string.IsNullOrWhiteSpace(request.Form["HasHoneypotTrapped"]) && request.Form["HasHoneypotTrapped"].Equals(true.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}
