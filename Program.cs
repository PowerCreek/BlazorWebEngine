using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using BlazorWebEngine;
using BlazorWebEngine.Classes;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.ElementHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services.AddScoped<BackingService>();
            builder.Services.AddScoped<IBuilder, BackingServiceBuilder>();
            
            builder.Services.AddScoped<OperationManager>();
            
            builder.Services.AddScoped<ElementRegistry>();
            builder.Services.AddScoped<ElementInformation>();
            builder.Services.AddScoped<ElementManager>();
            
            builder.Services.AddScoped<RootComponent>();

            await builder.Build().RunAsync();
        }
    }

}
