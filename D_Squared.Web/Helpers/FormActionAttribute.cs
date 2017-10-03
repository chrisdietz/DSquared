using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Helpers
{
    public class FormActionAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[Prefix + methodInfo.Name] != null && !controllerContext.IsChildAction;
        }
        public string Prefix = "";
    }
}