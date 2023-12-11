using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common
{
    public static class ThemeConfiguration
    {
        public static class Grids
        {
            public static string GridContainerCss { get; set; } = "grid-container";
        }
        public static class Divisions
        {
            public static string DefaultCss { get; set; } = "mb-3";
        }

        public static class Labels
        {
            public static string DefaultCss { get; set; } = "form-label";
            public static string ErrorCss { get; set; } = "alert alert-danger";
        }
        public static class Buttons
        {
            public static string PrimaryButtonCss { get; set; } = "btn btn-primary";
            public static string SecondaryButtonCss { get; set; } = "btn btn-secondary";
        }
        public static class Selects
        {
            public static string DefaultCss { get; set; } = "form-select";
        }

        public static class GenericControls
        {
            public static string DefaultCss { get; set; } = "form-control";
        }

        public static class Icons
        {
            public static string TenantsDefaultCss { get; set; } = "bi bi-people-fill";
            public static string NavigateBack { get; set; } = "bi bi-arrow-left-circle-fill";
        }

        public static class Images
        {
            public static string ThumbnailDefaultCss { get; set; } = "img-thumbnail";
        }
    }
}
