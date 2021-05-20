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

## Scaffold a controller

- Right-click the *Controllers* folder.
- Select **Add** > **New Scaffolded Item**.
- Select **API Controller** with actions, using **Entity Framework**, and then select **Add**.
- In the **Add API Controller with actions**, **using Entity Framework** dialog:
-- Select **TodoItem (TodoApi.Models)** in the **Model class**.
-- Select **TodoContext (TodoApi.Models)** in the **Data context class**.
-- Select **Add**.

The generated code:
- Marks the class with the [[ApiController]](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.apicontrollerattribute) attribute. This attribute indicates that the controller responds to web API requests. For information about specific behaviors that the attribute enables, see [Create web APIs with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-5.0).
- Uses DI to inject the database context (`TodoContext`) into the controller. The database context is used in each of the [CRUD](https://wikipedia.org/wiki/Create,_read,_update_and_delete) methods in the controller.

The ASP.NET Core templates for:
- Controllers with views include `[action]` in the route template.
- API controllers don't include `[action]` in the route template.

When the `[action]` token isn't in the route template, the action name is excluded from the route. That is, the action's associated method name isn't used in the matching route.

## create method

```c#
// POST: api/TodoItems
[HttpPost]
public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
{
    _context.TodoItems.Add(todoItem);
    await _context.SaveChangesAsync();

    //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
    return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
}
```
The preceding code is an HTTP POST method, as indicated by the [[HttpPost]](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.httppostattribute) attribute. The method gets the value of the to-do item from the body of the HTTP request.

For more information, see [Attribute routing with Http[Verb] attributes](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0#attribute-routing-with-httpverb-attributes).

The [CreatedAtAction](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.createdataction) method:

Returns an [HTTP 201 status code](https://developer.mozilla.org/docs/Web/HTTP/Status/201) if successful. HTTP 201 is the standard response for an HTTP POST method that creates a new resource on the server.
Adds a [Location](https://developer.mozilla.org/docs/Web/HTTP/Headers/Location) header to the response. The `Location` header specifies the [URI](https://developer.mozilla.org/docs/Glossary/URI) of the newly created to-do item. For more information, see [10.2.2 201 Created](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html).
References the `GetTodoItem` action to create the `Location` header's URI. The C# `nameof` keyword is used to avoid hard-coding the action name in the `CreatedAtAction` call.

## Routing and URL paths

The [[HttpGet]](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.httpgetattribute) attribute denotes a method that responds to an HTTP GET request. The URL path for each method is constructed as follows:

Start with the template string in the controller's Route attribute:
```c#
[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
```
- Replace `[controller]` with the name of the controller, which by convention is the controller class name minus the "Controller" suffix. For this sample, the controller class name is **TodoItems**Controller, so the controller name is "TodoItems". ASP.NET Core [routing](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0) is case insensitive.
- If the `[HttpGet]` attribute has a route template (for example, `[HttpGet("products")]`), append that to the path. This sample doesn't use a template. For more information, see [Attribute routing with Http[Verb] attributes](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0#attribute-routing-with-httpverb-attributes).

In the following GetTodoItem method, "{id}" is a placeholder variable for the unique identifier of the to-do item. When GetTodoItem is invoked, the value of "{id}" in the URL is provided to the method in its id parameter.
```c#
// GET: api/TodoItems/5
[HttpGet("{id}")]
public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
{
```

## Return values

The return type of the `GetTodoItems` and `GetTodoItem` methods is [ActionResult<T> type](https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-5.0#actionresultt-type). ASP.NET Core automatically serializes the object to JSON and writes the [JSON](https://www.json.org/) into the body of the response message. The response code for this return type is [200 OK](https://developer.mozilla.org/docs/Web/HTTP/Status/200), assuming there are no unhandled exceptions. Unhandled exceptions are translated into 5xx errors.

`ActionResult` return types can represent a wide range of HTTP status codes. For example, GetTodoItem can return two different status values:
- If no item matches the requested ID, the method returns a [404 status](https://developer.mozilla.org/docs/Web/HTTP/Status/404) [NotFound](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.notfound) error code.
- Otherwise, the method returns 200 with a JSON response body. Returning `item` results in an HTTP 200 response.

### PUT Method
`PutTodoItem` is similar to `PostTodoItem`, except it uses HTTP PUT. The response is [204 (No Content)](https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html). According to the HTTP specification, a PUT request requires the client to send the entire updated entity, not just the changes. To support partial updates, use [HTTP PATCH](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.httppatchattribute).










