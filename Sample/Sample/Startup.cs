using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sample.Common;
using Sample.DAL;
using Sample.Impl.Services.CustomLog;
using Sample.Impl.Services.ToDoTasks;
using Sample.Web.Middleware.Extensions;
using Sample.Web.Requirements;
using Sample.Web.Requirements.Handlers;

namespace Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.SecurityKey,
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddDbContext<DataContext>(opt =>
                opt.UseInMemoryDatabase("Sample"));

            services.AddScoped<ITasksService, TasksService>();
            services.AddScoped<ICustomLogService, CustomLogService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyForAlekseiAndAtLeast18", policy =>
                {
                    policy.RequireClaim("firstName", "Aleksei");
                    policy.Requirements.Add(new MinimumAgeRequirement(18));
                });
            });

            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

            services.AddHealthChecks();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestInfoLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
