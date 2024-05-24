using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.AspNetUsers
{
    public class CreateAspNetUsersModel : ICreateModel
    {

        [StringLength(256)]
        public string? UserName { get; set; }
    }
}
