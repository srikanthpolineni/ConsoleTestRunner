using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestRunner
{
    public class TestCase : IDisposable
    {
        List<MethodInfo> _methods = null;
        private MethodInfo _classCleanup;
        private object _testObject = null;
        public TestCase(object testObject, MethodInfo classCleanup, MethodInfo testInitialize, MethodInfo testCleanUp, MethodInfo testMethod)
        {
            _testObject = testObject;
            _classCleanup = classCleanup;
            _methods = new List<MethodInfo>();
            if (testInitialize != null)
                _methods.Add(testInitialize);
            if (testMethod != null)
                _methods.Add(testMethod);
            if (testCleanUp != null)
                _methods.Add(testCleanUp);
        }

        public string Execute()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var method in _methods)
            {
                var methodName = method.Name;
                try
                {
                    sb.AppendLine(string.Format("{0}    Start", methodName));
                    method.Invoke(_testObject, new object[] { });
                    sb.AppendLine(string.Format("{0}    Sucess", methodName));
                }
                catch (Exception ex)
                {
                    //TODO: Add method end fail
                    sb.AppendLine(string.Format("{0}    Error: {1}", methodName, ex.InnerException.Message));
                }
                finally
                {
                    sb.AppendLine(string.Format("{0}    End", methodName));
                }
                sb.AppendLine();
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void Dispose()
        {
            if (_classCleanup != null)
            {
                _classCleanup.Invoke(_testObject, new object[] { });
            }
        }
    }
}
