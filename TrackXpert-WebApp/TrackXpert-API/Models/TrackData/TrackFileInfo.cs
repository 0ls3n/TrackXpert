using System;

namespace TrackXpert_API.Models.TrackData;

public class TrackFileInfo
{
    public string? Format { get; set; }
    public long Size { get; set; }
    public string? PreviewUrl { get; set; } // The url to the file itself
    public string? WaveformData { get; set; } // Base64
    public DateTime UploadDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string? UploadedBy { get; set; }
    public bool IsDeleted { get; set; }
}
