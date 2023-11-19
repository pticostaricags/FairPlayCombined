using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.DataAccess.Interceptors
{
    public class SaveChangesInterceptor : ISaveChangesInterceptor
    {
        public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        {
            var changedEntities = eventData!.Context!.ChangeTracker.Entries();
            foreach (var entityEntry in changedEntities)
            {
                if (entityEntry.Entity is ApplicationUserVouch)
                {
                    var entity = entityEntry.Entity as ApplicationUserVouch;
                    if (entity != null)
                    {
                        var connectionString = eventData.Context.Database.GetConnectionString();
                        SqlConnectionStringBuilder sqlConnectionStringBuilder = new(connectionString);
                        var applicationName = sqlConnectionStringBuilder.ApplicationName ?? "Unknown App";
                        var userName = applicationName;
                        entity.SourceApplication = nameof(applicationName);
                        entity.RowCreationDateTime = DateTime.UtcNow;
                        entity.RowCreationUser = nameof(userName);
                        entity.OriginatorIpaddress = "127.0.0.1";
                    }
                }
            }
            return result;
        }

        public InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            return result;
        }
    }
}
