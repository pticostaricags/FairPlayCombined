using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models
{
    [ModelOfEntity<TestCreateModel>("[dbo].[AspNetUsers]")]
    public partial class TestCreateModel: ICreateModel
    {
    }

}
