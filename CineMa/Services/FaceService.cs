using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Azure;


namespace CineMa.Services
{
    public class FaceService
    {
        private readonly string _endpoint;
        private readonly string _key;

        private readonly HttpClient _http;

        public FaceService(IConfiguration config)
        {
            _endpoint = config["AzureFace:Endpoint"];//chaves armazenadas no secrets.json
            _key = config["AzureFace:Key"];
                
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);
        }


        public async Task<string?> DetectFace(byte[] imageBytes)
        {
            var url = $"{_endpoint}/face/v1.1/detect?returnFaceId=true";

            using var content = new ByteArrayContent(imageBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var response = await _http.PostAsync(url, content);
            var json = await response.Content.ReadAsStringAsync();

            var arr = JArray.Parse(json);
            if (arr.Count == 0)
                return null;

            return arr[0]["faceId"]?.ToString();
        }

        public async Task<bool> CompareFaces(string faceId1, string faceId2)
        {
            var url = $"{_endpoint}/face/v1.1/verify";

            var body = new
            {
                faceId1 = faceId1,
                faceId2 = faceId2
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(url, content);

            var result = JObject.Parse(await response.Content.ReadAsStringAsync());

            return result["isIdentical"]?.ToObject<bool>() ?? false;
        }
    }
}
