using System;

namespace TrackXpert_API.Models;

public class Track
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public Artist? Artist { get; set; }
    public DateTime ReleaseData { get; set; }
    public int Duration { get; set; } // in seconds
    public string? Format { get; set; }
    public long Size { get; set; } // in bytes
    public DateTime UploadDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string? UploadedBy { get; set; }
    public int FeedbackCount { get; set; }
    public int Likes { get; set; }
    public int PlaybackCount { get; set; }
    public bool Visibility { get; set; }
    public string? Genre { get; set; }
    public double Bpm { get; set; }
    public string? Key { get; set; }
    public string[]? Tags { get; set; }
    public string? Description { get; set; }
    public string? PreviewUrl { get; set; }
    public string? WaveformData { get; set; } // base64 string
    public bool IsDeleted { get; set; }
    public PStatus ProcessingStatus { get; set; }
    public bool IsExplicit { get; set; }

}
