using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.Common.Photo
{
    public class PhotoModel : IListModel
    {
        public long PhotoId { get; set; }
        public string? Name { get; set; }
        public string? Filename { get; set; }
        public byte[]? PhotoBytes { get; set; }
    }
}
