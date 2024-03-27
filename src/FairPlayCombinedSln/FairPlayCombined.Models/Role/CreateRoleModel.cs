using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Role
{
    public class CreateRoleModel
    {
        [Required]
        [StringLength(maximumLength: 256)]
        public string? Name { get; set; }
    }
}
