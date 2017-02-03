using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestRunner
{
    public class XUnitTestRunner : TestAbstractRunner
    {
        public XUnitTestRunner()
        {
            CurrentUnitTestFramework = Framework.xunit;
        }
        public override Framework CurrentUnitTestFramework { get; internal set; }


        protected internal override IEnumerable<Type> GetUnitTestTypes(Assembly assembly)
        {
            List<Type> testTypes = new List<Type>();
            var allTypes = assembly.GetTypes().Where(t => t.IsPublic && !t.IsAbstract);
            foreach (var type in allTypes)
            {
                testTypes.Add(type);
            }
            return testTypes;
        }

        protected internal override IEnumerable<MethodInfo> GetTestMethods(Type type)
        {
            return type.GetMethodByAttribute("FactAttribute");
        }

        protected internal override MethodInfo GetClassInitialize(Type type)
        {
            return null;
        }

        protected internal override MethodInfo GetClassCleanup(Type type)
        {
            return null;
        }

        protected internal override MethodInfo GetTestInitialize(Type type)
        {
            return null;
        }

        protected internal override MethodInfo GetTestCleanup(Type type)
        {
            return null;
        }
    }
}
