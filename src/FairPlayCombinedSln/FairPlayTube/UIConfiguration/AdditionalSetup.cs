using System.Reflection;

namespace FairPlayTube.UIConfiguration
{
    public static class AdditionalSetup
    {
        private static readonly Assembly[] additionalAssemblies =
                [typeof(FairPlayTube.SharedUI.Components.Pages.Home).Assembly];

        internal static Assembly[] AdditionalAssemblies => additionalAssemblies;
    }
}
