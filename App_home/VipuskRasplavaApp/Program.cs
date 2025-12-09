using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RaspredeleniyeDutyaApp.Data;

namespace RaspredeleniyeDutyaApp
{
    public class Program
    {
        private static async Task WaitForDatabase(DatabaseContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                if (await context.Database.CanConnectAsync())
                {
                    Console.WriteLine("Database connected!");
                    return;
                }
                else
                {
                    Console.WriteLine($"Waiting for database... ({i + 1}/10)");
                    await Task.Delay(3000);
                }
            }
        }

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DatabaseContext>(
                o => o.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"))
                );
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/home");
            builder.Services.AddAuthorization();
            builder.Services.AddMvc();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Raspredeleniye.xml"));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                await WaitForDatabase(context);

                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("Database tables created!");
            }

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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
