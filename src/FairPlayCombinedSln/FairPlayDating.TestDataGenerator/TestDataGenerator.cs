using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

namespace FairPlayDating.TestDataGenerator;

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
            }
            var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
            await dbContext.UserProfile.ExecuteDeleteAsync(stoppingToken);
            await dbContext.Photo.ExecuteDeleteAsync(stoppingToken);
            await dbContext.AspNetUsers.ExecuteDeleteAsync(stoppingToken);
            await dbContext.SaveChangesAsync(stoppingToken);
            var allGenders = await dbContext.Gender.ToArrayAsync(stoppingToken);
            var allEyesColors = await dbContext.EyesColor.ToArrayAsync(stoppingToken);
            var allDateObjectives = await dbContext.DateObjective.ToArrayAsync(stoppingToken);
            var allHairColor = await dbContext.HairColor.ToArrayAsync(stoppingToken);
            var allKidStatus = await dbContext.KidStatus.ToArrayAsync(stoppingToken);
            var allReligions = await dbContext.Religion.ToArrayAsync(stoppingToken);
            var allTattooStatuses = await dbContext.TattooStatus.ToArrayAsync(stoppingToken);
            var humansPhotosDirectory = Environment.GetEnvironmentVariable("HumansPhotosDirectory");
            string[]? allHumansPhotosPaths = null;
            if (!String.IsNullOrWhiteSpace(humansPhotosDirectory))
            {
                allHumansPhotosPaths = Directory.GetFiles(humansPhotosDirectory, "*.jpg", SearchOption.AllDirectories);
            }
            for (int i = 0; i < 10000; i++)
            {
                Photo photo = new Photo()
                {
                    Filename = "test",
                    Name = "test"
                };
                if (allHumansPhotosPaths?.Length > 0)
                {
                    string randomPath = Random.Shared.GetItems<string>(allHumansPhotosPaths, 1)[0];
                    photo.PhotoBytes = File.ReadAllBytes(randomPath);
                }
                else
                {
                    photo.PhotoBytes = Properties.Resources.EmptyProfilePhoto;
                }
                string email = $"GTEST-{Random.Shared.Next(1000000)}-{Faker.Internet.Email()}";
                string emailNormalized = email.Normalize();
                string strAbout = Faker.Lorem.Sentence();
                if (strAbout.Length > 50)
                    strAbout = strAbout.Substring(0, 50);
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
                    UserProfile = new UserProfile()
                    {
                        BiologicalGenderId = Random.Shared.GetItems<Gender>(allGenders, 1)[0].GenderId,
                        About = strAbout,
                        BirthDate = Faker.Identification.DateOfBirth(),
                        CurrentDateObjectiveId = Random.Shared.GetItems<DateObjective>(allDateObjectives, 1)[0].DateObjectiveId,
                        EyesColorId = Random.Shared.GetItems<EyesColor>(allEyesColors, 1)[0].EyesColorId,
                        HairColorId = Random.Shared.GetItems<HairColor>(allHairColor, 1)[0].HairColorId,
                        KidStatusId = Random.Shared.GetItems<KidStatus>(allKidStatus, 1)[0].KidStatusId,
                        PreferredEyesColorId = Random.Shared.GetItems<EyesColor>(allEyesColors, 1)[0].EyesColorId,
                        PreferredHairColorId = Random.Shared.GetItems<HairColor>(allHairColor, 1)[0].HairColorId,
                        PreferredKidStatusId = Random.Shared.GetItems<KidStatus>(allKidStatus, 1)[0].KidStatusId,
                        PreferredReligionId = Random.Shared.GetItems<Religion>(allReligions, 1)[0].ReligionId,
                        ReligionId = Random.Shared.GetItems<Religion>(allReligions, 1)[0].ReligionId,
                        TattooStatusId = Random.Shared.GetItems<TattooStatus>(allTattooStatuses, 1)[0].TattooStatusId,
                        PreferredTattooStatusId = Random.Shared.GetItems<TattooStatus>(allTattooStatuses, 1)[0].TattooStatusId,
                        ProfilePhoto = photo
                    }
                }, stoppingToken);
            }
            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
