using System;
using Microsoft.EntityFrameworkCore;
using TrackXpert_API.Models;

namespace TrackXpert_API.Data;

public class DataContext : DbContext
{
    public DbSet<Artist>? Artists {get; set;}
    public DbSet<Track>? Tracks {get; set;}
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
}
