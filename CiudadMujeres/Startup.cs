using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using CiudadMujeres.VueCoreConnection;
using CiudadMujeres.Data;
using CiudadMujeres.Models.Security;
using CiudadMujeres.Security;
using CiudadMujeres.Filters;
using CiudadMujeres.Infraestructure;
using CiudadMujeres.Services;
using CiudadMujeres.Entities;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace CiudadMujeres
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var jwtSettings = new JwtSettings();
			Configuration.Bind("JwtSettings", jwtSettings);

			services.AddSingleton(jwtSettings);
			services.AddTransient<JwtTokenCreator>();

			services.AddControllersWithViews();

			services.AddDbContext<ApplicationContext>(options =>
			{
				var conexion = Configuration.GetConnectionString("DefaultConection");
				options.UseMySql(Configuration.GetConnectionString("DefaultConection"), ServerVersion.AutoDetect(conexion));
			});

			services.AddScoped<ValidateEntityExistsAttribute<Entities.Persona>>();
			services.AddScoped<ValidateEntityExistsAttribute<Entities.Libro>>();
			services.AddScoped<ValidateEntityExistsAttribute<Entities.Comentario>>();

			services.AddScoped<IPersonaRepository, PersonaRepository>();
			services.AddScoped<ILibroRepository, LibroRepository>();
			services.AddScoped<IComentarioRepository, ComentarioRepository>();


			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationContext>()
				.AddDefaultTokenProviders();

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Ciudad de las Mujeres API", Version = "v1" });
				// Agregamos la definicion de seguridad de swagger, esto para permitir que swwager reciba el token de seguridad y procesar las solicitudes
				c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = Microsoft.OpenApi.Models.ParameterLocation.Header
				});
				c.CustomSchemaIds(type => type.ToString());
				c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
						  Reference = new OpenApiReference
						  {
							  Type= ReferenceType.SecurityScheme,
							  Id= "Bearer"
						  }
						},
						new string[]{ }
					}
				});

				// Establecemos los comentarios en la ruta principal de Swagger JSON y UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

			services.AddAuthentication(i =>
			{
				i.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				i.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				i.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				i.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = jwtSettings.Issuer,
						ValidAudience = jwtSettings.Audience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
						ClockSkew = jwtSettings.Expire
					};
					options.SaveToken = true;
					options.Events = new JwtBearerEvents();
					options.Events.OnMessageReceived = context =>
					{

						if (context.Request.Cookies.ContainsKey("X-Access-Token"))
						{
							context.Token = context.Request.Cookies["X-Access-Token"];
						}

						return Task.CompletedTask;
					};
				})
				.AddCookie(options =>
				{
					options.Cookie.SameSite = SameSiteMode.Strict;
					options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
					options.Cookie.IsEssential = true;
				});

			// connect vue app - middleware
			services.AddSpaStaticFiles(options => options.RootPath = "client-app/dist");

			services.Configure<IdentityOptions>(options =>
			{
				// Password settings.
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;

				// Lockout settings.
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;

				// User settings.
				options.User.AllowedUserNameCharacters =
				"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = true;
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext context)
		{
			context.Database.EnsureCreated();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ciudad de las Mujeres V1");
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			// use middleware and launch server for Vue
			app.UseSpaStaticFiles();
			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "client-app";
				if (env.IsDevelopment())
				{

					spa.UseVueDevelopmentServer();
				}
			});
		}
	}
}
