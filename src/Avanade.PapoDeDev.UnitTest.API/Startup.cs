using Avanade.PapoDeDev.UnitTest.API.FilterType;
using Avanade.PapoDeDev.UnitTest.API.Settings;
using Avanade.PapoDeDev.UnitTest.Domain.Configs;
using Avanade.PapoDeDev.UnitTest.Infra.CrossCutting;
using Avanade.PapoDeDev.UnitTest.Infra.Data.Maps.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Mime;

namespace Avanade.PapoDeDev.UnitTest.API
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        private readonly IWebHostEnvironment _webHostEnvironment;

        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.Filters.Add<ExceptionFilterAttribute>();
                config.RequireHttpsPermanent = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.IgnoreNullValues = false;
                options.JsonSerializerOptions.IgnoreReadOnlyProperties = false;
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new BadRequestObjectResult(context.ModelState);

                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    result.ContentTypes.Add(MediaTypeNames.Application.Xml);

                    return result;
                };
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 1);
            })
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Optimal;
            })
            .AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.AddSwaggerServices(_webHostEnvironment);            

            services.AddOptions<Domain.Config.ConnectionStringsMongoDb>()
                .Bind(_configuration.GetSection(nameof(Domain.Config.ConnectionStringsMongoDb)));

            //
            var _externalConfigurations = new ExternalConfigurations();

            services.AddOptions<ExternalConfigurations>()
                .Bind(_configuration.GetSection(nameof(ExternalConfigurations)));

            _configuration.GetSection("ExternalConfigurations")
                .Bind(_externalConfigurations);

            services.AddDependeciesInjections(_externalConfigurations);

            SetupMaps.ConfigureMaps();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                   options =>
                   {
                       options.Run(
                           async context =>
                           {
                               context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                               context.Response.ContentType = "text/html";
                               var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
                               if (null != exceptionObject)
                               {
                                   var errorMessage = $"<b>Error: {exceptionObject.Error.Message}</b> { exceptionObject.Error.StackTrace}";
                                   await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
                               }
                           });
                   });
            }

            app.UseCors(builder => builder.AllowAnyMethod()
                                          .AllowAnyOrigin()
                                          .AllowAnyHeader());

            app.UseSwaggerServices(provider);

            app.UseResponseCompression();

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