using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Domain.Mapping;
using OrganizerAPI.Domain.Services;
using OrganizerAPI.Domain.Validators;
using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Infrastructure.Repositories;
using OrganizerAPI.Shared.ModelsDTO;

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
            services.AddControllers().AddNewtonsoftJson(options => 
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddDbContext<OrganizerContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("Organizer")));

            services.AddScoped<IUserTaskService, UserTaskService>();

            services.AddTransient<AbstractValidator<UserTaskDTO>, UserTaskValidator>();

            services.AddScoped<UserTaskRepository>();

            services.AddTransient<IMapper, Mapper>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
