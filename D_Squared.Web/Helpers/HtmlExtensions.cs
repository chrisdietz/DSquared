using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace D_Squared.Web.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString CustomTextAreaFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes,
            bool disabled
        )
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            if (disabled)
            {
                attributes["disabled"] = "disabled";
            }
            return htmlHelper.TextAreaFor(expression, attributes);
        }

        public static MvcHtmlString CustomDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            Object htmlAttributes,
            bool disabled
            )
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            if (disabled)
            {
                attributes["disabled"] = "disabled";
            }
            return htmlHelper.DropDownListFor(expression, selectList, attributes);
        }

        public static MvcHtmlString CustomCheckBoxFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression,
            Object htmlAttributes,
            bool disabled
            )
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            if (disabled)
            {
                attributes["disabled"] = "disabled";
            }
            return htmlHelper.CheckBoxFor(expression, attributes);
        }
    }
}