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

## Scaffold Identity

- From Solution Explorer, right-click on the project > **Add** > **New Scaffolded Item**.
- From the left pane of the **Add Scaffold dialog**, select **Identity** > **Add**.
- In the **Add Identity** dialog, select the options you want.
-- Select your existing layout page so your layout file isn't overwritten with incorrect markup. When an existing 
_Layout.cshtml file is selected, it is not overwritten. For example:
-- `~/Pages/Shared/_Layout.cshtml` for Razor Pages or Blazor Server projects with existing Razor Pages infrastructure
-- `~/Views/Shared/_Layout.cshtml` for MVC projects or Blazor Server projects with existing MVC infrastructure
- To use your existing data context, select at least one file to override. You must select at least one file to add your data context.
-- Select your data context class.
-- Select Add.
- To create a new user context and possibly create a custom user class for Identity:
-- Select the + button to create a new Data context class. Accept the default value or specify a class (for example, `MyApplication.Data.ApplicationDbContext`).
-- Select Add.