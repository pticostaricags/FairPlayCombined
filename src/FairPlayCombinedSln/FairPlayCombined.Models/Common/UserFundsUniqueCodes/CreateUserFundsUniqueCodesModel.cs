using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.UserFundsUniqueCodes
{
    public class CreateUserFundsUniqueCodesModel : ICreateModel
    {
        public Guid Code { get; set; }
        public bool IsClaimed { get; set; }
        [StringLength(100)]
        public string? OwnerFullName { get; set; }
        [StringLength(100)]
        public string? OwnerEmailAddress { get; set; }
        [StringLength(500)]
        public string? OwnerLinkedProfileUrl { get; set; }
    }
}
