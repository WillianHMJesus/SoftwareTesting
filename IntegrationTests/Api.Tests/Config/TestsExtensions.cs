using System.Net.Http;
using System.Net.Http.Headers;

namespace Api.Tests.Config
{
    public static class TestsExtensions
    {
        public static void AssignAccessToken(this HttpClient client, string accessToken)
        {
            client.AssignJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public static void AssignJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
