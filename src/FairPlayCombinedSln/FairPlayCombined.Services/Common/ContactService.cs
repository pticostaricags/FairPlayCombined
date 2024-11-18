using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.Contact;
using FairPlayCombined.Models.Pagination;
using System.Diagnostics;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FairPlayCombined.Interfaces;

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
        private readonly IUserProviderService? userProviderService;
        public ContactService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
            ILogger<ContactService> logger, IUserProviderService userProviderService
            ) : this(dbContextFactory, logger)
        {
            this.userProviderService = userProviderService;
        }
        public async Task ImportFromExcelFileAsync(Stream stream, CancellationToken cancellationToken)
        {
            using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(stream: stream, isEditable: false);
            if (spreadsheetDocument != null)
            {
                IEnumerable<Sheet> sheets = spreadsheetDocument.WorkbookPart!.Workbook.GetFirstChild<Sheets>()!.Elements<Sheet>();
                string relationshipId = sheets.Single(p => p.Name == "My Active Contacts").Id!.Value!;
                WorksheetPart worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet!.GetFirstChild<SheetData>()!;
                IEnumerable<Row> rows = sheetData.Descendants<Row>();
                Dictionary<int, string> columns = new();
                int pos = 0;
                foreach (var cellInnerText in rows.ElementAt(0).Select(p=>p.InnerText))
                {
                    columns.Add(pos, cellInnerText);
                    pos++;
                }
                await InsertExcelContactsAsync(dbContextFactory, rows, columns, cancellationToken);
            }
        }

        private async Task InsertExcelContactsAsync(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory, IEnumerable<Row> rows, Dictionary<int, string> columns, CancellationToken cancellationToken)
        {
            int pos = 0;
            var userId = userProviderService!.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            foreach (Row row in rows)
            {
                if (pos == 0)
                {
                    pos++;
                    continue;
                }
                Debug.WriteLine(row.InnerText);
                Contact contactEntity = new()
                {
                    OwnerApplicationUserId = userId,
                };
                for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                {
                    var cell = row.Descendants<Cell>().ElementAt(i);
                    if (String.IsNullOrWhiteSpace(cell.InnerText))
                        continue;
                    Debug.WriteLine(cell.InnerText);
                    var columnName = columns[i];
                    switch (columnName)
                    {
                        case "First Name": contactEntity.Name = cell.InnerText; break;
                        case "Last Name": contactEntity.Lastname = cell.InnerText; break;
                        case "LinkedIn": contactEntity.LinkedInProfileUrl = cell.InnerText; break;
                        case "YouTube Url": contactEntity.YouTubeChannelUrl = cell.InnerText; break;
                        case "Email": contactEntity.EmailAddress = cell.InnerText; break;
                        case "Instagram Url": contactEntity.InstagramUrl = cell.InnerText; break;
                        case "Description": contactEntity.Notes = cell.InnerText; break;
                    }
                }
                await dbContext.Contact.AddAsync(contactEntity, cancellationToken);
            }
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
