using Microsoft.AspNetCore.Components.Forms;
using TrackXpert_ClassLibrary.Models.TrackData;

namespace TrackXpert_WebApp.Services
{
    public interface IUploadService
    {
        public Task<string> UploadTrackFileAsync(IBrowserFile file, long maxFileSize);
        public Task UploadTrackAsync(Track track);

	}
}
