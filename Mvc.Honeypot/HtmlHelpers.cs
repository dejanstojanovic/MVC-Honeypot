using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        #region Enumerations

        public enum InputType
        {
            Text,
            Email,
            Tel,
            Search,
            Hidden
        }

        #endregion

        #region Methods

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

        #endregion

        /// <summary>
        /// Renders out field with honeypot security check enabled
        /// </summary>
        /// <param name="helper">HtmlHelper which will be extended</param>
        /// <param name="name">Name of field. Should match model field of string type</param>
        /// <param name="value">Value of the field</param>
        /// <param name="css">CSS class to be applied to input field</param>
        /// <returns>Returns render out MvcHtmlString for displaying on the View</returns>
        public static MvcHtmlString HoneyPotField(this HtmlHelper helper, string name, object value, string inputCss = null, InputType fieldType = InputType.Text, string honeypotCss = null, InputType honeypotType = InputType.Hidden)
        {
            return HoneyPotField(helper, name, value, new { @class = inputCss }, fieldType, honeypotCss, honeypotType);
        }

        /// <summary>
        /// Renders out field with honeypot security check enabled
        /// </summary>
        /// <param name="helper">HtmlHelper which will be extended</param>
        /// <param name="name">Name of field. Should match model field of string type</param>
        /// <param name="value">Value of the field</param>
        /// <param name="htmlAttributes">Attributes to add to the HTML tag</param>
        /// <param name="fieldType">The field type</param>
        /// <param name="honeypotCss">The CSS class applied to the honeypot HTML tag</param>
        /// <param name="honeypotType">The type of input of the honeypot</param>
        /// <returns>Returns render out MvcHtmlString for displaying on the View</returns>
        public static MvcHtmlString HoneyPotField(this HtmlHelper helper, string name, object value, dynamic htmlAttributes = null, InputType fieldType=InputType.Text,string honeypotCss = null, InputType honeypotType=InputType.Hidden)
        {
            StringBuilder sbControlHtml = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter))
                {
                    HtmlInputText hashedField = new HtmlInputText(fieldType.ToString().ToLower());
                    string hashedName = GetHashedPropertyName(name);
                    hashedField.Value = value != null ? value.ToString() : string.Empty;
                    hashedField.ID = hashedName;
                    hashedField.Name = hashedName;
                    if (htmlAttributes != null)
                    {
                        foreach (var property in htmlAttributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            var attributeName = property.Name.Replace('_','-');
                            var attributeValue = property.GetValue(htmlAttributes, null);
                            hashedField.Attributes[attributeName] = attributeValue;
                        }
                    }
                    hashedField.RenderControl(htmlWriter);


                    HtmlInputText hiddenField = new HtmlInputText(honeypotType.ToString().ToLower());
                    hiddenField.Value = string.Empty;
                    hiddenField.ID = name;
                    hiddenField.Name = name;
                    if (!string.IsNullOrWhiteSpace(honeypotCss))
                    {
                        hiddenField.Attributes["class"] = honeypotCss;
                    }
                    hiddenField.RenderControl(htmlWriter);
                    sbControlHtml.Append(stringWriter.ToString());
                }
            }
            return new MvcHtmlString(sbControlHtml.ToString());
        }

        public static MvcHtmlString HoneyPotTextArea(this HtmlHelper helper, string name, object value, int cols, int rows, string inputCss = null, string honeypotCss = null)
        {
            return HoneyPotTextArea(helper, name, value, cols, rows, new { @class = inputCss }, honeypotCss);
        }

        public static MvcHtmlString HoneyPotTextArea(this HtmlHelper helper, string name, object value, int cols, int rows, dynamic htmlAttributes, string honeypotCss = null)
        {
            StringBuilder sbControlHtml = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter))
                {
                    HtmlTextArea hashedField = new HtmlTextArea();
                    string hashedName = GetHashedPropertyName(name);
                    hashedField.Value = value != null ? value.ToString() : string.Empty;
                    hashedField.ID = hashedName;
                    hashedField.Name = hashedName;
                    hashedField.Cols = cols;
                    hashedField.Rows = rows;
                    if (htmlAttributes != null)
                    {
                        foreach (var property in htmlAttributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            var attributeName = property.Name.Replace('_', '-');
                            var attributeValue = property.GetValue(htmlAttributes, null);
                            hashedField.Attributes[attributeName] = attributeValue;
                        }
                    }
                    hashedField.RenderControl(htmlWriter);


                    HtmlTextArea hiddenField = new HtmlTextArea();
                    hiddenField.Value = string.Empty;
                    hiddenField.ID = name;
                    hiddenField.Name = name;
                    hiddenField.Rows = rows;
                    hiddenField.Cols = cols;
                    if (!string.IsNullOrWhiteSpace(honeypotCss))
                    {
                        hiddenField.Attributes["class"] = honeypotCss;
                    }
                    hiddenField.RenderControl(htmlWriter);
                    sbControlHtml.Append(stringWriter.ToString());
                }
            }
            return new MvcHtmlString(sbControlHtml.ToString());
        }

    }
}
