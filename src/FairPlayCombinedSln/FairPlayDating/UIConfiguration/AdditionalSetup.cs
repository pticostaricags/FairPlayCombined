﻿using System.Reflection;

namespace FairPlayDating.UIConfiguration
{
    public static class AdditionalSetup
    {
        private static readonly Assembly[] additionalAssemblies =
                [
            typeof(FairPlayCombined.SharedAuth._Imports).Assembly];

        internal static Assembly[] AdditionalAssemblies => additionalAssemblies;
    }
}