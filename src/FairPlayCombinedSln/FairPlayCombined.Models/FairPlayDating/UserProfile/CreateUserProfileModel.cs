using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.UserProfile
{
    public class CreateUserProfileModel : ICreateModel
    {
        [Required]
        public string? ApplicationUserId { get; set; }

        [Required]
        [StringLength(100)]
        public string? About { get; set; }
        [DeniedValues(0)]
        public int HairColorId { get; set; }
        [DeniedValues(0)]
        public int PreferredHairColorId { get; set; }
        [DeniedValues(0)]
        public int EyesColorId { get; set; }
        [DeniedValues(0)]
        public int PreferredEyesColorId { get; set; }
        [DeniedValues(0)]
        public int BiologicalGenderId { get; set; }
        [DeniedValues(0)]
        public int CurrentDateObjectiveId { get; set; }
        [DeniedValues(0)]
        public int ReligionId { get; set; }
        [DeniedValues(0)]
        public int PreferredReligionId { get; set; }
        [DeniedValues(default(double))]
        public double CurrentLatitude { get; set; }
        [DeniedValues(default(double))]
        public double CurrentLongitude { get; set; }
        [DeniedValues(default(long))]
        public long ProfilePhotoId { get; set; }
        [DeniedValues(0)]
        public int KidStatusId { get; set; }
        [DeniedValues(0)]
        public int PreferredKidStatusId { get; set; }
        [DeniedValues(0)]
        public int TattooStatusId { get; set; }
        [DeniedValues(0)]
        public int PreferredTattooStatusId { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        [Required]
        public NetTopologySuite.Geometries.Point? CurrentGeoLocation { get; set; }
    }
}
