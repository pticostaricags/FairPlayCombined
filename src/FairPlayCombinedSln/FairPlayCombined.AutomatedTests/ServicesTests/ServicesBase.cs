using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests
{
    [TestClass]
#pragma warning disable S1118 // Utility classes should not have public constructors
    public class ServicesBase
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        protected static MsSqlContainer? _msSqlContainer;
#pragma warning restore CA2211 // Non-constant fields should not be visible

        [AssemblyInitialize()]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void AssemblyInit(TestContext context)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            _msSqlContainer = new MsSqlBuilder().Build();
            _msSqlContainer!.StartAsync().Wait();
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            if (_msSqlContainer!.State == DotNet.Testcontainers.Containers.TestcontainersStates.Running)
            {
                _msSqlContainer!.StopAsync().Wait();
            }
        }
    }
}
#pragma warning restore S1118 // Utility classes should not have public constructors