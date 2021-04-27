using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BlazorAntiForgery.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton<BlazorAntiForgeryDelegatingHandler>();

            builder.Services
                .AddHttpClient("", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler((s) => s.GetService<BlazorAntiForgeryDelegatingHandler>());

            await builder.Build().RunAsync();
        }
    }
}
