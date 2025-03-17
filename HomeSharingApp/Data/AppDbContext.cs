using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HomeSharingApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HomeSharingApp.Areas.Identity.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
  protected readonly IConfiguration Configuration;

  public AppDbContext(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder options)
  {
    // connect to postgres with connection string from app settings
    options.UseNpgsql(Configuration.GetConnectionString("HomeSharingAppDb"));
  }

  public DbSet<PropertyListing>? PropertyListings { get; set; }
  public DbSet<AppUser> Users { get; set; }
  public DbSet<Reservation> Reservations { get; set; }
  public DbSet<HomeSharingApp.Models.Review>? Reviews { get; set; }
}