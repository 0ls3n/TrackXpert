using System;

namespace TrackXpert_API.Models.TrackData;

public class Track
{
    public Guid Id { get; set; }
    public TrackMetadata? Metadata { get; set; }
    public TrackAnalytics? Analytics { get; set; }
    public TrackFileInfo? FileInfo { get; set; }
    public TrackProcessingStatus? ProcessingStatus { get; set; }
}
