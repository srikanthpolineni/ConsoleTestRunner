using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestRunner
{
    public class MSTestRunner : TestAbstractRunner
    {
        public MSTestRunner() : base()
        {
            CurrentUnitTestFramework = Framework.mstest;
        }

        public override Framework CurrentUnitTestFramework { get; internal set; }

        protected internal override IEnumerable<Type> GetUnitTestTypes(Assembly assembly)
        {
            List<Type> testTypes = new List<Type>();
            var allTypes = assembly.GetTypes().Where(t => t.IsPublic && !t.IsAbstract);
            foreach (var type in allTypes)
            {
                var testAttributes = type.GetCustomAttributes().Where(a => a.GetType().Name == "TestClassAttribute");
                if (testAttributes != null && testAttributes.Count() > 0)
                {
                    testTypes.Add(type);
                }
            }
            return testTypes;
        }

        protected internal override IEnumerable<MethodInfo> GetTestMethods(Type type)
        {
            return type.GetMethodByAttribute("TestMethodAttribute");
        }

        protected internal override MethodInfo GetClassInitialize(Type type)
        {
            return type.GetMethodByAttribute("ClassInitializeAttribute").FirstOrDefault();
        }

        protected internal override MethodInfo GetClassCleanup(Type type)
        {
            return type.GetMethodByAttribute("ClassCleanupAttribute").FirstOrDefault();
        }

        protected internal override MethodInfo GetTestInitialize(Type type)
        {
            return type.GetMethodByAttribute("TestInitializeAttribute").FirstOrDefault();
        }

        protected internal override MethodInfo GetTestCleanup(Type type)
        {
            return type.GetMethodByAttribute("TestCleanupAttribute").FirstOrDefault();
        }
    }
}
