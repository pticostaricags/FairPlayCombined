namespace FairPlayCombined.Models.GoogleGemini
{
#pragma warning disable IDE1006 // Naming Styles
    public class GenerateContentRequestModel
    {
        public Content[]? contents { get; set; }
        public Generationconfig? generationConfig { get; set; }
        public Safetysetting[]? safetySettings { get; set; }

        private const string SAFETY_SETTINGS_CATEGORY_HARASSMENT = "HARM_CATEGORY_HARASSMENT";
        private const string SAFETY_SETTINGS_TRESHOLD_MEDIUM_AND_ABOVE = "BLOCK_MEDIUM_AND_ABOVE";
        private const string SAFETY_SETTINGS_CATEGORY_HATE_SPEECH = "HARM_CATEGORY_HATE_SPEECH";
        private const string SAFET_SETTINGS_CATEGORY_DANGEROUS_CONTENT = "HARM_CATEGORY_DANGEROUS_CONTENT";

        public static readonly GenerateContentRequestModel DefaultGenerateContentRequestModel =
            new()
            {
                generationConfig = new()
                {
                    temperature = 0.9f,
                    topK = 1,
                    topP = 1,
                    maxOutputTokens = 2048
                },
                safetySettings =
                [
                    new()
                    {
                        category = SAFETY_SETTINGS_CATEGORY_HARASSMENT,
                        threshold = SAFETY_SETTINGS_TRESHOLD_MEDIUM_AND_ABOVE
                    },
                    new()
                    {
                        category = SAFETY_SETTINGS_CATEGORY_HATE_SPEECH,
                        threshold = SAFETY_SETTINGS_TRESHOLD_MEDIUM_AND_ABOVE
                    },
                    new()
                    {
                        category = SAFET_SETTINGS_CATEGORY_DANGEROUS_CONTENT,
                        threshold = SAFETY_SETTINGS_TRESHOLD_MEDIUM_AND_ABOVE
                    }
                ]
            };
    }

    public class Generationconfig
    {
        public float temperature { get; set; }
        public int topK { get; set; }
        public int topP { get; set; }
        public int maxOutputTokens { get; set; }
        public object[]? stopSequences { get; set; }
    }

    public class Content
    {
        public string? role { get; set; }
        public Part[]? parts { get; set; }
    }

    public class Part
    {
        public string? text { get; set; }
    }

    public class Safetysetting
    {
        public string? category { get; set; }
        public string? threshold { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
