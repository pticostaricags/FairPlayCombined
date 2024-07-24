using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.UserFundsUniqueCodes
{
    public class UserFundsUniqueCodesModel
    {
        public int UserFundsUniqueCodesId { get; set; }

        public Guid Code { get; set; }

        public bool IsClaimed { get; set; }
    }
}
