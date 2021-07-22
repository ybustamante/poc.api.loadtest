using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace poc.api.loadtest
{
    public class Startup
    {
        private ILogger<Startup> _logger;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var config = new AWS.Logger.AWSLoggerConfig("yb.webapp.trading")
            {
                Region = "us-east-1"
            };

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Program", LogLevel.Debug)
                    .SetMinimumLevel(LogLevel.Information)
                    .AddConsole();
            }).AddAWSProvider(config);

            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "poc.api.loadtest", Version = "v1" });
            });
            services.AddLogging(config =>
            {
                config.AddAWSProvider(Configuration.GetAWSLoggingConfigSection());
                config.SetMinimumLevel(LogLevel.Debug);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }            

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "poc.api.loadtest v1"));            

            app.UseRouting();

            app.UseAuthorization();            

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapControllers();                
            });
        }
    }
}
