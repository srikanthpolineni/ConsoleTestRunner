using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestRunner
{
    public static class ExtensionMethods
    {

        public static IEnumerable<MethodInfo> GetMethodByAttribute(this Type type, string attributeType)
        {
            List<MethodInfo> result = new List<MethodInfo>();
            var allMethods = type.GetMethods().Where(m => m.IsPublic && !m.IsAbstract);
            foreach (var method in allMethods)
            {
                var expectedttributes = method.GetCustomAttributes().Where(a => a.GetType().Name == attributeType);
                if (expectedttributes != null && expectedttributes.Count() > 0)
                    result.Add(method);
            }
            return result;
        }
    }
}
