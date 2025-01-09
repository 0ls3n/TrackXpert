using Microsoft.AspNetCore.Components.Forms;

namespace TrackXpert_WebApp.Services
{
    public interface IUploadService
    {
        public Task<string> UploadTrackAsync(IBrowserFile file, long maxFileSize);
    }
}
