namespace FairPlayCombined.Models.AzureVideoIndexer
{
    public class AzureVideoIndexerServiceConfiguration
    {
        public string? AccountId { get; set; }
        public string? Location { get; set; }
        public bool IsArmAccount { get; set; }
        public string? ResourceGroup { get; set; }
        public string? SubscriptionId { get; set; }
        public string? ResourceName { get; set; }
        public string? ApiVersion { get; set; } = "2024-01-01";
    }
}