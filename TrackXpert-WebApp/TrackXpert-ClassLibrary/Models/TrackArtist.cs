using System;
using TrackXpert_ClassLibrary.Models.TrackData;

namespace TrackXpert_ClassLibrary.Models;

public class TrackArtist
{
    public Guid TrackId { get; set; }
    public Track? Track { get; set; }

    public Guid ArtistId { get; set; }
    public Artist? Artist { get; set; }
}
