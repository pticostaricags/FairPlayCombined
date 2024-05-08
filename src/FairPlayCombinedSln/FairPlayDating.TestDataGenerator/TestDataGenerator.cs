using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.Common.GeoNames;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace FairPlayDating.TestDataGenerator;
#pragma warning disable S6678 // Use PascalCase for named placeholders
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
            var (allGenders, allEyesColors, allDateObjectives, allHairColor, allKidStatus, allReligions,
                allTattooStatuses) =
            await GetAllEntitiesListsAsync(dbContext, stoppingToken);
            var (allMalesPhotosPaths, allFemalesPhotosPaths) = PreparaHumansPhotosPaths();
            HttpClient httpClient = new();
            var loggerFactory =
            LoggerFactory.Create(configure =>
            {
                configure.AddConsole();
            });
            var geoNamesServiceLogger = loggerFactory.CreateLogger<GeoNamesService>();
            GeoNamesService geoNamesService = new(httpClient, geoNamesServiceLogger);
            List<geodata> geoDataCollection = [];
            await PreparerandomLocations(logger, geoNamesService, geoDataCollection);
            int itemsCount = 5000;
            var geoDataArray = geoDataCollection.ToArray();
            for (int i = 0; i < itemsCount; i++)
            {
                SetGeoLocation(geoDataArray, out geodata randomGeoLocation, out Point currentGeoLocation);
                Photo? photo = null;
                if ( i % 2 == 0)
                {
                    photo = InitilizePhoto(allMalesPhotosPaths);
                    await AddUserAsync(logger, dbContext, allEyesColors, allDateObjectives, allHairColor, allKidStatus, allReligions, allTattooStatuses, itemsCount, i, randomGeoLocation, currentGeoLocation, photo, stoppingToken,
                        biologicalGenderId:1);
                }
                else
                {
                    photo = InitilizePhoto(allFemalesPhotosPaths);
                    await AddUserAsync(logger, dbContext, allEyesColors, allDateObjectives, allHairColor, allKidStatus, allReligions, allTattooStatuses, itemsCount, i, randomGeoLocation, currentGeoLocation, photo, stoppingToken,
                        biologicalGenderId:2);
                }
            }
            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }

#pragma warning disable S107 // Methods should not have too many parameters
    private static async Task AddUserAsync(ILogger<TestDataGenerator> logger, FairPlayCombinedDbContext dbContext, EyesColor[] allEyesColors, DateObjective[] allDateObjectives, HairColor[] allHairColor, KidStatus[] allKidStatus, Religion[] allReligions, TattooStatus[] allTattooStatuses, int itemsCount, int i, geodata randomGeoLocation, Point currentGeoLocation, Photo photo, CancellationToken stoppingToken,
        int biologicalGenderId)
#pragma warning restore S107 // Methods should not have too many parameters
    {
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
                BiologicalGenderId = biologicalGenderId,
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
                CurrentLatitude = (double)randomGeoLocation.nearest!.latt,
                CurrentLongitude = (double)randomGeoLocation.nearest.longt
            }
        }, stoppingToken);
    }

    private static Photo InitilizePhoto(string[]? allHumansPhotosPaths)
    {
        Photo photo = new()
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

        return photo;
    }

    private static void SetGeoLocation(geodata[] geoDataArray, out geodata randomGeoLocation, out Point currentGeoLocation)
    {
        randomGeoLocation = Random.Shared.GetItems<geodata>(geoDataArray.ToArray(), 1)[0];
        currentGeoLocation = new NetTopologySuite.Geometries
                                .Point
                                (
                                (double)randomGeoLocation!.nearest!.longt,
                                (double)randomGeoLocation.nearest.latt
                                )
        {
            SRID = FairPlayCombined.Common.Constants.GeoCoordinates.SRID
        };
    }

    private static (string[]? males, string[]? females) PreparaHumansPhotosPaths()
    {
        var humansPhotosDirectory = Environment.GetEnvironmentVariable("HumansPhotosDirectory");
        string[]? allMalesPhotosPaths = null;
        string[]? allFemalesPhotosPaths = null;
        if (!String.IsNullOrWhiteSpace(humansPhotosDirectory))
        {
            allMalesPhotosPaths = Directory.GetFiles(Path.Combine(humansPhotosDirectory,"Male"),"*.jpg", SearchOption.AllDirectories);
            allFemalesPhotosPaths = Directory.GetFiles(Path.Combine(humansPhotosDirectory, "Female"), "*.jpg", SearchOption.AllDirectories);
        }

        return (allMalesPhotosPaths, allFemalesPhotosPaths);
    }

    private static async Task PreparerandomLocations(ILogger<TestDataGenerator> logger, GeoNamesService geoNamesService, List<geodata> geoDataCollection)
    {
        for (int i = 0; i < 50; i++)
        {
            logger.LogInformation("Getting random location {i} of 50", i);
            geodata? randomGeoLocation;
            try
            {
                randomGeoLocation = await geoNamesService.GeoRandomLocationAsync(CancellationToken.None);
                if (randomGeoLocation is null)
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
            geoDataCollection.Add(randomGeoLocation!);
        }
    }

    private static async Task<(
        Gender[] allGenders, EyesColor[] allEyesColors, DateObjective[] allDateObjectives,
        HairColor[] allHairColor, KidStatus[] allKidStatus, Religion[] allReligions,
        TattooStatus[] allTattooStatuses)> GetAllEntitiesListsAsync(FairPlayCombinedDbContext dbContext,
        CancellationToken stoppingToken)
    {
        var allGenders = await dbContext.Gender.ToArrayAsync(stoppingToken);
        var allEyesColors = await dbContext.EyesColor.ToArrayAsync(stoppingToken);
        var allDateObjectives = await dbContext.DateObjective.ToArrayAsync(stoppingToken);
        var allHairColor = await dbContext.HairColor.ToArrayAsync(stoppingToken);
        var allKidStatus = await dbContext.KidStatus.ToArrayAsync(stoppingToken);
        var allReligions = await dbContext.Religion.ToArrayAsync(stoppingToken);
        var allTattooStatuses = await dbContext.TattooStatus.ToArrayAsync(stoppingToken);
        return (allGenders, allEyesColors, allDateObjectives,
        allHairColor, allKidStatus, allReligions, allTattooStatuses);
    }
}
#pragma warning restore S6678 // Use PascalCase for named placeholders