using System.Reflection;

namespace FairPlayCRM.UIConfiguration
{
    public static class AdditionalSetup
    {
        private static readonly Assembly[] additionalAssemblies =
                [
            typeof(FairPlayCRM.SharedUI.Components.Pages.Home).Assembly,
        typeof(FairPlayCombined.SharedAuth._Imports).Assembly];

        internal static Assembly[] AdditionalAssemblies => additionalAssemblies;
    }
}
