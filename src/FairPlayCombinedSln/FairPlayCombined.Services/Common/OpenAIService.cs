using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common;
public class OpenAIService(HttpClient httpClient, OpenAIServiceConfiguration openAIServiceConfiguration)
{

    public async Task<GenerateDallE3ResponseModel?> GenerateDallE3ImageAsync(string prompt, CancellationToken cancellationToken)
    {
        var requestUrl = openAIServiceConfiguration.GenerateDall3ImageUrl;
        GenerateDallE3RequestModel requestModel = new()
        {
            model = "dall-e-3",
            prompt = prompt,
            n = 1,
            size = "1024x1024"
        };
        var response = await httpClient.PostAsJsonAsync<GenerateDallE3RequestModel>(requestUrl,
            requestModel, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GenerateDallE3ResponseModel>();
        return result;

    }
}

public class OpenAIServiceConfiguration
{
    public string? GenerateDall3ImageUrl { get; set; }
}


public class GenerateDallE3RequestModel
{
    public string? model { get; set; }
    public string? prompt { get; set; }
    public int n { get; set; }
    public string? size { get; set; }
}



public class GenerateDallE3ResponseModel
{
    public int created { get; set; }
    public GenerateDallE3ResponseDatumModel[]? data { get; set; }
}

public class GenerateDallE3ResponseDatumModel
{
    public string? revised_prompt { get; set; }
    public string? url { get; set; }
}
