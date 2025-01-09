using Microsoft.AspNetCore.Components.Forms;

namespace TrackXpert_WebApp.Services
{
    public class UploadService : IUploadService
    {

        private readonly HttpClient _client;

        public UploadService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("TrackClient");
        }

        public async Task<string> UploadTrackAsync(IBrowserFile file, long maxFileSize)
        {
            string jsonResult = string.Empty;

            using var content = new MultipartFormDataContent();
            using var fileStream = file!.OpenReadStream(maxFileSize);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.Name!);

            try
            {
                var response = await _client.PostAsync("tracks/upload", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    jsonResult = result;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    jsonResult = errorResponse;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IOException: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            return jsonResult;
        }
    }
}
