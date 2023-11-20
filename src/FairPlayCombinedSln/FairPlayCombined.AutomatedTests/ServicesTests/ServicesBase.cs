using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests
{
    [TestClass]
    public class ServicesBase
    {
        protected static MsSqlContainer _msSqlContainer;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            _msSqlContainer = new MsSqlBuilder().Build();
            _msSqlContainer.StartAsync().Wait();
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            if (_msSqlContainer.State == DotNet.Testcontainers.Containers.TestcontainersStates.Running)
            {
                _msSqlContainer.StopAsync().Wait();
            }
        }
    }
}
