using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Reflection;

namespace Mvc.Honeypot
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class HoneypotField : RequiredAttribute, IMetadataAware
    {
        private bool _IsTrapped;

        public HoneypotField()
        {
            _IsTrapped = false;
        }

        void IMetadataAware.OnMetadataCreated(ModelMetadata metadata)
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {

                if (HttpContext.Current.Request.Form.AllKeys.Contains(Mvc.Honeypot.HtmlHelpers.GetHashedPropertyName(validationContext.DisplayName)))
                {
                    String val = HttpContext.Current.Request.Form[Mvc.Honeypot.HtmlHelpers.GetHashedPropertyName(validationContext.DisplayName)];
                    validationContext.ObjectInstance.GetType().GetProperty(validationContext.DisplayName).SetValue(validationContext.ObjectInstance, val);
                }
                _IsTrapped = false;

                return ValidationResult.Success;
            }
            else
            {
                _IsTrapped = true;
                return ValidationResult.Success;

            }
        }

        public bool IsTrapped
        {
            get
            {
                return _IsTrapped;
            }
        }




    }
}
