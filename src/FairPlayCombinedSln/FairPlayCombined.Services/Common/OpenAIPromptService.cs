using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.OpenAIPrompt;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreateOpenAIPromptModel,
        UpdateOpenAIPromptModel,
        OpenAIPromptModel,
        FairPlayCombinedDbContext,
        OpenAiprompt,
        PaginationRequest,
        PaginationOfT<OpenAIPromptModel>
        >]
    public partial class OpenAIPromptService : BaseService
    {
    }
}
