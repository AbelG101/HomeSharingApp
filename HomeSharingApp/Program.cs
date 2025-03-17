using HomeSharingApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using HomeSharingApp.Areas.Identity.Data;
using Stripe;
using HomeSharingApp.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("HomeSharingAppDb") ?? throw new InvalidOperationException("Connection string 'HomeSharingAppDb' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddSingleton<AppDbContext, AppDbContext>();

builder.Services.AddSingleton<IPropertyListingService, PropertyListingService>();

builder.Services.AddSingleton<IReservationService, ReservationService>();

builder.Services.AddSingleton<IReviewService, HomeSharingApp.Services.ReviewService>();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("/Errors/PageNotFound?statuscode={0}");

app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PropertyListing}/{action=GetPropertyListings}/{id?}");
app.MapRazorPages();

app.Run();