using System;
using System.ComponentModel.DataAnnotations;

namespace TrackXpert_ClassLibrary.Models.TrackData;

public class TrackMetadata
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Key { get; set; }
    public double Bpm { get; set; }
    public string[]? Tags { get; set; }
    public bool IsExplicit { get; set; }
}
