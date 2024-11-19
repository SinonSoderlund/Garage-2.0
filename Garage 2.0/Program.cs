using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Garage_2._0.Data;
using Garage_2._0.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Garage_2._0.Models.Entities;


namespace Garage_2._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Garage_2_0Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Garage_2_0Context") ?? throw new InvalidOperationException("Connection string 'Garage_2_0Context' not found.")));

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Garage_2_0Context>();

            builder.Services.AddScoped<ISpotRepository, SpotRepository>();
            builder.Services.AddScoped<IFeedbackMessageRepository, FeedbackMessageRepository>();
            builder.Services.AddScoped<IPersonNumberRepository, PersonNumberRepository>();
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
