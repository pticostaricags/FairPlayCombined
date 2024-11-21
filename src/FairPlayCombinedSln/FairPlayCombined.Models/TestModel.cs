using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models
{
    [ModelOfEntity<ICreateModel>("[dbo].[AspNetUsers]")]
    public partial class TestModel
    {
        public string? Id { get; set; }
    }
}
