ASP.NET Core Identity Reference
Personal referece for asp.net identity Web App with Razor pages
**README copied from [ASP.NET Core authentication docs](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-5.0)**


## Create a Web app with authentication

- Select File > New > Project.
- Select ASP.NET Core Web Application.
- Select Individual User Accounts and click OK.

Create an ASP.NET Core Web Application project with Individual User Accounts. The generated project provides [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-5.0) as a [Razor Class Library](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/ui-class?view=aspnetcore-5.0). The Identity Razor Class Library exposes endpoints with the `Identity` area. For example:
- /Identity/Account/Login
- /Identity/Account/Logout
- /Identity/Account/Manage

## Configure Identity services
Services are added in `ConfigureServices`. The typical pattern is to call all the `Add{Service}` methods, and then call all the `services.Configure{Service}` methods. Services are made available to the app through dependency injection.

Identity is enabled by calling [UseAuthentication](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.authappbuilderextensions.useauthentication#Microsoft_AspNetCore_Builder_AuthAppBuilderExtensions_UseAuthentication_Microsoft_AspNetCore_Builder_IApplicationBuilder_). `UseAuthentication` adds authentication [middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0) to the request pipeline.

The template-generated app doesn't use [authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-5.0). `app.UseAuthorization` is included to ensure it's added in the correct order should the app add authorization. `UseRouting`, `UseAuthentication`, `UseAuthorization`, and `UseEndpoints` must be called in order listed.
