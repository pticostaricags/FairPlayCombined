using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FairPlayCombined.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FairPlayTubeController(
    ILogger<FairPlayTubeController> logger,
    IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
    IOpenAIService openAIService) : ControllerBase
{

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateThumbnailAsync(
        [FromBody] CreateThumbnailRequestModel createThumbnailRequestModel,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Executing {MethodName} with {Data}",
            nameof(CreateThumbnailAsync), createThumbnailRequestModel);
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var promptInfo = await dbContext.Prompt
            .SingleAsync(p => p.PromptName == Common.Constants.PromptsNames.CreateYouTubeThumbnail,
            cancellationToken: cancellationToken);
        StringBuilder fullPrompt = new();
        fullPrompt.AppendLine(promptInfo.BaseText);
        fullPrompt.AppendLine("**** USER INPUT ****");
        fullPrompt.AppendLine($"Video Title: {createThumbnailRequestModel.VideoTitle}");
        fullPrompt.AppendLine($"Video Captions: {createThumbnailRequestModel.VideoCaptions}");
        fullPrompt.AppendLine("**** USER INPUT ****");
        var result = await openAIService.GenerateDallE3ImageAsync(fullPrompt.ToString(),
            cancellationToken: cancellationToken);
        return Ok(new
        {
            Image_Url = result!.data![0].url
        });
    }
}

public class CreateThumbnailRequestModel
{
    [Required]
    public string? VideoTitle { get; set; }
    [Required]
    public string? VideoCaptions { get; set; }
}