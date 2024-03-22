using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.Common.ConfigurationSecret;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreateConfigurationSecretModel,
        UpdateConfigurationSecretModel,
        ConfigurationSecretModel,
        FairPlayCombinedDbContext,
        ConfigurationSecret,
        PaginationRequest,
        PaginationOfT<ConfigurationSecretModel>
        >]
    public partial class ConfigurationSecretService : BaseService
    {
        public async Task<bool> UpdateAllConfigurationSecretAsync(
            List<ConfigurationSecretModel> configurationSecretModels,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await dbContext.ConfigurationSecret.ExecuteDeleteAsync(cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            foreach (var singleConfigurationSecretModel in configurationSecretModels)
            {
                ConfigurationSecret entity = new()
                {
                    Name = singleConfigurationSecretModel.Name,
                    Value = singleConfigurationSecretModel.Value,

                };
                await dbContext.ConfigurationSecret.AddAsync(entity, cancellationToken);
            }
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
