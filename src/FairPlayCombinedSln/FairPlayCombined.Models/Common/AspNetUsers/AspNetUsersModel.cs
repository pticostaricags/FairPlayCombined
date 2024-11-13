using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.AspNetUsers
{
    public class AspNetUsersModel : IListModel
    {
        public string? Id { get; set; }

        [StringLength(256)]
        public string? UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(50)]
        public string? Lastname { get; set; }

        public string? FullName => $"{Name} {Lastname}";
    }
}
