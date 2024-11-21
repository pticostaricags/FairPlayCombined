using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models
{
    [ModelOfEntity<ICreateModel>("[dbo].[AspNetUsers]")]
    public partial class TestModel
    {
        public string? Id { get; set; }
    }
}
