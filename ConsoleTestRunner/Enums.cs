namespace ConsoleTestRunner
{
    public enum Framework
    {
        mstest,
        xunit,
        nunit
    }

    public enum TestAttributes
    {
        classInitialize,
        testInitialize,
        testMethod,
        testCleanUp,
        classCleanUp
    }
}
