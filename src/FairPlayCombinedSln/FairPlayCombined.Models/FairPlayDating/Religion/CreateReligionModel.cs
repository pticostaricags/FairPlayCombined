﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.Religion
{
    public class CreateReligionModel : ICreateModel
    {
        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
    }
}
