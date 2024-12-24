using System;

namespace TrackXpert_API.Models;

public class Artist
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string[]? SocialLinks { get; set; }
    public string? WebsiteUrl { get; set; }
    public Track[]? Tracks { get; set; }
    public string[]? Genres { get; set; }
    public DateTime JoinDate { get; set; }
    public bool IsVerified { get; set; }
    public string? Email { get; set; }
}
