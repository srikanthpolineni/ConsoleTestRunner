using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestRunner
{
    public abstract class TestAbstractRunner
    {
        public TestAbstractRunner()
        {
            
        }

        public abstract Framework CurrentUnitTestFramework { get; internal set; }

        protected internal abstract IEnumerable<Type> GetUnitTestTypes(Assembly assembly);

        protected internal abstract IEnumerable<MethodInfo> GetTestMethods(Type type);

        protected internal abstract MethodInfo GetClassInitialize(Type type);

        protected internal abstract MethodInfo GetClassCleanup(Type type);

        protected internal abstract MethodInfo GetTestInitialize(Type type);

        protected internal abstract MethodInfo GetTestCleanup(Type type);

        public IEnumerable<TestCase> GetTestCases(string componentPath)
        {
            var unitTestAssembly = Assembly.LoadFrom(componentPath);
            var unitTestTypes = this.GetUnitTestTypes(unitTestAssembly);
            List<TestCase> testCases = new List<TestCase>();
            foreach (var type in unitTestTypes)
            {
                var classInitializeMethod = this.GetClassInitialize(type);
                var classCleanupMethod = this.GetClassCleanup(type);

                var testInitializeMethod = this.GetTestInitialize(type);
                var testCleanupMethod = this.GetTestCleanup(type);

                //TODO: try catch
                var testObject = Activator.CreateInstance(type);

                if (classInitializeMethod != null)
                    classInitializeMethod.Invoke(testObject, new object[] { });


                foreach (var testMethod in GetTestMethods(type))
                {
                    var testCase = new TestCase(testObject, classCleanupMethod, testInitializeMethod, testCleanupMethod, testMethod);
                    testCases.Add(testCase);
                }
            }
            return testCases;
        }
    }
}
