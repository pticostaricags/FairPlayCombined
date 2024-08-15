using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.Contacts;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreateContactModel,
        UpdateContactModel,
        ContactModel,
        FairPlayCombinedDbContext,
        Contact,
        PaginationRequest,
        PaginationOfT<ContactModel>
        >]
    public partial class ContactService : BaseService, IContactService
    {
    }
}
