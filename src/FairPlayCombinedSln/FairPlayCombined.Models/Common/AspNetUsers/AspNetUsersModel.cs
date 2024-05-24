using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.AspNetUsers
{
    public class AspNetUsersModel : IListModel
    {
        public string? Id { get; set; }

        [StringLength(256)]
        public string? UserName { get; set; }
    }
}
