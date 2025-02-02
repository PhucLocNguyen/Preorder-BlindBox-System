using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PreOrderBlindBox.API;
using PreOrderBlindBox.Data.DBContext;
using PreOrderBlindBox.Services;
using PreOrderBlindBox.Services.Utils;
using System.Text;

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

			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pre-order Blind Box Api", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							}
						},
						new string[]{}
					}
				});
			});

			builder.Services.AddHttpContextAccessor();

			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
							   .Where(a => !a.IsDynamic)
							   .ToArray();

			builder.Services.AddAutoMapper(assemblies);
			builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
					ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
				};
			});


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(/*c =>
				{
					c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
				}*/);
			}

			app.UseHttpsRedirection();
			app.UseMiddleware<JwtCookieMiddleware>();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
