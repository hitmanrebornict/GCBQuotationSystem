using GCBQuotationSystem.Components;
using GCBQuotationSystem.Components.Services;
using GCBQuotationSystem.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Radzen;


namespace GCBQuotationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<QuotationSystemContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

			// Add services to the container.
			builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
			builder.Services.AddRadzenComponents();
			builder.Services.AddScoped<MasterDataServices>();
			builder.Services.AddScoped<GenerateQuoteServices>();
			builder.Services.AddScoped<QuoteDataServices>();
			builder.Services.AddScoped<QuoteDetailServices>();
			builder.Services.AddScoped<UserPreferenceSettings>();


			builder.Services.AddScoped<ProtectedLocalStorage>();

			builder.Services.AddControllersWithViews();




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
          

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
