using FairPlayCombined.Common.FairPlaySocial.Enums;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using Microsoft.EntityFrameworkCore;

namespace FairPlaySocial.TestDataGenerator;

public class TestDataGenerator(ILogger<TestDataGenerator> logger,
    IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
                await dbContext.UserProfile.ExecuteDeleteAsync(stoppingToken);
                await dbContext.Post.ExecuteDeleteAsync(stoppingToken);
                await dbContext.Photo.ExecuteDeleteAsync(stoppingToken);
                await dbContext.AspNetUsers.ExecuteDeleteAsync(stoppingToken);
                await dbContext.SaveChangesAsync(stoppingToken);
                int itemsCount = 500;
                for (int i = 0; i < itemsCount; i++)
                {
                    string postText = Faker.Lorem.Paragraph();
                    if (postText.Length > 500)
                        postText = postText.Substring(0, 500);
                    string email = $"GTEST-{Random.Shared.Next(1000000)}-{Faker.Internet.Email()}";
                    string emailNormalized = email.Normalize();
                    var maxDateOfBirthAllowed = DateTimeOffset.UtcNow.AddYears(-20);
                    var minDateOfBirthDallowed = DateTimeOffset.UtcNow.AddYears(-40);
                    var dateOfBirthTicks =
                    Random.Shared.NextInt64(minDateOfBirthDallowed.Ticks, maxDateOfBirthAllowed.Ticks);
                    logger.LogInformation("Adding item {x} of {y}", i, itemsCount);
                    string testImageFileName = "TestImage.png";
                    string testImageName = "TestImage";
                    await dbContext.AspNetUsers.AddAsync(new AspNetUsers()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = email,
                        NormalizedUserName = emailNormalized,
                        Email = email,
                        NormalizedEmail = emailNormalized,
                        PasswordHash = "TestPassword123",
                        EmailConfirmed = false,
                        LockoutEnabled = true,
                        Post =
                        [
                            new Post()
                            {
                                PostTypeId = (byte)FairPlayCombined.Common.FairPlaySocial.Enums.PostType.Post,
                                PostVisibilityId = (short)FairPlayCombined.Common.FairPlaySocial.Enums.PostVisibility.Public,
                                Text = postText,
                                Photo = new Photo() {
                                    Filename = testImageFileName,
                                    Name = testImageName,
                                    PhotoBytes = Properties.Resources.TestImage
                                }
                            }
                        ]
                    }, stoppingToken);
                    if (i % 10 == 0)
                    {
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
