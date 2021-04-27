using Microsoft.JSInterop;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorAntiForgery.Client
{
    public class BlazorAntiForgeryDelegatingHandler : DelegatingHandler
    {
        private IJSRuntime JSRuntime { get; set; }

        public string HeaderName { get; set; } = "X-CSRF-TOKEN";

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
        /// <returns></returns>
        private async Task<string> GetToken()
        {
            var cookies = await JSRuntime.InvokeAsync<string>("blazorAntiForgery.getCookies");

            return cookies.Split(';')
                          .First(x => x.StartsWith(HeaderName + "="))
                          [(HeaderName.Length + 1)..];
        }
    }
}
