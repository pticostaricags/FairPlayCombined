﻿using FairPlayCombined.Interfaces;

namespace FairPlaySocial.TestDataGenerator.Providers
{
    public class UserProviderService : IUserProviderService
    {
        internal static string? UserId { get; set; }

        public string? GetAccessToken()
        {
            throw new NotImplementedException();
        }

        public string? GetCurrentUserId()
        {
            return UserId;
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
