﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.UserProfile
{
    public class UserProfileModel : IListModel
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
        public int Age { get; set; }
        public string? BiologicalGenderText { get; set; }
        public string? CurrentDateObjectiveText { get; set; }
        public string? EyesColorText { get; set; }
        public string? HairColorText { get; set; }
        public string? KidStatusText { get; set; }
        public string? PreferredEyesColorText { get; set; }
        public string? PreferredHairColorText { get; set; }
        public string? PreferredKidStatusText { get; set; }
        public string? PreferredReligionText { get; set; }
        public string? PreferredTattooStatusText { get; set; }
        public string? ReligionText { get; set; }
        public string? TattooStatusText { get; set; }
    }
}
