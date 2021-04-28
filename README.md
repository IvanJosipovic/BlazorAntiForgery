[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/BlazorAntiForgery.svg?style=flat-square)](https://www.nuget.org/packages/BlazorAntiForgery)
[![Nuget (with prereleases)](https://img.shields.io/nuget/dt/BlazorAntiForgery.svg?style=flat-square)](https://www.nuget.org/packages/BlazorAntiForgery)
![](https://github.com/IvanJosipovic/BlazorAntiForgery/workflows/Create%20Release/badge.svg)

Anti Cross-Site Request Forgery (CSRF) for Blazor web applications

## [Example Project](https://github.com/IvanJosipovic/BlazorAntiForgery/tree/master/src/Sample)

## Install in Blazor

- Add [BlazorAntiForgery NuGet](https://www.nuget.org/packages/BlazorAntiForgery)
  - dotnet add package BlazorAntiForgery
- Add calls to Program.cs

    ```csharp
    builder.Services.AddBlazorAntiForgeryServices();

    builder.Services.AddHttpClient("AntiForgery", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddBlazorAntiForgery();
    ```

- Add JS Interop to the bottom of body in index.html

  ```html
  <script src="_content/BlazorAntiForgery/JSInterop.js"></script>
  ```

## Install in ASP.NET

- Add to ConfigureServices in Startup.cs

    ```csharp
    services.AddAntiforgery(options =>
    {
        options.HeaderName = "X-CSRF-TOKEN";
    });
    ```

- Add to Configure in Startup.cs
  - Note, this configuration assumes that Blazor is hosted by ASP.Net. The paths will need to be adjusted if Blazor is hosted separately.

  ```csharp
    app.Use(next => context =>
    {
      if (context.Request.Path.Value.StartsWith("/_framework/blazor.webassembly.js", StringComparison.OrdinalIgnoreCase))
      {
          var tokens = app.ApplicationServices.GetRequiredService<IAntiforgery>().GetAndStoreTokens(context);
          context.Response.Cookies.Append(tokens.HeaderName, tokens.RequestToken, new CookieOptions() { HttpOnly = false, Secure = true, SameSite = SameSiteMode.Strict });
      }

      return next(context);
    });
  ```

## Usage

- In ASP.NET, add [AutoValidateAntiforgeryToken] attribute to the Controllers or Actions
- In Blazor, use IHttpClientFactory to create an AntiForgery Client. The client will automatically send the Anti Forgery Tokens.

    ```csharp
    @page "/"

    <EditForm Model="@model" OnValidSubmit="@HandleValidSubmit">
        <InputText id="name" @bind-Value="model.Value" />

        <button type="submit">Submit</button>
    </EditForm>

    Success: @response

    @code{
        [Inject] private IHttpClientFactory httpClientFactory { get; set; }

        private Model model = new Model() { Value = "test" };
        private bool? response;

        private async Task HandleValidSubmit()
        {
            using var client = httpClientFactory.CreateClient("AntiForgery");

            var resp = await client.PostAsJsonAsync<Model>("/api/AntiForgeryTest", model);

            response = resp.IsSuccessStatusCode;

            StateHasChanged();
        }
    }
    ```
