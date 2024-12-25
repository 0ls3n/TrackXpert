using System;
using TrackXpert_API.Models.TrackData;

namespace TrackXpert_API.Models;

public class TrackArtist
{
    public Guid TrackId { get; set; }
    public Track? Track { get; set; }

    public Guid ArtistId { get; set; }
    public Artist? Artist { get; set; }
}
