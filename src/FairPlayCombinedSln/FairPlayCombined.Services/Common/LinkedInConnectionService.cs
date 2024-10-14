using CsvHelper.Configuration;
using CsvHelper;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using FairPlayCombined.DataAccess.Models.dboSchema;
using Microsoft.Extensions.Logging;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.Models.Common.LinkedInConnection;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreateLinkedInConnectionModel,
        UpdateLinkedInConnectionModel,
        LinkedInConnectionModel,
        FairPlayCombinedDbContext,
        LinkedInConnection,
        PaginationRequest,
        PaginationOfT<LinkedInConnectionModel>
        >]
    public partial class LinkedInConnectionService : BaseService, ILinkedInConnectionService
    {
        private readonly IUserProviderService? userProviderService = null;
        public LinkedInConnectionService(ILogger<LinkedInConnectionService> logger,
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService):this(dbContextFactory, logger)
        {
            this.userProviderService = userProviderService;
        }
        public async Task ImportFromConnectionsFileAsync(Stream stream, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            using StreamReader streamReader = new(stream);
            using CsvParser csvParser = new(streamReader, configuration:
                    new CsvConfiguration(CultureInfo.CurrentCulture)
                    {
                        Delimiter = ",",
                        ShouldQuote = ((ShouldQuoteArgs args) => { return false; })
                    });
            using CsvReader csvReader = new(csvParser);
            logger.LogInformation("Start of reading file");
            if (await csvReader.ReadAsync())
            {
                bool firstNameHeaderFound = false;
                while (await csvReader.ReadAsync())
                {
                    logger.LogInformation("Read a new row");
                    if (!firstNameHeaderFound)
                    {
                        firstNameHeaderFound = IsFirstNameHeader(csvReader);
                    }
                    else
                    {
                        logger.LogInformation("Reading main columns text");
                        await SaveRecordToDatabaseAsync(dbContext, csvReader, cancellationToken);
                    }
                }
            }
        }

        private async Task SaveRecordToDatabaseAsync(FairPlayCombinedDbContext dbContext,
            CsvReader csvReader, CancellationToken cancellationToken)
        {
            var firstName = csvReader.GetField(0);
            var lastName = csvReader.GetField(1);
            var profileUrl = csvReader.GetField(2);
            var emailAddress = csvReader.GetField(3);
            var company = csvReader.GetField(4);
            var position = csvReader.GetField(5);
            var connectedOn = csvReader.GetField(6);

            LinkedInConnection linkedInConnectionEntity = new()
            {
                ApplicationUserId = userProviderService!.GetCurrentUserId()
            };
            if (!String.IsNullOrWhiteSpace(firstName))
            {
                linkedInConnectionEntity.FirstName = firstName;
            }

            if (!String.IsNullOrWhiteSpace(lastName))
            {
                linkedInConnectionEntity.LastName = lastName;
            }

            if (!String.IsNullOrWhiteSpace(profileUrl))
            {
                linkedInConnectionEntity.ProfileUrl = profileUrl;
            }

            if (!String.IsNullOrWhiteSpace(emailAddress))
            {
                linkedInConnectionEntity.EmailAddress = emailAddress;
            }

            if (!String.IsNullOrWhiteSpace(company))
            {
                linkedInConnectionEntity.Company = company;
            }

            if (!String.IsNullOrWhiteSpace(position))
            {
                linkedInConnectionEntity.Position = position;
            }
            if (!String.IsNullOrWhiteSpace(connectedOn))
            {
                var connectedOnDate = DateOnly.ParseExact(connectedOn, "dd MMM yyyy",
                    CultureInfo.InvariantCulture);
                linkedInConnectionEntity.ConnectedOn = connectedOnDate;
            }
            if (!String.IsNullOrWhiteSpace(linkedInConnectionEntity.FirstName))
            {
                logger.LogInformation("Adding record");
                await dbContext.LinkedInConnection.AddAsync(linkedInConnectionEntity,
                    cancellationToken);
                logger.LogInformation("Saving record to database");
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private bool IsFirstNameHeader(CsvReader csvReader)
        {
            bool couldRead = csvReader.TryGetField(0, out string? firstColumnText);
            logger.LogInformation("Checking First Column Value: {Value}", firstColumnText);
            if (!couldRead)
            {
                return false;
            }
            if (firstColumnText == "First Name")
            {
                logger.LogInformation("First Name Header found");
                return true;
            }

            return false;
        }
    }
}