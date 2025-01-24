using Microsoft.EntityFrameworkCore;
using PreOrderBlindBox.API;
using PreOrderBlindBox.Data.DBContext;

namespace PreOrderBlindBox.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddWebAPI();

            builder.Services.AddDbContext<Preorder_BlindBoxContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContext"));
            });
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
