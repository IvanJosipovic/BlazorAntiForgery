using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorAntiForgery
{
    /// <summary>
    /// Reads the cookie using JSInterop and adds it as header to the request
    /// </summary>
    public class BlazorAntiForgeryDelegatingHandler : DelegatingHandler
    {
        private IJSRuntime JSRuntime { get; }

        /// <summary>
        /// Name of the header that will be added to requests with the Anti Forgery Token
        /// </summary>
        public string HeaderName { get; set; } = "X-CSRF-TOKEN";

        /// <summary>
        /// Name of the Cookie that contains the Anti Forgery Token
        /// </summary>
        public string CookieName { get; set; } = "X-CSRF-TOKEN";

        public BlazorAntiForgeryDelegatingHandler(IJSRuntime jSRuntime)
        {
            this.JSRuntime = jSRuntime;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add(HeaderName, await GetToken());

            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Retrieves the Anti Forgery Token from the cookies
        /// </summary>
        private async Task<string> GetToken()
        {
            var cookies = await JSRuntime.InvokeAsync<string>("blazorAntiForgery.getCookies");

            try
            {
                return cookies.Split(';')
                              .First(x => x.Trim().StartsWith(CookieName + "="))
                              .Trim()
                              .Substring(CookieName.Length + 1);
            }
            catch (Exception)
            {
                throw new Exception($"Unable to find Cookie: {CookieName}");
            }
        }
    }
}
