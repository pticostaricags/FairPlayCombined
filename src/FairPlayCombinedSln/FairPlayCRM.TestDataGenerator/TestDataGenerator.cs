using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using Microsoft.EntityFrameworkCore;

namespace FairPlayCRM.TestDataGenerator;

public class TestDataGenerator : BackgroundService
{
    private readonly ILogger<TestDataGenerator> _logger;
    private readonly IServiceProvider _serviceProvider;
    public TestDataGenerator(ILogger<TestDataGenerator> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            }
            var scope = _serviceProvider.CreateScope();
            var dbContextFactory = scope.ServiceProvider
                .GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
            foreach (var user in dbContext.AspNetUsers)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Contact entity = new()
                    {
                        BirthDate = Faker.Identification.DateOfBirth(),
                        BusinessPhoneNumber = Faker.Phone.Number(),
                        EmailAddress = Faker.Internet.Email(),
                        InstagramUrl = Faker.Internet.Url(),
                        LinkedInProfileUrl = Faker.Internet.Url(),
                        Lastname = Faker.Name.Last(),
                        Name = Faker.Name.First(),
                        MobilePhoneNumber = Faker.Phone.Number(),
                        Notes = "TEST DATA",
                        XformerlyTwitterUrl = Faker.Internet.Url(),
                        YouTubeChannelUrl = Faker.Internet.Url(),
                        OwnerApplicationUserId = user.Id
                    };
                    await dbContext.Contact.AddAsync(entity, stoppingToken);
                }
                await dbContext.SaveChangesAsync();
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
