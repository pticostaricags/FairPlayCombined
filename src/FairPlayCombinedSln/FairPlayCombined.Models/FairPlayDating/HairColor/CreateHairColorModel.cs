﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.HairColor
{
    public class CreateHairColorModel : ICreateModel
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
    }
}
