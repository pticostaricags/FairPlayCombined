using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models
{
    [ModelOfEntity<ICreateModel>("[dbo].[AspNetUsers]")]
    public partial class TestModel
    {
        [CustomRequired]
        [CustomStringLength(maximumLength:0)]
        public string? Id { get; set; }
    }
}
