using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
public partial class UserProfile
{
    //Check https://docs.microsoft.com/en-us/ef/core/modeling/spatial
    [Required]
    public NetTopologySuite.Geometries.Point? CurrentGeoLocation { get; set; }
}

