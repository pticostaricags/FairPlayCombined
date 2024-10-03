using FairPlayCombined.Interfaces;

namespace FairPlayCombined.AutomatedTests.ServicesTests.Providers
{
    internal class TestUserProviderService : IUserProviderService
    {
        public string? GetAccessToken()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentUserId()
        {
            return "AT User";
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
