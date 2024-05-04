﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.AspNetUsers
{
    public class CreateAspNetUsersModel : ICreateModel
    {

        [StringLength(256)]
        public string? UserName { get; set; }
    }
}
