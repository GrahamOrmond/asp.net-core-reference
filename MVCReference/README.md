# Asp.net Core MVC Reference
personal reference for Asp.net Core MVC
done through following [Get started with ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-5.0&tabs=visual-studio)
*README copied from Microsoft documentation*

## MVC
The Model-View-Controller (MVC) architectural pattern separates an app into three main components: Model, View, and Controller. The MVC pattern helps you create apps that are more testable and easier to update than traditional monolithic apps.

MVC-based apps contain:

- Models: Classes that represent the data of the app. The model classes use validation logic to enforce business rules for that data. Typically, model objects retrieve and store model state in a database. In this tutorial, a Movie model retrieves movie data from a database, provides it to the view or updates it. Updated data is written to a database.
- Views: Views are the components that display the app's user interface (UI). Generally, this UI displays the model data.
- Controllers: Classes that:
-- Handle browser requests.
-- Retrieve model data.
-- Call view templates that return a response.

The MVC architectural pattern separates an app into three main groups of components: Models, Views, and Controllers. This pattern helps to achieve separation of concerns: The UI logic belongs in the view. Input logic belongs in the controller. Business logic belongs in the model. This separation helps manage complexity when building an app, because it enables work on one aspect of the implementation at a time without impacting the code of another.

## Controllers

Every public method in a controller is callable as an HTTP endpoint. In the sample above, both methods return a string. Note the comments preceding each method.

An HTTP endpoint:
- Is a targetable URL in the web application, such as https://localhost:5001/HelloWorld.
- Combines:
-- The protocol used: HTTPS.
-- The network location of the web server, including the TCP port: localhost:5001.
-- The target URI: HelloWorld.

The first comment states this is an [HTTP GET](https://developer.mozilla.org/docs/Web/HTTP/Methods/GET) method that's invoked by appending `/HelloWorld/` to the base URL.

The second comment specifies an [HTTP GET](https://developer.mozilla.org/docs/Web/HTTP/Methods/GET) method that's invoked by appending `/HelloWorld/Welcome/` to the URL. Later on in the tutorial, the scaffolding engine is used to generate `HTTP POST` methods, which update data.

MVC invokes controller classes, and the action methods within them, depending on the incoming URL. The default [URL routing logic](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0) used by MVC, uses a format like this to determine what code to invoke:
`/[Controller]/[ActionName]/[Parameters]`

The routing format is set in the Configure method in Startup.cs file.
```c#
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
```

When you browse to the app and don't supply any URL segments, it defaults to the "Home" controller and the "Index" method specified in the template line above.
- The first URL segment determines the controller class to run. So `localhost:5001/HelloWorld` maps to the HelloWorldController class.
- The second part of the URL segment determines the action method on the class. So `localhost:5001/HelloWorld/Index` causes the `Index` method of the `HelloWorldController` class to run. Notice that you only had to browse to `localhost:5001/HelloWorld` and the `Index` method was called by default. `Index` is the default method that will be called on a controller if a method name isn't explicitly specified.
- The third part of the URL segment (`id`) is for route data. Route data is explained later in the tutorial.

### url parameters
```c#
// GET: /HelloWorld/Welcome/ 
// Requires using System.Text.Encodings.Web;
public string Welcome(string name, int numTimes = 1)
{
    return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
}
```
The preceding code:
- Uses the C# optional-parameter feature to indicate that the numTimes parameter defaults to 1 if no value is passed for that parameter.
- Uses HtmlEncoder.Default.Encode to protect the app from malicious input, such as through JavaScript.
-Uses Interpolated Strings in $"Hello {name}, NumTimes is: {numTimes}".

## Views

View templates are created using Razor. Razor-based view templates:
- Have a .cshtml file extension.
- Provide an elegant way to create HTML output with C#.

```c#
public IActionResult Index()
{
    return View();
}
```
The preceding code:
- Calls the controller's View method.
- Uses a view template to generate an HTML response.
 
Controller methods:
- Are referred to as action methods. For example, the Index action method in the preceding code.
- Generally return an IActionResult or a class derived from ActionResult, not a type like string.

Controller actions are invoked in response to an incoming URL request. A controller class is where the code is written that handles the incoming browser requests. The controller retrieves data from a data source and decides what type of response to send back to the browser. View templates can be used from a controller to generate and format an HTML response to the browser.

View templates should not:
- Do business logic
- Interact with a database directly.

A view template should work only with the data that's provided to it by the controller. Maintaining this "separation of concerns" helps keep the code:
- Clean.
- Testable.
- Maintainable.

### Layout Pages

[Layout](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/layout?view=aspnetcore-5.0) templates allows:
- Specifying the HTML container layout of a site in one place.
- Applying the HTML container layout across multiple pages in the site.

RenderBody is a placeholder where all the view-specific pages you create show up, wrapped in the layout page. For example, if you select the Privacy link, the Views/Home/Privacy.cshtml view is rendered inside the RenderBody method.


The Views/_ViewStart.cshtml file brings in the Views/Shared/_Layout.cshtml file to each view. The Layout property can be used to set a different layout view, or set it to null so no layout file will be used.

The content in the Index.cshtml view template is merged with the Views/Shared/_Layout.cshtml view template. A single HTML response is sent to the browser. Layout templates make it easy to make changes that apply across all of the pages in an app. To learn more, see [Layout](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/layout?view=aspnetcore-5.0).

## Models
Add classes for managing entities in a database. These classes will be the "Model" part of the MVC app.
You use these classes with Entity Framework Core (EF Core) to work with a database. EF Core is an object-relational mapping (ORM) framework that simplifies the data access code that you have to write.

The model classes you create are known as POCO classes (from Plain Old CLR Objects) because they don't have any dependency on EF Core. They just define the properties of the data that will be stored in the database.

### Create a database context class

A database context class is needed to coordinate EF Core functionality (Create, Read, Update, Delete) for the `Movie` model. The database context is derived from [Microsoft.EntityFrameworkCore.DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext) and specifies the entities to include in the data model.

- Create a Data folder
- Add a Data/MvcMovieContext.cs file with the following code:
```c#
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcReference.Data
{
    public class MvcReferenceContext : DbContext
    {
        public MvcReferenceContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
    }
}
```
The preceding code creates a DbSet<Movie> property for the entity set. In Entity Framework terminology, an entity set typically corresponds to a database table. An entity corresponds to a row in the table.

### Register the database context

ASP.NET Core is built with [dependency injection (DI)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0). Services (such as the EF Core DB context) must be registered with DI during application startup. Components that require these services (such as Razor Pages) are provided these services via constructor parameters. 

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();

    services.AddDbContext<MvcMovieContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("MvcMovieContext")));
}
```
The name of the connection string is passed in to the context by calling a method on a [DbContextOptions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontextoptions) object. For local development, the [ASP.NET Core configuration system](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0) reads the connection string from the *appsettings.json* file.

### Add a database connection string
Add a connection string to the appsettings.json file:
```json
{
  "ConnectionStrings": {
    "MvcReferenceContext": "Server=(localdb)\\mssqllocaldb;Database=MvcReferenceContext-1;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### Scaffold model pages

Use the scaffolding tool to produce Create, Read, Update, and Delete (CRUD) pages for the model.
- In Solution Explorer, right-click the Controllers folder > **Add** > **New Scaffolded Item**.
- In the **Add Scaffold** dialog, select **MVC Controller with views**, **using Entity Framework** > **Add**.
- Complete the Add Controller dialog:
-- Model class: Movie (MvcReference.Models)
-- Data context class: MvcReferenceContext (MvcReference.Data)
-- Views: Keep the default of each option checked
-- Controller name: Keep the default MoviesController
- Select Add

Visual Studio creates:
- A movies controller (Controllers/MoviesController.cs)
- Razor view files for Create, Delete, Details, Edit, and Index pages (Views/Movies/*.cshtml)

The automatic creation of these files is known as scaffolding.

### initial migration

Use the EF Core [Migrations](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/migrations?view=aspnetcore-5.0) feature to create the database. Migrations is a set of tools that let you create and update a database to match your data model.

- From the Tools menu, select NuGet Package Manager > Package Manager Console (PMC).
- In the PMC, enter the following commands:
-- Add-Migration InitialCreate
-- Update-Database

`Add-Migration InitialCreate`:  Generates a Migrations/{timestamp}_InitialCreate.cs migration file. The `InitialCreate` argument is the migration name. Because this is the first migration, the generated class contains code to create the database schema. The database schema is based on the model specified in the `MvcReferenceContext` class.

`Update-Database`: Updates the database to the latest migration, which the previous command created. This command runs the `Up` method in the *Migrations/{time-stamp}_InitialCreate.cs* file, which creates the database.

For more information on the PMC tools for EF Core, see [EF Core tools reference - PMC in Visual Studio](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell).

### Dependency injection in the controller
```c#
public class MoviesController : Controller
{
    private readonly MvcReferenceContext _context;

    public MoviesController(MvcReferenceContext context)
    {
        _context = context;
    }
    
    The constructor uses [Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0) to inject the database context (`MvcReferenceContext`) into the controller. The database context is used in each of the [CRUD](https://wikipedia.org/wiki/Create,_read,_update_and_delete) methods in the controller.
```

### Strongly typed models

MVC also provides the ability to pass strongly typed model objects to a view. This strongly typed approach enables compile time code checking. The scaffolding mechanism used this approach (that is, passing a strongly typed model) with the `MoviesController` class and views.

The `id` parameter is generally passed as route data. For example https://localhost:5001/movies/details/1 sets:
- The controller to the `movies` controller (the first URL segment).
- The action to `details` (the second URL segment).
- The id to 1 (the last URL segment).
You can also pass in the `id` with a query string as follows:
- https://localhost:5001/movies/details?id=1
The `id` parameter is defined as a nullable type (`int?`) in case an ID value isn't provided.

### @model keyword

The `@model` statement at the top of the view file specifies the type of object that the view expects. When the movie controller was created, the following `@model` statement was included:
```cshtml
@model MvcMovie.Models.Movie
```
This `@model` directive allows access to the movie that the controller passed to the view. The `Model` object is strongly typed. 

## Controller methods and Views 

[Tag Helpers][1] enable server-side code to participate in creating and rendering HTML elements in Razor files. [Tag Helpers][1] are one of the most popular new features in ASP.NET Core. For more information, see Additional resources.

The `[Bind]` attribute is one way to protect against [over-posting](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost). You should only include properties in the [Bind] attribute that you want to change. For more information, see [Protect your controller from over-posting](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application). [ViewModels](https://rachelappel.com/use-viewmodels-to-manage-data-amp-organize-code-in-asp-net-mvc-applications/) provide an alternative approach to prevent over-posting.

The HttpPost attribute specifies that this Edit method can be invoked only for POST requests. You could apply the [HttpGet] attribute to the first edit method, but that's not necessary because [HttpGet] is the default.

The `ValidateAntiForgeryToken` attribute is used to [prevent forgery of a request](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-5.0) and is paired up with an anti-forgery token generated in the edit view file (Views/Movies/Edit.cshtml). The edit view file generates the anti-forgery token with the [Form Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-5.0).

The [Form Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-5.0) generates a hidden anti-forgery token that must match the `[ValidateAntiForgeryToken]` generated anti-forgery token in the `Edit` method of the Movies controller. For more information, see [Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-5.0).

### Processing the POST Request

The `[ValidateAntiForgeryToken]` attribute validates the hidden [XSRF](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-5.0) token generated by the anti-forgery token generator in the [Form Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-5.0)

The [model binding](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-5.0) system takes the posted form values and creates a `Movie` object that's passed as the `movie` parameter. The `ModelState.IsValid` property verifies that the data submitted in the form can be used to modify (edit or update) a `Movie` object. If the data is valid, it's saved. The updated (edited) movie data is saved to the database by calling the `SaveChangesAsync` method of database context. After saving the data, the code redirects the user to the `Index` action method of the `MoviesController` class, which displays the movie collection, including the changes just made.

Before the form is posted to the server, client-side validation checks any validation rules on the fields. If there are any validation errors, an error message is displayed and the form isn't posted. If JavaScript is disabled, you won't have client-side validation but the server will detect the posted values that are not valid, and the form values will be redisplayed with error messages. Later in the tutorial we examine [Model Validation](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0) in more detail. The [Validation Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-5.0) in the *Views/Movies/Edit.cshtml* view template takes care of displaying appropriate error messages.

[1]: https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-5.0

