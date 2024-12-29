using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace TrackXpert_WebApp.Components.Pages;

public partial class UploadTrack : ComponentBase
{

    HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5133/api/tracks") };

    [Inject]
    private IConfiguration? config { get; set; }
    private long maxFileSize = 1024 * 1024 * 200; // This means the maximum file size is 200 MB
    private int maxAllowedFiles = 3;
    private List<string> errors = new();
    private async Task LoadFiles(InputFileChangeEventArgs e)
    {
        errors.Clear();

        if (e.FileCount > maxAllowedFiles)
        {
            errors.Add($"Error: Attempting to upload {e.FileCount} files, but only {maxAllowedFiles} files are allowed!");
            return;
        }

        var files = e.GetMultipleFiles(maxAllowedFiles);

        using var content = new MultipartFormDataContent();

        foreach (var file in files)
        {
            try
            {
                var fileContent = new StreamContent(file.OpenReadStream(maxFileSize));
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "files", file.Name);
            }
            catch (Exception ex)
            {
                errors.Add($"File: {file.Name} Error: {ex.Message}");
            }
        }

        try
        {
            var response = await client.PostAsync("tracks/upload", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Upload successful: " + result);
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                errors.Add($"API Error: {errorResponse}");
            }
        }
        catch (Exception ex)
        {
            errors.Add($"Error uploading files: {ex.Message}");
        }
    }
}

