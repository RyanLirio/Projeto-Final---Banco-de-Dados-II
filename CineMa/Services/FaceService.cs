using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace CineMa.Services
{
    public class FaceService
    {
        private readonly string _endpoint;
        private readonly string _key;
        private readonly HttpClient _http;

        public FaceService(IConfiguration config)
        {
            _endpoint = config["AzureFace:Endpoint"];
            _key = config["AzureFace:Key"];

            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);

            Console.WriteLine(">>> FaceService carregado");
            Console.WriteLine("Endpoint: " + _endpoint);
        }

        // Extrai embedding da imagem (API moderna)
        public async Task<byte[]?> ExtractFeatures(byte[] imageBytes)
        {
            Console.WriteLine(">>> CHAMOU O MÉTODO EXTRACTFEATURES");

            var url = $"{_endpoint}/faceembedding?api-version=2023-02-01-preview";

            Console.WriteLine("URL usada: " + url);
            Console.WriteLine("Chave usada: " + _key);

            using var content = new ByteArrayContent(imageBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var response = await _http.PostAsync(url, content);

            Console.WriteLine("Status HTTP: " + response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Resposta Azure: " + json);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var obj = JObject.Parse(json);

            if (obj["faceEmbedding"] == null)
                return null;

            var vector = obj["faceEmbedding"].Select(v => (float)v).ToArray();
            return vector.SelectMany(BitConverter.GetBytes).ToArray();
        }

        // Compara embeddings
        public async Task<bool> CompareEmbeddings(byte[] emb1, byte[] emb2)
        {
            var url = $"{_endpoint}/facematching?api-version=2023-02-01-preview";

            var body = new
            {
                faceEmbedding1 = emb1,
                faceEmbedding2 = emb2
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(url, content);

            var result = JObject.Parse(await response.Content.ReadAsStringAsync());

            return result["areFacesIdentical"]?.ToObject<bool>() ?? false;
        }
    }
}
