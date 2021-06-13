using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //To add application sql server provider.
            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("EmployeeDBConnection"));
            });

            //To configure the identity db by using entity framework core.
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                //To configure the default password complexity set by PasswordOptions class.
                options.Password.RequiredLength = 7;
            }).AddEntityFrameworkStores<AppDbContext>();

            //to add cross origin resource sharing.
            services.AddCors();

            services.AddControllers();

            //To add JWT authentication
            services.AddAuthentication(configureOptions =>
            {
                configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configureOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
                };
            });
            //if we want to apply athorization globally, we need to add the below options in the AddControllers.
            //services.AddControllers( options => {
            //    var policy = new AuthorizationPolicyBuilder()
            //                     .RequireAuthenticatedUser()
            //                     .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});

            //For google authentication
            //services.AddAuthentication().AddGoogle(options =>
            //{
            //    options.ClientId = "791370068098-vbq4t5sglk3gmev5c92o5tvi6dvoca8o.apps.googleusercontent.com";
            //    options.ClientSecret = "_5rfUtwe9jsaud0aNyDmfn6-";
            //});


            //Registering the Swagger generator
            services.AddSwaggerGen();

            ////To register swaggger with Authorize Button on the Swagger UI.
            //services.AddSwaggerGen(options =>
            //{
            //    var security = new Dictionary<string, IEnumerable<string>>
            //    {
            //        {"Bearer", new string[0] }
            //    };
            //    options.AddSecurityDefinition(name: "Bearer", new ApiKeyScheme
            //    {
            //        Description = "JWT Authorization using bearer",
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey
            //    });
            //    options.AddSecurityRequirement(security);
            //});

            //to register repository.
            //services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
            services.AddScoped<IUserFormRepository, UserFormRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                               ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                developerExceptionPageOptions.SourceCodeLineCount = 1;
                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
            }

            //enable middleware to serve generated swagger as a JSON endpoint
            app.UseSwagger();

            //enable middleware to serve swagger UI.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "Employee Management API");

                //To serve the Swagger UI at the app's root (http://localhost:<port>/), 
                //set the RoutePrefix property to an empty string:
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("home.html");
            //app.UseDefaultFiles(defaultFilesOptions);

            ////app.UseDefaultFiles();
            //app.UseStaticFiles();

            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("home.html");
            //app.UseFileServer(fileServerOptions);

            //app.UseFileServer();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {                    
            //        await context.Response.WriteAsync(_configuration["MyKey"]);
            //    });
            //});


            // app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("Start first middleware");
            //    await context.Response.WriteAsync("Hello from fisrt middleware");               
            //    await next();
            //    logger.LogInformation("Fistmiddleware response over");
            //});
            //app.Run( async (context) =>
            //{
            //    //throw new Exception("Some error occurred");
            //    //logger.LogInformation("start second middleware");
            //    //await context.Response.WriteAsync("Hello from second middleware");
            //    //logger.LogInformation("second middleware response over");

            //    await context.Response.WriteAsync(env.EnvironmentName);
            //});

            //to add cross origin resource sharing allow all methods.
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            //to use authentication.
            app.UseAuthentication();
            //to use authorization.
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
