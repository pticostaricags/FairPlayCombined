namespace FairPlayCombined.Models
{
    public class UserModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
