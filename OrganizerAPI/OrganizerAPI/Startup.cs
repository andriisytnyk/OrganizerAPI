using System;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Domain.Mapping;
using OrganizerAPI.Domain.Services;
using OrganizerAPI.Domain.Validators;
using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Infrastructure.Repositories;
using OrganizerAPI.Shared;
using OrganizerAPI.Shared.ModelsDTO;
=======
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
>>>>>>> master

namespace OrganizerAPI
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
<<<<<<< HEAD
            services.AddCors();
            //services.AddControllers().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //);
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            // configure strongly typed settings objects
            var jwtSettingsSection = Configuration.GetSection("JWTSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);

            // configure jwt authentication
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddDbContext<OrganizerContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("Organizer")));

            services.AddScoped<IUserTaskService, UserTaskService>();
            services.AddScoped<IUserService, UserService>();

            services.AddTransient<AbstractValidator<UserTaskDto>, UserTaskValidator>();
            services.AddTransient<AbstractValidator<UserRequestDto>, UserRequestDtoValidator>();
            services.AddTransient<AbstractValidator<UserDto>, UserDtoValidator>();

            services.AddScoped<UserTaskRepository>();
            services.AddScoped<UserRepository>();

            services.AddTransient<IMapper, Mapper>();
=======
            services.AddControllers();
>>>>>>> master
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
