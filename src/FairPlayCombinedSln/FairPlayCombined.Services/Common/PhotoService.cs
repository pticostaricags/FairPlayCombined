using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.Common.Photo;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreatePhotoModel,
        UpdatePhotoModel,
        PhotoModel,
        FairPlayCombinedDbContext,
        Photo,
        PaginationRequest,
        PaginationOfT<PhotoModel>
        >]
    public partial class PhotoService : BaseService
    {
        public async Task UpdatePhotoAsync(UpdatePhotoModel updatePhotoModel, 
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var photoEntity = await dbContext.Photo
                .SingleAsync(p => p.PhotoId == updatePhotoModel.PhotoId,
                cancellationToken);
            photoEntity.PhotoBytes = updatePhotoModel.PhotoBytes;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
