using System.Text.Json.Serialization;

namespace FairPlayCombined.Models.LinkedInApi
{

    public class RegisterUploadResponseModel
    {
        public Value? value { get; set; }
    }

    public class Value
    {
        public string? mediaArtifact { get; set; }
        public Uploadmechanism? uploadMechanism { get; set; }
        public string? asset { get; set; }
        public string? assetRealTimeTopic { get; set; }
    }

    public class Uploadmechanism
    {
        [JsonPropertyName("com.linkedin.digitalmedia.uploading.MediaUploadHttpRequest")]
        public ComLinkedinDigitalmediaUploadingMediauploadhttprequest? comlinkedindigitalmediauploadingMediaUploadHttpRequest { get; set; }
    }

    public class ComLinkedinDigitalmediaUploadingMediauploadhttprequest
    {
        public string? uploadUrl { get; set; }
        public Headers? headers { get; set; }
    }

    public class Headers
    {
        [JsonPropertyName("media-type-family")]
        public string? mediatypefamily { get; set; }
    }
}
