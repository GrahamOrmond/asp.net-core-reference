# ASP.NET Core Web API Reference
personal reference for asp.net core web API
done through following [Create a web API with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio)
*README copied from Microsoft documentation*

The project template creates a WeatherForecast API with support for [Swagger](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-5.0).

## Swagger 

Swagger is used to generate useful documentation and help pages for web APIs. This tutorial focuses on creating a web API. For more information on Swagger, see [ASP.NET Core web API documentation with Swagger / OpenAPI](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-5.0).

The Swagger page /swagger/index.html is displays.
- The [Curl](https://curl.haxx.se/) command to test the WeatherForecast API.
- The URL to test the WeatherForecast API.
- The response code, body, and headers.
- A drop down list box with media types and the example value and schema.
- 
## Model Classes

A model is a set of classes that represent the data that the app manages. The model for this app is a single TodoItem class.
- In **Solution Explorer**, right-click the project. Select **Add** > **New Folder**. Name the folder *Models*.
- Right-click the *Models* folder and select **Add** > **Class**. Name the class *TodoItem* and select **Add**.

The Id property functions as the unique key in a relational database.

Model classes can go anywhere in the project, but the Models folder is used by convention.

## Update the launchUrl
In Properties\launchSettings.json, update launchUrl from "swagger" to "api/TodoItems":
```json
"launchUrl": "api/TodoItems",
```
Because Swagger has been removed, the preceding markup changes the URL that is launched to the GET method of the controller added in the following sections.

## Add a database context

The database context is the main class that coordinates Entity Framework functionality for a data model. This class is created by deriving from the [Microsoft.EntityFrameworkCore.DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext) class.

### Add NuGet packages
- From the **Tools** menu, select **NuGet Package Manager** > **Manage NuGet Packages for Solution**.
- Select the **Browse** tab, and then enter `Microsoft.EntityFrameworkCore.InMemory` in the search box.
- Select `Microsoft.EntityFrameworkCore.InMemory` in the left pane.
- Select the **Project** checkbox in the right pane and then select **Install**.

#### Add the TodoContext database context
- Right-click the *Models* folder and select **Add** > **Class**. Name the class *TodoContext* and click **Add**.

```c#
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
```

## Register the database context

In ASP.NET Core, services such as the DB context must be registered with the dependency injection (DI) container. The container provides the service to controllers.

Update Startup.cs with the following code:
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<TodoContext>(opt =>
                                       opt.UseInMemoryDatabase("TodoList"));
    services.AddControllers();
}
```

The preceding code:

- Removes the Swagger calls.
- Removes unused using declarations.
- Adds the database context to the DI container.
 - Specifies that the database context will use an in-memory database.

