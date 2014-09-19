using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Mvc.Honeypot
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Calculates MD5 HASH
        /// </summary>
        /// <param name="PropertyName">Name of property that needs to be hashed</param>
        /// <returns>Hashed value of property name</returns>
        public static string GetHashedPropertyName(string PropertyName)
        {
            //Initializer session
            HttpContext.Current.Session["honeypot"] = true;
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(string.Concat(PropertyName, HttpContext.Current.Session.SessionID));
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sbHashedName = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sbHashedName.Append(hash[i].ToString("X2"));
            }
            return sbHashedName.ToString();
        }

        /// <summary>
        /// Renders out field with honeypot security check enabled
        /// </summary>
        /// <param name="helper">HtmlHelper which will be extended</param>
        /// <param name="name">Name of field. Should match model field of string type</param>
        /// <param name="value">Value of the field</param>
        /// <param name="css">CSS class to be applied to input field</param>
        /// <returns>Returns render out MvcHtmlString for displaying on the View</returns>
        public static MvcHtmlString HoneyPotField(this HtmlHelper helper, string name, object value, string css = null)
        {
            StringBuilder sbControlHtml = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter))
                {
                    HtmlInputText hashedField = new HtmlInputText();
                    string hashedName = GetHashedPropertyName(name);
                    hashedField.Value = value != null ? value.ToString() : string.Empty;
                    hashedField.ID = hashedName;
                    hashedField.Name = hashedName;
                    if (!string.IsNullOrWhiteSpace(css))
                    {
                        hashedField.Attributes["class"] = css;
                    }
                    hashedField.RenderControl(htmlWriter);
                    HtmlInputHidden hiddenField = new HtmlInputHidden();
                    hiddenField.Value = string.Empty;
                    hiddenField.ID = name;
                    hiddenField.Name = name;
                    hiddenField.RenderControl(htmlWriter);
                    sbControlHtml.Append(stringWriter.ToString());
                }
            }
            return new MvcHtmlString(sbControlHtml.ToString());
        }

    }
}
