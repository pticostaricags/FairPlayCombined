namespace FairPlayCombined.Interfaces
{
    public interface IUserValidationService
    {
        Task ValidateUserDataAsync(string name, string lastName, string email,
             string reasonToCreateAccount, CancellationToken cancellationToken);
    }
}
