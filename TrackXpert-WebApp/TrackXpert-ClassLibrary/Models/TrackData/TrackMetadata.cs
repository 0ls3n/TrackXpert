using System;
using System.ComponentModel.DataAnnotations;

namespace TrackXpert_ClassLibrary.Models.TrackData;

public class TrackMetadata
{
    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Genre { get; set; }

    public string? Description { get; set; }

    public DateTime ReleaseDate { get; set; }

    public string? Key { get; set; }

    public double Bpm { get; set; }

    public List<string>? Tags { get; set; }

    [Required]
    public bool IsExplicit { get; set; }
}
