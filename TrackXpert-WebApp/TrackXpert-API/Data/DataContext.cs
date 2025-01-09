using System;
using Microsoft.EntityFrameworkCore;
using TrackXpert_ClassLibrary.Models;
using TrackXpert_ClassLibrary.Models.TrackData;

namespace TrackXpert_API.Data;

public class DataContext : DbContext
{
    public DbSet<Artist>? Artists { get; set; }
    public DbSet<Track>? Tracks { get; set; }

    public DbSet<TrackArtist>? TrackArtists { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TrackArtist>().HasNoKey();

        modelBuilder.Entity<Track>(entity =>
        {
            entity.OwnsOne(t => t.Metadata);

            entity.OwnsOne(t => t.Analytics);
            entity.OwnsOne(t => t.FileInfo);
            entity.OwnsOne(t => t.ProcessingStatus);
        });
    }
}
