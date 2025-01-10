using System;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using TrackXpert_ClassLibrary.Models.TrackData;
using TrackXpert_WebApp.Services;

namespace TrackXpert_WebApp.Components.Pages;

public partial class UploadTrack : ComponentBase
{
    [Inject]
    private IConfiguration? config { get; set; }

    [Inject]
    private IUploadService? uploadService { get; set; }

    private TrackMetadata? trackMetadata;
    private string CurrentTag = string.Empty;

    private IBrowserFile? selectedFile;
    private long maxFileSize = 1024 * 1024 * 250; // This means the maximum file size is 250 MB
    private List<string> errors = new();

    private void LoadFiles(InputFileChangeEventArgs e)
    {
        selectedFile = e.File;
    }

    private void HandleFileSelected(InputFileChangeEventArgs e)
    {
        selectedFile = e.File;
        string[] fileName = selectedFile.Name.Split(".");
        trackMetadata = new()
        {
            Title = fileName[0],
            ReleaseDate = DateTime.Now,
            Tags = new List<string>()
        };
    }

    private void AddTag()
    {
        if (CurrentTag != string.Empty)
        {
            trackMetadata!.Tags!.Add(CurrentTag);
            CurrentTag = string.Empty; // Clear the input field
        }
    }

    private void RemoveTag(string tag)
    {
        trackMetadata!.Tags!.Remove(tag);
    }

    private async Task HandleSubmit()
    {
        try
        {
            string result = await uploadService!.UploadTrackFileAsync(selectedFile!, maxFileSize);

            TrackResultModel? trackJsonResult = JsonSerializer.Deserialize<TrackResultModel>(result);

            TrackFileInfo trackFileInfo = new()
            {
                PreviewUrl = trackJsonResult!.FilePath,
                Size = selectedFile!.Size,
                Format = selectedFile.Name.Split(".")[1],
                IsDeleted = false,
                UploadDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            TrackAnalytics trackAnalytics = new()
            {
                FeedbackCount = 0,
                Likes = 0,
                PlaybackCount = 0
            };

            TrackProcessingStatus trackProcessingStatus = new()
            {
                ProcessingStatus = TrackXpert_ClassLibrary.Models.PStatus.Uploaded
            };

            Track track = new Track()
            {
                FileInfo = trackFileInfo,
                Metadata = trackMetadata,
                Analytics = trackAnalytics,
                ProcessingStatus = trackProcessingStatus
            };

            await uploadService.UploadTrackAsync(track);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error: {ex.Message}");
        }
    }
}

public class TrackResultModel
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("filePath")]
    public string FilePath { get; set; }
}

