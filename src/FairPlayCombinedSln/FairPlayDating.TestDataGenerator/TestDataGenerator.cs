using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.Common.GeoNames;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Utilities;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using static Azure.Core.HttpHeader;

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
            await dbContext.Post.ExecuteDeleteAsync(stoppingToken);
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
            HttpClient httpClient = new HttpClient();
            var loggerFactory = 
            LoggerFactory.Create(configure => 
            {
                configure.AddConsole();
            });
            var geoNamesServiceLogger = loggerFactory.CreateLogger<GeoNamesService>();
            GeoNamesService geoNamesService = new(httpClient, geoNamesServiceLogger);
            List<geodata> geoDataCollection = new List<geodata>();
            for (int i = 0; i < 50; i++)
            {
                geodata? randomGeoLocation = null;
                logger.LogInformation("Getting random location {i} of 50",i);
                try
                {
                    randomGeoLocation = await geoNamesService.GeoRandomLocationAsync(CancellationToken.None);
                }
                catch (Exception)
                {
                    randomGeoLocation = new geodata()
                    {
                        nearest = new geodataNearest()
                        {
                            latt = 3.5158M,
                            longt = -74.37231M
                        }
                    };
                }
                geoDataCollection.Add(randomGeoLocation);
            }
            int itemsCount = 5000;
            var geoDataArray = geoDataCollection.ToArray();
            for (int i = 0; i < itemsCount; i++)
            {
                geodata? randomGeoLocation =
                    Random.Shared.GetItems<geodata>(geoDataArray.ToArray(), 1)[0];
                var currentGeoLocation = new NetTopologySuite.Geometries
                        .Point
                        (
                        (double)randomGeoLocation!.nearest!.longt,
                        (double)randomGeoLocation.nearest.latt
                        )
                {
                    SRID = FairPlayCombined.Common.Constants.GeoCoordinates.SRID
                };
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
                var maxDateOfBirthAllowed = DateTimeOffset.UtcNow.AddYears(-20);
                var minDateOfBirthDallowed = DateTimeOffset.UtcNow.AddYears(-40);
                var dateOfBirthTicks =
                Random.Shared.NextInt64(minDateOfBirthDallowed.Ticks, maxDateOfBirthAllowed.Ticks);
                logger.LogInformation("Adding item {x} of {y}", i, itemsCount);
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
                        About = "Photos from https://generated.photos",
                        BirthDate = new DateTime(dateOfBirthTicks, DateTimeKind.Utc),
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
                        ProfilePhoto = photo,
                        CurrentGeoLocation = currentGeoLocation,
                        CurrentLatitude = (double)randomGeoLocation.nearest.latt,
                        CurrentLongitude =(double)randomGeoLocation.nearest.longt
                    }
                }, stoppingToken);
            }
            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
