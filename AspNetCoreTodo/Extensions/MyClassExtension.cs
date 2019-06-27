using AspNetCoreTodo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MyClassExtension
    {
        public static bool IsNullOrEmpty(this string mystring)
        {
            return string.IsNullOrEmpty(mystring);
        }
    }
}
