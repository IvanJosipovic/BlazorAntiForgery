using BlazorAntiForgery;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlazorAntiForgery
    {
        /// <summary>
        /// Adds Blazor Anti Forgery dependencies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cookieName">Name of the cookie that contains the Anti Forgery Token</param>
        /// <param name="headerName">Name of the header that will be added to requests with the Anti Forgery Token</param>
        public static IServiceCollection AddBlazorAntiForgeryServices(this IServiceCollection services, string cookieName = "X-CSRF-TOKEN", string headerName = "X-CSRF-TOKEN")
        {
            return services.AddSingleton(h => new BlazorAntiForgeryDelegatingHandler(h.GetRequiredService<IJSRuntime>()) { CookieName = cookieName, HeaderName = headerName } );
        }

        /// <summary>
        /// Adds Blazor Anti Forgery to this HttpClient
        /// </summary>
        /// <param name="builder"></param>
        public static IHttpClientBuilder AddBlazorAntiForgery(this IHttpClientBuilder builder)
        {
            return builder.AddHttpMessageHandler((s) => s.GetService<BlazorAntiForgeryDelegatingHandler>());
        }
    }
}