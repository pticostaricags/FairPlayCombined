using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoPlan;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
        CreateVideoPlanModel,
        UpdateVideoPlanModel,
        VideoPlanModel,
        FairPlayCombinedDbContext,
        VideoPlan,
        PaginationRequest,
        PaginationOfT<VideoPlanModel>
        >]
    public partial class VideoPlanService : BaseService, IVideoPlanService
    {
        public async Task UpdateVideoPlanAsync(UpdateVideoPlanModel createModel,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(UpdateVideoPlanAsync));
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            VideoPlan entity = await dbContext.VideoPlan
                .SingleAsync(p=>p.VideoPlanId == createModel.VideoPlanId,
                cancellationToken:cancellationToken);

            entity.ApplicationUserId = createModel.ApplicationUserId;
            entity.VideoName = createModel.VideoName;
            entity.VideoDescription = createModel.VideoDescription;
            entity.VideoScript = createModel.VideoScript;

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
