using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Api.Interface;
using Api.Services;
using Domain.Interface;
using Domain.Interface.Domain.Interface;
using Infrastructure.Context;
using Infrastructure.Interface;
using Infrastructure.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Api
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
            services.AddCors();
            services.AddControllers();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {

                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IUserService, UserService>();


            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            services.AddDbContext<IDbContext,PruebaContext>(options => options.UseSqlite(connection));
            services.AddScoped<IUnitOfWork, UnitOfWork>().AddDbContext<PruebaContext>();

            _ = services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {

                    var obj = context.ModelState;

                    string jsonString = "";

                    obj.Values.Where(x => x.Errors.Count > 0).ToList().ForEach(x =>
                    {
                        x.Errors.ToList().ForEach(x =>
                        {
                            jsonString += x.ErrorMessage.ToString() + System.Environment.NewLine;

                        });

                    });
                    var response = new
                    {
                        Status = false,
                        Message = jsonString,
                    };

                    var result = new BadRequestObjectResult(response);

                    // TODO: add `using System.Net.Mime;` to resolve MediaTypeNames
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                    return result;
                };
            });


            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Woin API",
                    Description = "Web Api service Rev 0.01 Alpha",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Dev Zeros",
                        Email = "carloscastilla31@gmail.com",
                        Url = new Uri("https://desarrollo-zeros.github.io/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use Private licence",
                        Url = new Uri("http://woin.com.co/"),
                    },

                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           if (env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Woin WebService Api Version Rev 0.01");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
