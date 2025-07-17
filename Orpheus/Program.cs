using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orpheus.Core.Interfaces;
using Orpheus.Data;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;
using Orpheus.Data.Repository;
using Orpheus.Core.Implementations;

namespace Orpheus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("OrpheusDbConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<OrpheusDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<OrpheusAppUser>
                (options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<OrpheusDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped<IInstrumentItemService, InstrumentItemService>();

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

            app.Run();
        }
    }
}
