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

        public short HairColorId { get; set; }

        public short PreferredHairColorId { get; set; }

        public short EyesColorId { get; set; }

        public short PreferredEyesColorId { get; set; }
        [DeniedValues(0)]
        public short BiologicalGenderId { get; set; }

        public short CurrentDateObjectiveId { get; set; }

        public short ReligionId { get; set; }

        public short PreferredReligionId { get; set; }

        public double CurrentLatitude { get; set; }

        public double CurrentLongitude { get; set; }

        public long ProfilePhotoId { get; set; }

        public short KidStatusId { get; set; }

        public short PreferredKidStatusId { get; set; }

        public short TattooStatusId { get; set; }

        public short PreferredTattooStatusId { get; set; }

        public DateTimeOffset BirthDate { get; set; }
    }
}
