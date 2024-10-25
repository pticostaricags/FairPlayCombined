using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using Testcontainers.MsSql;
using static Grpc.Core.Metadata;

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

        protected static async Task<AspNetUsers> CreateFromUserAsync(FairPlayCombinedDbContext dbContext)
        {
            string fromUserName = "fromuser@test.test";
            AspNetUsers fromUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = fromUserName,
                NormalizedUserName = fromUserName.Normalize(),
                Email = fromUserName,
                NormalizedEmail = fromUserName.Normalize(),
                Name = "AT FROM NAME",
                Lastname = "AT FROM LASTNAME"
            };
            var applicationName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name;
            fromUser!.SourceApplication = applicationName;
            fromUser.RowCreationDateTime = DateTimeOffset.UtcNow;
            fromUser.RowCreationUser = fromUser.UserName!;
            fromUser.OriginatorIpaddress = String.Join(",", IpAddressProvider.GetCurrentHostIPv4AddressesAsync().Result);
            await dbContext.AspNetUsers.AddAsync(fromUser);
            await dbContext.SaveChangesAsync();
            return fromUser;
        }

        protected static async Task<AspNetUsers> CreateToUserAsync(FairPlayCombinedDbContext dbContext)
        {
            string toUserName = "toUser@test.test";
            AspNetUsers toUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = toUserName,
                NormalizedUserName = toUserName.Normalize(),
                Email = toUserName,
                NormalizedEmail = toUserName.Normalize(),
                Name = "AT TO NAME",
                Lastname = "AT TO LASTNAME"
            };
            var applicationName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name;
            toUser!.SourceApplication = applicationName;
            toUser.RowCreationDateTime = DateTimeOffset.UtcNow;
            toUser.RowCreationUser = toUser.UserName!;
            toUser.OriginatorIpaddress = String.Join(",", IpAddressProvider.GetCurrentHostIPv4AddressesAsync().Result);
            await dbContext.AspNetUsers.AddAsync(toUser);
            await dbContext.SaveChangesAsync();
            return toUser;
        }
    }
}
#pragma warning restore S1118 // Utility classes should not have public constructors