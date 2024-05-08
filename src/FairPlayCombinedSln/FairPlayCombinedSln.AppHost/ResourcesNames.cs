using Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombinedSln.AppHost
{
    public static class ResourcesNames
    {
        public static string FairPlayTube => nameof(Projects.FairPlayTube).ToLower();
        public static string FairPlayAdminPortal => nameof(Projects.FairPlayAdminPortal).ToLower();
        public static string FairPlayDating => nameof(Projects.FairPlayDating).ToLower();
        public static string FairPlayDatingTestDataGenerator => "fairplaydatingtestdatagenerator";
        public static string FairPlayTubeVideoIndexing => "fairplaytubevideoindexing";
        public static string FairPlayShop => nameof(Projects.FairPlayShop).ToLower();
        public static string CitiesImporter => "citiesimporter";
        public static string FairPlaySocial => nameof(Projects.FairPlaySocial).ToLower();
        public static string FairPlaySocialTestDataGenerator => "fairplaysocialtestdatagenerator";
        public static string FairPlayCombinedLocalizationGenerator => "fairplaycombinedlocalizationgenerator";
        public static string FairPlayBudget => nameof(Projects.FairPlayBudget).ToLower();
    }
}
