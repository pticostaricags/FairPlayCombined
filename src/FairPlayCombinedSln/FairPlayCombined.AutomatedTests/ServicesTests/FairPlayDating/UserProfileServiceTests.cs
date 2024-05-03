using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayDating.UserProfile;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class UserProfileServiceTests : ServicesBase
    {

        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<UserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleUserProfile in dbContext.UserProfile)
            {
                dbContext.UserProfile.Remove(singleUserProfile);
            }

            foreach (var singleActivity in dbContext.Activity)
            {
                dbContext.Activity.Remove(singleActivity);
            }
            foreach (var singleGender in dbContext.Gender)
            {
                dbContext.Gender.Remove(singleGender);
            }
            foreach (var singleDateObjective in dbContext.DateObjective)
            {
                dbContext.DateObjective.Remove(singleDateObjective);
            }
            foreach (var singleEyeColor in dbContext.EyesColor)
            {
                dbContext.EyesColor.Remove(singleEyeColor);
            }
            foreach (var singleHairColor in dbContext.HairColor)
            {
                dbContext.HairColor.Remove(singleHairColor);
            }
            foreach (var singleKidStatus in dbContext.KidStatus)
            {
                dbContext.KidStatus.Remove(singleKidStatus);
            }
            foreach (var singleReligion in dbContext.Religion)
            {
                dbContext.Religion.Remove(singleReligion);
            }
            foreach (var tattooStatus in dbContext.TattooStatus)
            {
                dbContext.TattooStatus.Remove(tattooStatus);
            }
            foreach (var singleUser in dbContext.AspNetUsers)
            {
                dbContext.AspNetUsers.Remove(singleUser);
            }
            foreach (var singlePhoto in dbContext.Photo)
            {
                dbContext.Photo.Remove(singlePhoto);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateUserProfileAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<UserProfileService>();
            services.AddTransient<ILogger<UserProfileService>>(p =>
            {
                return logger;
            });
            services.AddTransient<UserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, _,
                _, Gender gender,
                DateObjective dateObjective, EyesColor eyesColor,
                HairColor hairColor, KidStatus kidStatus, Religion religion,
                TattooStatus tattooStatus, Photo photo) =
                await CreateTestRecordsAsync(dbContext);
            var UserProfileService = sp.GetRequiredService<UserProfileService>();
            CreateUserProfileModel createUserProfileModel = new()
            {
                ApplicationUserId = fromUser.Id,
                ProfilePhotoId = photo.PhotoId,
                About = "TEST ABOUT",
                BiologicalGenderId = gender.GenderId,
                BirthDate = DateTime.UtcNow,
                CurrentDateObjectiveId = dateObjective.DateObjectiveId,
                EyesColorId = eyesColor.EyesColorId,
                HairColorId = hairColor.HairColorId,
                KidStatusId = kidStatus.KidStatusId,
                PreferredEyesColorId = eyesColor.EyesColorId,
                PreferredHairColorId = hairColor.HairColorId,
                PreferredKidStatusId = kidStatus.KidStatusId,
                PreferredReligionId = religion.ReligionId,
                PreferredTattooStatusId = tattooStatus.TattooStatusId,
                ReligionId = religion.ReligionId,
                TattooStatusId = tattooStatus.TattooStatusId,
                CurrentGeoLocation = new NetTopologySuite.Geometries
                        .Point(-74.37231, 3.5158)
                {
                    SRID = FairPlayCombined.Common.Constants.GeoCoordinates.SRID
                }
            };
            await UserProfileService.CreateUserProfileAsync(createUserProfileModel, CancellationToken.None);
            var result = await dbContext.UserProfile.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createUserProfileModel.ApplicationUserId, result.ApplicationUserId);
        }

        private static void RegisterDbContext(ServiceCollection services)
        {
            var cs = _msSqlContainer!.GetConnectionString();
            Extensions.EnhanceConnectionString(nameof(FairPlayCombined.AutomatedTests), ref cs);
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.AddInterceptors(
                        new SaveChangesInterceptor(new TestUserProviderService())
                        );
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            services.AddTransient<IUserProviderService, TestUserProviderService>();
        }

        private static async Task<(AspNetUsers fromUser, AspNetUsers toUser, Activity activity,
            Gender gender, DateObjective dateObjective,
            EyesColor eyesColor, HairColor hairColor, KidStatus kidStatus,
            Religion religion, TattooStatus tattooStatus,
            Photo photo)>
            CreateTestRecordsAsync(FairPlayCombinedDbContext dbContext)
        {
            string fromUserName = "fromuser@test.test";
            string toUserName = "toUser@test.test";
            AspNetUsers fromUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = fromUserName,
                NormalizedUserName = fromUserName.Normalize(),
                Email = fromUserName,
                NormalizedEmail = fromUserName.Normalize()
            };
            AspNetUsers toUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = toUserName,
                NormalizedUserName = toUserName.Normalize(),
                Email = toUserName,
                NormalizedEmail = toUserName.Normalize()
            };
            await dbContext.AspNetUsers.AddRangeAsync(fromUser);
            await dbContext.AspNetUsers.AddRangeAsync(toUser);
            Activity activity = new()
            {
                Name = "TEST ACTIVITY"
            };
            await dbContext.Activity.AddAsync(activity);
            Gender gender = new()
            {
                Name = "TEST GENDER"
            };
            await dbContext.Gender.AddAsync(gender);
            DateObjective dateObjective = new()
            {
                Name = "TEST OBJECTIVE"
            };
            await dbContext.DateObjective.AddAsync(dateObjective);
            EyesColor eyesColor = new()
            {
                Name = "TEST EYES COLOR"
            };
            await dbContext.EyesColor.AddAsync(eyesColor);
            HairColor hairColor = new()
            {
                Name = "TEST HAIR COLOR"
            };
            await dbContext.HairColor.AddAsync(hairColor);
            KidStatus kidStatus = new()
            {
                Name = "TEST STATUS"
            };
            await dbContext.KidStatus.AddAsync(kidStatus);
            Religion religion = new()
            {
                Name = "TEST RELIGION",
            };
            await dbContext.Religion.AddAsync(religion);
            TattooStatus tattooStatus = new()
            {
                Name = "TEST STATUS"
            };
            await dbContext.TattooStatus.AddAsync(tattooStatus);
            Photo photo = new()
            {
                Name = nameof(Properties.Resources.TestProduct),
                Filename = $"{Properties.Resources.TestProduct}.bmp",
                PhotoBytes = Properties.Resources.TestProduct,
            };
            await dbContext.Photo.AddAsync(photo);
            await dbContext.SaveChangesAsync();
            return (fromUser, toUser, activity, gender, dateObjective, eyesColor, hairColor,
                kidStatus, religion, tattooStatus, photo);
        }

        [TestMethod]
        public async Task Test_DeleteUserProfileAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<UserProfileService>();
            services.AddTransient<ILogger<UserProfileService>>(p =>
            {
                return logger;
            });
            services.AddTransient<UserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, _,
                            _, Gender gender,
                            DateObjective dateObjective, EyesColor eyesColor,
                            HairColor hairColor, KidStatus kidStatus, Religion religion,
                            TattooStatus tattooStatus, Photo photo) =
                            await CreateTestRecordsAsync(dbContext);
            var UserProfileService = sp.GetRequiredService<UserProfileService>();
            UserProfile entity = new()
            {
                ApplicationUserId = fromUser.Id,
                ProfilePhotoId = photo.PhotoId,
                About = "TEST ABOUT",
                BiologicalGenderId = gender.GenderId,
                BirthDate = DateTime.UtcNow,
                CurrentDateObjectiveId = dateObjective.DateObjectiveId,
                EyesColorId = eyesColor.EyesColorId,
                HairColorId = hairColor.HairColorId,
                KidStatusId = kidStatus.KidStatusId,
                PreferredEyesColorId = eyesColor.EyesColorId,
                PreferredHairColorId = hairColor.HairColorId,
                PreferredKidStatusId = kidStatus.KidStatusId,
                PreferredReligionId = religion.ReligionId,
                PreferredTattooStatusId = tattooStatus.TattooStatusId,
                ReligionId = religion.ReligionId,
                TattooStatusId = tattooStatus.TattooStatusId,
                CurrentGeoLocation = new NetTopologySuite.Geometries
                        .Point(-74.37231, 3.5158)
                {
                    SRID = FairPlayCombined.Common.Constants.GeoCoordinates.SRID
                }
            };
            await dbContext.UserProfile.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.UserProfileId);
            await UserProfileService.DeleteUserProfileByIdAsync(entity.UserProfileId, CancellationToken.None);
            var itemsCount = await dbContext.UserProfile.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedUserProfileAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<UserProfileService>();
            services.AddTransient<ILogger<UserProfileService>>(p =>
            {
                return logger;
            });
            services.AddTransient<UserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, _,
                            _, Gender gender,
                            DateObjective dateObjective, EyesColor eyesColor,
                            HairColor hairColor, KidStatus kidStatus, Religion religion,
                            TattooStatus tattooStatus, Photo photo) =
                            await CreateTestRecordsAsync(dbContext);
            var UserProfileService = sp.GetRequiredService<UserProfileService>();
            UserProfile entity = new()
            {
                ApplicationUserId = fromUser.Id,
                ProfilePhotoId = photo.PhotoId,
                About = "TEST ABOUT",
                BiologicalGenderId = gender.GenderId,
                BirthDate = DateTime.UtcNow,
                CurrentDateObjectiveId = dateObjective.DateObjectiveId,
                EyesColorId = eyesColor.EyesColorId,
                HairColorId = hairColor.HairColorId,
                KidStatusId = kidStatus.KidStatusId,
                PreferredEyesColorId = eyesColor.EyesColorId,
                PreferredHairColorId = hairColor.HairColorId,
                PreferredKidStatusId = kidStatus.KidStatusId,
                PreferredReligionId = religion.ReligionId,
                PreferredTattooStatusId = tattooStatus.TattooStatusId,
                ReligionId = religion.ReligionId,
                TattooStatusId = tattooStatus.TattooStatusId,
                CurrentGeoLocation = new NetTopologySuite.Geometries
                        .Point(-74.37231, 3.5158)
                {
                    SRID = FairPlayCombined.Common.Constants.GeoCoordinates.SRID
                }
            };
            await dbContext.UserProfile.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.UserProfileId);
            var result = await UserProfileService.GetPaginatedUserProfileAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new()
                        {
                            PropertyName = nameof(UserProfileModel.UserProfileId),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].UserProfileId, entity.UserProfileId);
        }

        [TestMethod]
        public async Task Test_GetUserProfileByIdAsync()
        {
            ServiceCollection services = new();
            RegisterDbContext(services);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<UserProfileService>();
            services.AddTransient<ILogger<UserProfileService>>(p =>
            {
                return logger;
            });
            services.AddTransient<UserProfileService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            (AspNetUsers fromUser, _,
                            _, Gender gender,
                            DateObjective dateObjective, EyesColor eyesColor,
                            HairColor hairColor, KidStatus kidStatus, Religion religion,
                            TattooStatus tattooStatus, Photo photo) =
                            await CreateTestRecordsAsync(dbContext);
            var UserProfileService = sp.GetRequiredService<UserProfileService>();
            UserProfile entity = new()
            {
                ApplicationUserId = fromUser.Id,
                ProfilePhotoId = photo.PhotoId,
                About = "TEST ABOUT",
                BiologicalGenderId = gender.GenderId,
                BirthDate = DateTime.UtcNow,
                CurrentDateObjectiveId = dateObjective.DateObjectiveId,
                EyesColorId = eyesColor.EyesColorId,
                HairColorId = hairColor.HairColorId,
                KidStatusId = kidStatus.KidStatusId,
                PreferredEyesColorId = eyesColor.EyesColorId,
                PreferredHairColorId = hairColor.HairColorId,
                PreferredKidStatusId = kidStatus.KidStatusId,
                PreferredReligionId = religion.ReligionId,
                PreferredTattooStatusId = tattooStatus.TattooStatusId,
                ReligionId = religion.ReligionId,
                TattooStatusId = tattooStatus.TattooStatusId,
                CurrentGeoLocation = new NetTopologySuite.Geometries
                        .Point(-74.37231, 3.5158)
                {
                    SRID = FairPlayCombined.Common.Constants.GeoCoordinates.SRID
                }
            };
            await dbContext.UserProfile.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.UserProfileId);
            var result = await UserProfileService.GetUserProfileByIdAsync(entity.UserProfileId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.UserProfileId, result.UserProfileId);
        }
    }
}
