# Asp.net Core Web App
reference for .net web app with razor pages
Tutorials done through *[Get started with Razor Pages in ASP.NET Core][1]*
*ReadME Copied from Microsoft Documentation*

## Project files

##### Pages folder
Contains Razor pages and supporting files. Each Razor page is a pair of files:

- .cshtml file that has HTML markup with C# code using Razor syntax.
- .cshtml.cs file that has C# code that handles page events.

Supporting files have names that begin with an underscore. For example, the _Layout.cshtml file configures UI elements common to all pages.

##### wwwroot folder
Contains static assets, like HTML files, JavaScript files, and CSS files. For more information, see [Static files in ASP.NET Core.](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-5.0)

##### appsettings.json
Contains configuration data, like connection strings. For more information, see [Configuration in ASP.NET Core.](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0)

##### Program.cs
Contains the entry point for the app. For more information, see [.NET Generic Host in ASP.NET Core.](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0)

##### Startup.cs
Contains code that configures app behavior. For more information, see [App startup in ASP.NET Core.](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-5.0)

## Models

classes are added for managing entities in a database. The app's model classes use [Entity Framework Core (EF Core)][2] to work with the database. EF Core is an object-relational mapper (O/RM) that simplifies data access. You write the model classes first, and EF Core creates the database. In Entity Framework terminology, an entity set typically corresponds to a database table. An entity corresponds to a row in the table.

The model classes are known as POCO classes (from "Plain-Old CLR Objects") because they don't have a dependency on EF Core. They define the properties of the data that are stored in the database.

##### adding model to project

- right-click project > **Add** > **New Folder**. Name the folder *Models*
- right-click models folder > **Add** > **Class**

```c#
public class Movie
{
    public int ID { get; set; }
    public string Title { get; set; }

    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; }
    public decimal Price { get; set; }
}
```
The Movie class contains:

- The ID field is required by the database for the primary key.
- `[DataType(DataType.Date)]`: The [[DataType]](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.datatypeattribute?view=net-5.0) attribute specifies the type of the data (`Date`). With this attribute:
-- The user isn't required to enter time information in the date field.
-- Only the date is displayed, not time information.

[more DataAnnotations here][4]

##### Scaffold the model

using the the scaffolding tool to produce pages for Create, Read, Update, and Delete (CRUD) operations for the model.

1. right-click *pages* folder > **Add** > **New Folder** *(give it the same name as the model but plural)*
2. right-click *Pages/ModelName* > **Add** > **New Scaffolded Item**
3. in *Add Scaffold* dialog, **Razor Pages using Entity Framework (CRUD)** > **Add**.
4. Complete the Add Razor Pages using Entity Framework (CRUD) dialog:
a. In the **Model class** drop down, select ***ModelName* (ProjectName.Models)**.
b. In the **Data context class** row, select the + (plus) sign.
c. In the **Add Data Context** dialog, the class name `ProjectName.Data.RazorPagesModelNameContext` is generated.
d. select **Add**

**The appsettings.json file is updated with the connection string used to connect to a local database.**

The scaffold process creates and updates the following files:
- Pages/ModelName: Create, Delete, Details, Edit, and Index. - (model CRUD function)
- Data/ProjectNameContext.cs - (coordinates EF Core functionality - derived from [Microsoft.EntityFrameworkCore.DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext))
- Startup.cs - (registers [dependency injection][3])

#### Create initial database schema using Entity Framework migration
In Entity Framework terminology, an entity set typically corresponds to a database table. An entity corresponds to a row in the table.

the Package Manager Console (PMC) window can be used to:
- Add an initial migration.
- Update the database with the initial migration.

Initial migration
1. From the **Tools** menu, select **NuGetPackage Manager** > **Package Manager Console**
2. In the **PMC**, enter the following commands:
`Add-Migration InitialCreate`
`Update-Database`

The `migrations` command generates code to create the initial database schema. The schema is based on the model specified in `DbContext`. The `InitialCreate` argument is used to name the migrations. Any name can be used, but by convention a name is selected that describes the migration.

The `update` command runs the `Up` method in migrations that have not been applied. In this case, `update` runs the `Up` method in the *Migrations/<time-stamp>_InitialCreate.cs* file, which creates the database.

## Scaffolded Razor Pages
Razor Pages are derived from `PageModel`. By convention, the `PageModel`-derived class is named `<PageName>Model`. The constructor uses [dependency injection][3] to add the `RazorPagesMovieContext` to the page:
```c#
public class IndexModel : PageModel
{
    private readonly WebAppReference.Data.WebAppReferenceContext _context;

    public IndexModel(WebAppReference.Data.WebAppReferenceContext context)
    {
        _context = context;
    }
```

### Razor .cshtml.cs
When a request is made for the page, the `OnGetAsync` method returns a list of movies to the Razor Page. On a Razor Page, `OnGetAsync` or `OnGet` is called to initialize the state of the page. In this case, `OnGetAsync` gets a list of movies and displays them.

See [Asynchronous code](https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-5.0#asynchronous-code) for more information on asynchronous programming with Entity Framework.

When OnGet returns void or OnGetAsync returns Task, no return statement is used. For example the Privacy Page:
```c#
public class PrivacyModel : PageModel
{
    public void OnGet()
    {
    }
}
```
When the return type is `IActionResult` or `Task<IActionResult>`, a return statement must be provided. For example, the *Pages/Movies/Create.cshtml.cs* `OnPostAsync` method:
```c#
public async Task<IActionResult> OnPostAsync()
{
    if (!ModelState.IsValid)
        return Page();

    _context.Movie.Add(Movie);
    await _context.SaveChangesAsync();

    return RedirectToPage("./Index");
}
```

### Razor .cshtml

Razor can transition from HTML into C# or into Razor-specific markup. When an `@` symbol is followed by a [Razor reserved keyword](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-5.0#razor-reserved-keywords), it transitions into Razor-specific markup, otherwise it transitions into C#.

#### @page directive
The `@page` Razor directive makes the file an MVC action, which means that it can handle requests. `@page` must be the first Razor directive on a page.

`@page` and `@model` are examples of transitioning into Razor-specific markup. See Razor syntax for more information.

#### @model directive
The `@model` directive specifies the type of the model passed to the Razor Page. The `@model` line makes the `PageModel`-derived class available to the Razor Page. 
```c#
@page
@model RazorPagesMovie.Pages.Movies.IndexModel
```

### Layout Page
The menu layout is implemented in the Pages/Shared/_Layout.cshtml file.

[Layout](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/layout?view=aspnetcore-5.0) templates allow the HTML container layout to be:
- Specified in one place.
- Applied in multiple pages in the site.

The `@RenderBody()` line is a placeholder where all the page-specific views show.

### ViewData and layout
`ViewData` dictionary property that can be used to pass data to a View. Objects are added to the `ViewData` dictionary using a key value pattern.
```c#
@page
@model RazorPagesMovie.Pages.Movies.IndexModel

@{
    ViewData["Title"] = "Index";
}
```

The `Title` property is used in the Pages/Shared/_Layout.cshtml file. The following markup shows the first few lines of the _Layout.cshtml file.
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - RazorPagesMovie</title>

    @*Markup removed for brevity.*@
```

#### comments
The line `@*Markup removed for brevity.*@` is a Razor comment. Unlike HTML comments `<!-- -->`, Razor comments are not sent to the client. See [MDN web docs: Getting started with HTML](https://developer.mozilla.org/docs/Learn/HTML/Introduction_to_HTML/Getting_started#HTML_comments) for more information.

## Database
The `RazorPagesMovieContext` object handles the task of connecting to the database and mapping `Movie` objects to database records. The database context is registered with the [Dependency Injection][3] container in the `ConfigureServices` method in Startup.cs:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddRazorPages();

    services.AddDbContext<RazorPagesMovieContext>(options =>
      options.UseSqlServer(Configuration.GetConnectionString("RazorPagesMovieContext")));
}
```
The ASP.NET Core Configuration system reads the ConnectionString key. For local development, configuration gets the connection string from the appsettings.json file.
```json
"ConnectionStrings": {
    "RazorPagesMovieContext": "Server=(localdb)\\mssqllocaldb;Database=RazorPagesMovieContext-bc;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
When the app is deployed to a test or production server, an environment variable can be used to set the connection string to a test or production database server. For more information, see [Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0).

### SQL Server Express LocalDB
LocalDB is a lightweight version of the SQL Server Express database engine that's targeted for program development. LocalDB starts on demand and runs in user mode, so there's no complex configuration.
By default, LocalDB database creates *.mdf files in the C:\Users\<user>\ directory.

## Validation
The validation rules are enforced any time a user creates or edits a movie.

A key tenet of software development is called DRY ("Don't Repeat Yourself"). Razor Pages encourages development where functionality is specified once, and it's reflected throughout the app. DRY can help:
- Reduce the amount of code in an app.
- Make the code less error prone, and easier to test and maintain.

The validation support provided by Razor Pages and Entity Framework is a good example of the DRY principle:
- Validation rules are declaratively specified in one place, in the model class.
- Rules are enforced everywhere in the app.

Attribute Examples:
| Attribute | Description |
| ----------- | ----------- |
| [Required] | Specifies that a data field value is required. |
| [MinimumLength] | Specifies the minimum length of array or string data allowed in a property. |
| [RegularExpression] | Specifies that a data field value in ASP.NET Dynamic Data must match the specified regular expression.|
| [Range] | Specifies the numeric range constraints for the value of a data field. |
| [StringLength] | Specifies the minimum and maximum length of characters that are allowed in a data field. |
[More about Data Annotations][4]

### Validation Error UI in Razor Pages
 The errors are enforced both client-side, using JavaScript and jQuery, and server-side, when a user has JavaScript disabled. A significant benefit is that no code changes were necessary in the Create or Edit pages. Once data annotations were applied to the model, the validation UI was enabled.
 
 
 The form data isn't posted to the server until there are no client-side validation errors. The [Input Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-5.0) uses the [DataAnnotations][4] attributes and produces HTML attributes needed for jQuery Validation on the client-side. The [Validation Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-5.0#the-validation-tag-helpers) displays validation errors. See [Validation](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0) for more information.
 
 
 When validation logic needs to change, it's done only in the model. Validation is applied consistently throughout the application, validation logic is defined in one place. Validation in one place helps keep the code clean, and makes it easier to maintain and update.
 
 ### DataType Attributes
 The `[DataType]` attributes:
- Give hints for the view engine to format the data.
- Supplies attributes such as <a> for URL's and <a href="mailto:EmailAddress.com"> for email.
- Can provide a date selector DataType.Date in browsers that support HTML5.
- Emit HTML 5 data-, pronounced "data dash", attributes that HTML 5 browsers consume.
- Do not provide any validation.

The `[DataType]` attribute is used to specify a data type that's more specific than the database intrinsic type. `[DataType]` attributes aren't validation attributes. In the sample application, only the date is displayed, without time.

The DataType enumeration provides many data types, such as Date, Time, PhoneNumber, Currency, EmailAddress, and more.

[1]: https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/razor-pages-start?view=aspnetcore-5.0&tabs=visual-studio
[2]: https://docs.microsoft.com/en-us/ef/core/
[3]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0
[4]: https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0
