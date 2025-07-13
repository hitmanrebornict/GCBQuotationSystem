using FMS.Components.Services;
using GCBQuotationSystem.Areas.Identity.Data;
using GCBQuotationSystem.Components;
using GCBQuotationSystem.Components.Services;
using GCBQuotationSystem.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;


namespace GCBQuotationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			//builder.Services.AddDbContext<QuotationSystemContext>(options => { }
			//	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

			builder.Services.AddDbContext<GCBQuotationIdentityDbContext>(options =>
			   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

			builder.Services.AddDbContext<QuotationSystemContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			}, ServiceLifetime.Transient);

			builder.Services.AddDefaultIdentity<IdentityUser>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false; // Disable email confirmation
				options.User.RequireUniqueEmail = false;        // No unique email required
			})
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<GCBQuotationIdentityDbContext>();

			// Add services to the container.
			builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
			builder.Services.AddRadzenComponents();
			builder.Services.AddScoped<MasterDataServices>();
			builder.Services.AddScoped<GenerateQuoteServices>();
			builder.Services.AddScoped<QuoteDataServices>();
			builder.Services.AddScoped<QuoteDetailServices>();
			builder.Services.AddScoped<UserPreferenceSettings>();
			builder.Services.AddHostedService<RoleSeederHostedService>();


			builder.Services.AddScoped<ProtectedLocalStorage>();

			builder.Services.AddControllersWithViews();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/login";
				options.AccessDeniedPath = "/Identity/Account/AccessDenied";
				options.SlidingExpiration = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
			});

			// Add logging to see what's happening
			builder.Services.AddLogging(logging =>
			{
				logging.AddConsole();
				logging.AddDebug();
			});

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

			app.MapControllers();
			app.UseStaticFiles();

			app.UseHttpsRedirection();

            app.UseAntiforgery();

			app.UseAuthentication(); // MUST be before Authorization
			app.UseAuthorization();

			app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
			app.MapRazorPages();
            app.Run();
        }
    }
}
