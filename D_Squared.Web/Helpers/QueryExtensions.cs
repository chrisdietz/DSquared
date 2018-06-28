using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Helpers
{
    public static class QueryExtensions
    {
        public static List<SelectListItem> ToSelectList<T>(this IList<T> query, string dataValueField, string dataTextField, object selectedValue, bool addDefault = false, string defaultOption = "", string defaultValue = "")
        {
            var items = new SelectList(query, dataValueField, dataTextField, selectedValue ?? -1).ToList();
            if (addDefault)
            {
                items.Insert(0, new SelectListItem()
                {
                    Text = defaultOption,
                    Value = defaultValue
                });
            }

            return items;
        }

        public static SelectList ToSelectList(this IList<string> query, object selectedValue, bool addDefault = false, string defaultValue = "")
        {
            if (addDefault)
            {
                query.Insert(0, defaultValue);
            }

            return new SelectList(query, selectedValue ?? -1);
        }
    }
}