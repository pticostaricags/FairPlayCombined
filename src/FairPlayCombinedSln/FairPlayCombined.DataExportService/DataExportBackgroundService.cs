using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces.FairPlayTube;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FairPlayCombined.DataExportService;

public class DataExportBackgroundService(ILogger<DataExportBackgroundService> logger,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TimeSpan timeToWait = TimeSpan.FromMinutes(1);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var fairPlayTubeUserDataService = scope.ServiceProvider.GetRequiredService<IFairPlayTubeUserDataService>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var emailFrom = configuration["EmailFrom"]!;
                SendGridClient sendGridClient = scope.ServiceProvider.GetRequiredService<SendGridClient>();
                var dbContextFactory =
                    scope.ServiceProvider.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
                var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
                await dbContext.UserDataExportQueue
                    .Where(p => p.IsCompleted &&
                    DateTimeOffset.UtcNow > p.RowCreationDateTime.AddHours(24))
                    .ExecuteDeleteAsync(stoppingToken);
                if (await dbContext.UserDataExportQueue
                    .AnyAsync(p => !p.IsCompleted, stoppingToken)
                    )
                {
                    var queueQuery = dbContext.UserDataExportQueue
                        .Where(p => !p.IsCompleted)
                        .Include(p => p.ApplicationUser);
                    if (await queueQuery!.AnyAsync(stoppingToken))
                    {
                        foreach (var singleEnqueuedItem in queueQuery)
                        {
                            var fileUrl =
                            await fairPlayTubeUserDataService.GetUserDataAsync(singleEnqueuedItem.ApplicationUserId, stoppingToken);
                            singleEnqueuedItem.IsCompleted = true;
                            singleEnqueuedItem.FileUrl = fileUrl;
                            dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(20));
                            logger.LogInformation("Saving export into the database");
                            await dbContext.SaveChangesAsync(stoppingToken);
                            await BuildAndSendEmailAsync(logger, emailFrom, sendGridClient, singleEnqueuedItem, stoppingToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ErrorMessage}", ex.Message);
            }
            await Task.Delay(timeToWait, stoppingToken);
        }
    }

    private static async Task BuildAndSendEmailAsync(ILogger<DataExportBackgroundService> logger, string emailFrom, SendGridClient sendGridClient, UserDataExportQueue singleEnqueuedItem, CancellationToken stoppingToken)
    {
        SendGridMessage sendGridMessage = new()
        {
            From = new EmailAddress(emailFrom),
            Subject = "FairPlay Data Export",
            HtmlContent = "<p>Your data is ready, you can download it from the here:" +
                                        $"<a href='{singleEnqueuedItem.FileUrl}'>{singleEnqueuedItem.FileUrl}</a>.</p>"
        };

        sendGridMessage.AddTo(singleEnqueuedItem.ApplicationUser.Email);
        var response =
            await sendGridClient.SendEmailAsync(sendGridMessage, stoppingToken);
        if (!response.IsSuccessStatusCode)
        {
            var responseStrong = await response.Body.ReadAsStringAsync(stoppingToken);
            logger.LogError("Response: {ResponseMessage}", responseStrong);
        }
    }
}
