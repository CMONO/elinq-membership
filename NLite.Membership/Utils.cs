using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace NLite.Membership
{
   static class Utils
    {
       public static readonly DateTime MinDate = new DateTime(1900, 1, 1);
       public static T Get<T>(this NameValueCollection nvc, string name, T defaultValue)
       {
           var v = nvc[name];
           if (v == null)
               return defaultValue;
           return Converter.Convert<string,T>(v);
       }
    }
}
