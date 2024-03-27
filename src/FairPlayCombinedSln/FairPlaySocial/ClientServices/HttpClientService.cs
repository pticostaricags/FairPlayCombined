namespace FairPlaySocial.ClientServices
{
    public class HttpClientService(IHttpClientFactory httpClientFactory)
    {
        public HttpClient CreateAnonymousClient()
        {
            return httpClientFactory.CreateClient(
                $"{nameof(FairPlaySocial)}.ServerAPI.Anonymous");
        }

        public HttpClient CreateAuthorizedClient()
        {
            return httpClientFactory.CreateClient(
                $"{nameof(FairPlaySocial)}.ServerAPI");
        }
    }
}
