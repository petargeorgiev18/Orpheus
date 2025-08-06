using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orpheus.Core.Interfaces;
using Orpheus.Data;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;
using Orpheus.Data.Repository;
using Orpheus.Core.Implementations;
using Orpheus.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Orpheus
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<OrpheusDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<OrpheusAppUser>
                (options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<OrpheusDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
            });


            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped<IInstrumentService, InstrumentService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IAccessoryService, AccessoryService>();
            builder.Services.AddScoped<IMerchService, MerchService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                var dbContext = services.GetRequiredService<OrpheusDbContext>();

                try
                {
                    await dbContext.Database.EnsureDeletedAsync();
                    logger.LogInformation("Database deleted successfully");

                    await dbContext.Database.EnsureCreatedAsync();
                    logger.LogInformation("Database created successfully");

                    await IdentitySeedData.InitializeAsync(services);
                    await ItemSeedData.InitializeAsync(services);
                    logger.LogInformation("Seeding completed successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during database initialization");
                }
            }

            app.Run();
        }
    }
}