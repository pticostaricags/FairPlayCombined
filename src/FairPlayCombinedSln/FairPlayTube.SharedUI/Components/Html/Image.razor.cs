using Microsoft.AspNetCore.Components;

namespace FairPlayTube.SharedUI.Components.Html
{
    public partial class Image
    {
        [Parameter]
        [EditorRequired]
        public string? Src { get; set; }
        [Parameter]
        [EditorRequired]
        public int? Width { get; set; }
        [Parameter]
        [EditorRequired]
        public string? Alt { get; set; }
    }
}