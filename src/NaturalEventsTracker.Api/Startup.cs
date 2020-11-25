using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NaturalEventsTracker.Entities.AppSettingsModels;
using NaturalEventsTracker.Entities.ViewModels;
using NaturalEventsTracker.Entities.ViewModels.Eonet;
using NaturalEventsTracker.Services.Implementations;
using NaturalEventsTracker.Services.Interfaces;
using AutoMapper;

namespace NaturalEventsTracker.Api
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
            services.AddControllers();

            services.AddHttpClient();

            services.AddAutoMapper(config =>
            {
                config.CreateMap<Event, EventViewModel>()
                    .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
                    .ForMember(d => d.Title, o => o.MapFrom(s => s.title))
                    .ForMember(d => d.Descrition, o => o.MapFrom(s => s.description));
            }, typeof(Startup));

            services.AddTransient<IEonetService, EonetService>();

            services.Configure<AppSettings>(Configuration);
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

                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
