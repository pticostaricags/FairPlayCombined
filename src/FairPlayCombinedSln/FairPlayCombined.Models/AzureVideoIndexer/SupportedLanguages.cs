using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable IDE1006 // Naming Styles
namespace FairPlayCombined.Models.AzureVideoIndexer
{

    public class SupportedLanguageModel
    {
        public string? name { get; set; }
        public string? languageCode { get; set; }
        public bool isRightToLeft { get; set; }
        public bool isSourceLanguage { get; set; }
        public bool isAutoDetect { get; set; }
        public bool isSupportedForLanguageDataset { get; set; }
        public bool isSupportedForPronunciationDataset { get; set; }
        public bool isSupportedForCustomModels { get; set; }
        public bool isSupportedForTranslation { get; set; }
    }

}
#pragma warning restore IDE1006 // Naming Styles