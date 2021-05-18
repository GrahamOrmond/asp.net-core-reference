# Asp.net Core Web App
reference for .net web app with razor pages
Tutorials done through *[Get started with Razor Pages in ASP.NET Core][1]*

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

[more DataAnnotations here](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0)

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
- Startup.cs - (registers [dependency injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0))

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


[1]: https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/razor-pages-start?view=aspnetcore-5.0&tabs=visual-studio
[2]: https://docs.microsoft.com/en-us/ef/core/
