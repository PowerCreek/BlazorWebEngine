using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorWebEngine.Components;
using BlazorWebEngine.Management;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebEngine
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp =>
                new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

            builder.Services.AddScoped<IElementServices, BackingService>();
            builder.Services.AddScoped<IBuilder, BackingServiceBuilder>();

            builder.Services.AddScoped<OperationManager>();
            builder.Services.AddScoped<ElementContextProvider>();
            builder.Services.AddScoped<ComponentMap>();
            builder.Services.AddScoped<NodeManager>();
            builder.Services.AddScoped<NodeRegistry>();
            builder.Services.AddScoped<NodeInformation>();

            builder.Services.AddScoped<RootComponent>();

            await builder.Build().RunAsync();
        }
    }
}