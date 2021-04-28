using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorAntiForgery.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlazorAntiForgeryServices();

            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient());

            builder.Services.AddHttpClient("", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient("AntiForgery", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                            .AddBlazorAntiForgery();

            await builder.Build().RunAsync();
        }
    }
}
