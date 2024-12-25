using System;
using Microsoft.EntityFrameworkCore;
using TrackXpert_API.Models;
using TrackXpert_API.Models.TrackData;

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
            entity.OwnsOne(t => t.Metadata, metadata =>
            {
                metadata.Property(m => m.Tags).HasConversion(
                    v => string.Join(',', v), // Convert array to string for storage
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)); // Convert back to array
            });

            entity.OwnsOne(t => t.Analytics);
            entity.OwnsOne(t => t.FileInfo);
            entity.OwnsOne(t => t.ProcessingStatus);
        });
    }
}
