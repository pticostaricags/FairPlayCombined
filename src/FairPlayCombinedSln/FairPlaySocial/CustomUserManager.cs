using Microsoft.AspNetCore.Identity;
using FairPlaySocial.Data;
using Microsoft.Extensions.Options;
using FairPlayCombined.Common;

namespace FairPlaySocial
{
    public class CustomUserManager(IUserStore<ApplicationUser> store,
        IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<ApplicationUser>> logger) : UserManager<ApplicationUser>(
            store: store, optionsAccessor: optionsAccessor,
            passwordHasher: passwordHasher, userValidators: userValidators,
            passwordValidators: passwordValidators,
            keyNormalizer: keyNormalizer, errors: errors,
            services: services, logger: logger)
    {
        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            var createUserResult = await base.CreateAsync(user, password);
            if (createUserResult.Succeeded)
            {
                await AddToRoleAsync(user, Constants.RoleName.User);
            }
            return createUserResult;
        }
    }
}