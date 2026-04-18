using System;
using System.Collections.Generic;
using DocumentService.Api.Messaging;
using jsreport.AspNetCore;
using jsreport.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolicyService.Api.Events;
using Steeltoe.Discovery.Client;

namespace DocumentService.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDiscoveryClient(Configuration);
        
        var appSettingsSection = Configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);
        var appSettings = appSettingsSection.Get<AppSettings>();

        services.AddMvc().AddNewtonsoftJson();
        
        services.AddMediatR(opts => opts.RegisterServicesFromAssemblyContaining<Startup>());
        
        services.AddJsReport(new ReportingService(appSettings.JsReportUrl));
        
        services.AddRabbitListeners();
        
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        
        app.UseDiscoveryClient();
        
        app.UseRabbitListeners(new List<Type> { typeof(PolicyCreated) });
    }
}
