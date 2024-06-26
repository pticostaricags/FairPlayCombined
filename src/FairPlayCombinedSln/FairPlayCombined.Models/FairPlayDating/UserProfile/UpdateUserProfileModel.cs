﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.UserProfile
{
    public class UpdateUserProfileModel : IUpdateModel
    {
        [Required]
        public long UserProfileId { get; set; }

        [Required]
        public string? ApplicationUserId { get; set; }

        [Required]
        [StringLength(100)]
        public string? About { get; set; }

        public int HairColorId { get; set; }

        public int PreferredHairColorId { get; set; }

        public int EyesColorId { get; set; }

        public int PreferredEyesColorId { get; set; }

        public int BiologicalGenderId { get; set; }

        public int CurrentDateObjectiveId { get; set; }

        public int ReligionId { get; set; }

        public int PreferredReligionId { get; set; }

        public double CurrentLatitude { get; set; }

        public double CurrentLongitude { get; set; }

        public long ProfilePhotoId { get; set; }

        public int KidStatusId { get; set; }

        public int PreferredKidStatusId { get; set; }

        public int TattooStatusId { get; set; }

        public int PreferredTattooStatusId { get; set; }

        public DateTimeOffset BirthDate { get; set; }
        [Required]
        public NetTopologySuite.Geometries.Point? CurrentGeoLocation { get; set; }
    }
}
