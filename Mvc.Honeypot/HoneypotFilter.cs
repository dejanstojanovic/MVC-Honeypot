using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Honeypot
{
    public class HoneypotFilter : ActionFilterAttribute
    {
        #region Fields
        private string[] honeypots;
        private bool isTrapped;
        #endregion

        #region Properties
        /// <summary>
        /// Attribute property indicating whethet honeypot is triggered
        /// </summary>
        public bool IsTrapped
        {
            get
            {
                return this.isTrapped;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// List of model property names to be validated for honeypot
        /// </summary>
        /// <param name="honeypots">String array of model properties to be honeypot validated</param>
        public HoneypotFilter(params string[] honeypots)
        {
            this.honeypots = honeypots;

        }

        #endregion

        #region Methods
        /// <summary>
        /// Set IsTrapped propery and HttpRequest form field
        /// </summary>
        /// <param name="value">Trap triggered or not</param>
        private void SetIsTrapped(bool value)
        {
            this.isTrapped = value;
            if (value)
            {
                var collection = HttpContext.Current.Request.Form;
                var propInfo = collection.GetType().GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                propInfo.SetValue(collection, false, new object[] { });
                collection.Add("HasHoneypotTrapped", true.ToString());
            }
        }
        #endregion

        #region Attribute handler methods
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetIsTrapped(false);

            var collection = HttpContext.Current.Request.Form;
            foreach (string field in honeypots)
            {
                if (!string.IsNullOrWhiteSpace(collection[field]))
                {
                    SetIsTrapped(true);
                }
                {
                    string hashedName = Mvc.Honeypot.HtmlHelpers.GetHashedPropertyName(field);
                    if (collection.AllKeys.Contains(hashedName))
                    {
                        string val = HttpContext.Current.Request.Form[hashedName];
                        foreach (var actionValue in filterContext.ActionParameters)
                        {
                            foreach (var prop in actionValue.Value.GetType().GetProperties())
                            {
                                if (prop.Name == field && prop.CanWrite && prop.PropertyType == typeof(string))
                                {
                                    if (prop.PropertyType == val.GetType())
                                    {
                                        prop.SetValue(actionValue.Value, val);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        #endregion
    }
}
