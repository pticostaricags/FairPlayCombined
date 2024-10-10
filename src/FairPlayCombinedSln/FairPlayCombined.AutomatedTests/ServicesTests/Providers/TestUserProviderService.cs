using FairPlayCombined.Interfaces;

namespace FairPlayCombined.AutomatedTests.ServicesTests.Providers
{
    internal class TestUserProviderService : IUserProviderService
    {
        public static string? CurrentUserId { private get;  set; }
        public string? GetAccessToken()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentUserId()
        {
            return CurrentUserId!;
        }

        public bool IsAuthenticatedWithGoogle()
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticatedWithLinkedIn()
        {
            throw new NotImplementedException();
        }
    }
}
