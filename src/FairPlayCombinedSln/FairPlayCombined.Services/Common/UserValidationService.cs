using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace FairPlayCombined.Services.Common;

public class UserValidationService(
    IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
    ILogger<UserValidationService> logger,
    IOpenAIService openAIService) : IUserValidationService
{
    public async Task ValidateUserDataAsync(string name, string lastName, string email,
         string reasonToCreateAccount, CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "Start of method: {MethodName}", nameof(ValidateUserDataAsync));
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var promptEntity = await dbContext.Prompt
            .AsNoTracking()
            .SingleAsync(p => p.PromptName == Constants.PromptsNames.ValidateUserSentData,
            cancellationToken: cancellationToken);
        StringBuilder systemMessage = new();
        systemMessage.AppendLine(promptEntity.BaseText);
        systemMessage.AppendLine($"Your response must be in the language culture: " +
            $"{CultureInfo.CurrentCulture.Name}");
        StringBuilder userMessage = new();
        userMessage.AppendLine("Properties:");
        userMessage.AppendLine($"Name: {name}");
        userMessage.AppendLine($"Lastname: {lastName}");
        userMessage.AppendLine($"Email: {email}");
        userMessage.AppendLine($"Reason To Create Account: {reasonToCreateAccount}");
        var response = await openAIService.GenerateChatCompletionAsync(systemMessage.ToString(), userMessage.ToString(), cancellationToken);
        var resultText = response!.choices![0].message!.content;
        if (resultText!.Contains("$$ERRORS$$"))
        {
            throw new RuleException(resultText.Replace("$$ERRORS$$", String.Empty));
        }
    }
}
