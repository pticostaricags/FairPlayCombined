using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.Models.FairPlaySocial.Post;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlaySocial
{
    [ServiceOfT<
        CreatePostModel,
        UpdatePostModel,
        PostModel,
        FairPlayCombinedDbContext,
        Post,
        PaginationRequest,
        PaginationOfT<PostModel>
        >]
    public partial class PostService : BaseService
    {
    }
}
