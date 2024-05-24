namespace FairPlayCombined.Models.AzureContentSafety
{
#pragma warning disable IDE1006 // Naming Styles
    public class PromptShieldResponseModel
    {
        public Userpromptanalysis? userPromptAnalysis { get; set; }
        public Documentsanalysis[]? documentsAnalysis { get; set; }
    }

    public class Userpromptanalysis
    {
        public bool attackDetected { get; set; }
    }

    public class Documentsanalysis
    {
        public bool attackDetected { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
