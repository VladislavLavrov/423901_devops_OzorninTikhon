using App_practical.Data;
using Microsoft.EntityFrameworkCore;

namespace App_practical
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
                o => o.UseNpgsql("Host=postgres;Port=95432;Database=Calculator;Username=postgres;Password=admin")
                );

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                // ���� ��
                await WaitForDatabase(context);

                // ������������� ������� ������� �� ������ �������
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
