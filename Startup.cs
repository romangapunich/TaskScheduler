using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentScheduler;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;
using SuperAgentCore.Settings.BackGroundService;
using SuperAgentCore.Settings.BearerToken;
using TaskScheduler.Extensions;
using TaskScheduler.MediatR;
using TaskScheduler.MediatR.Behavior;
using TaskScheduler.Services.BackGroundService;
using TaskScheduler.Swagger;

namespace TaskScheduler
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private object _environment;
        private readonly string _serviceName;
        private readonly string _ver;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _serviceName = AppDomain.CurrentDomain.FriendlyName;
            _ver = GetType().Assembly.GetName().Version.ToString();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            
            services.Configure<BearerTokensSettings>(_configuration.GetSection("BearerTokensSettings"));
           
            services.Configure<BackgroundSettings>(_configuration.GetSection("BackgroundSettings"));
            services.AddCoreSwagger(_serviceName, _ver, "Specify the SA token.");
            services.AddSwaggerGen();
            services.AddAuthorizationPolicy(_configuration);
            services.AddMediatR(typeof(Startup));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddMvc();
            // FluentValidation
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddFluentValidation(new[] { typeof(Startup).GetTypeInfo().Assembly, typeof(IPipelineBehavior<,>).Assembly });

            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication();
            services.AddControllers();
            services.AddScoped<ISayHiService, SayHiService>();
            var serviceProvider = services.BuildServiceProvider();
            JobManager.Initialize(new JobRegistry(serviceProvider));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePathBase("/api");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCoreSwagger("/api/swagger/v1/swagger.json", $"{_serviceName} API v.{_ver}");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
