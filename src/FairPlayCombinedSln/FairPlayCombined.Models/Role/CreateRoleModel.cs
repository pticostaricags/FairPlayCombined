using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Role
{
    public class CreateRoleModel
    {
        [Required]
        [StringLength(maximumLength:256)]
        public string? Name { get; set; }
    }
}
