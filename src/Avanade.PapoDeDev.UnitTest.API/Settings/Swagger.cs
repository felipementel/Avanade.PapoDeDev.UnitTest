using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Avanade.PapoDeDev.UnitTest.API.Settings
{
    public static class Swagger
    {
        public static void AddSwaggerServices(this IServiceCollection services,
            IWebHostEnvironment webHostEnvironment)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1",
                    info: new OpenApiInfo
                    {
                        Title = "Avanade.PapoDeDev.UnitTest.API",
                        Version = "v1",
                        Description = $"Parameters: </br>" +
                        $" - EnvironmentName: {webHostEnvironment.EnvironmentName} </br>" +
                        $" - AssemblyVersion: {Assembly.GetEntryAssembly().GetName().Version} </br>" +
                        $" - OSVersion: {Environment.OSVersion} </br>" +
                        $" - DateAndHour: {DateTime.Now:u} </br>",
                        TermsOfService = new Uri("https://avanade-papodedev.com.br"),
                        Contact = new OpenApiContact
                        {
                            Name = "Felipe & Samuel",
                            Email = "dontexists@avanade-papodedev.com.br"
                        },
                        License = new OpenApiLicense
                        {
                            Name = "BSD",
                            Url = new Uri("https://pt.wikipedia.org/wiki/Licen%C3%A7a_BSD"),
                        }
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });            
        }

        public static void UseSwaggerServices(
            this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                //c.RoutePrefix = string.Empty;

                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Avanade.SubTCSE.Projeto.Api v1");
                options.DisplayRequestDuration();
                options.EnableValidator();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        url: $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}