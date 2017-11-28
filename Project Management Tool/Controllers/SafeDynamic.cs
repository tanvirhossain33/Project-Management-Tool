using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Project_Management_Tool.Controllers
{
    public static class SafeDynamic
    {
        public static dynamic ToSafeDynamic(this object obj)
        {
            //would be nice to restrict to anonymous types - but alas no.
            IDictionary<string, object> toReturn = new ExpandoObject();

            foreach (var prop in obj.GetType().GetProperties(
              BindingFlags.Public | BindingFlags.Instance)
              .Where(p => p.CanRead))
            {
                toReturn[prop.Name] = prop.GetValue(obj, null);
            }

            return toReturn;
        }
    }
}