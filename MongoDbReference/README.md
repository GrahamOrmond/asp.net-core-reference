# ASP.NET Web API with MongoDB
personal reference for asp.net web api using MongoDB

**ReadMe copied from  microsft documentation *[Create a web API with ASP.NET Core and MongoDB](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-5.0&tabs=visual-studio)* and [MongoDB documentation](https://docs.mongodb.com/manual/introduction/)**

## MongoDB Windows Installation
[MongoDB install guide here](https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/)

1. Download MongoDB [here](https://www.mongodb.com/try/download/community?tck=docs_server)
2. Run the installer and select default setting
a. *Install MongoD as a service*
b. *Run service as Network Service user*
c. *Install MongoDB Compass (optional)*

### Installed MongoDB as a Windows Service
The MongoDB service is started upon successful installation 

To begin using MongoDB, connect a [mongo.exe](https://docs.mongodb.com/manual/reference/program/mongo/#mongodb-binary-bin.mongo) shell to the running MongoDB instance. Either:

From Windows Explorer/File Explorer, go to `C:\Program Files\MongoDB\Server\4.4\bin\` directory and double-click on [mongo.exe](https://docs.mongodb.com/manual/reference/program/mongo/#mongodb-binary-bin.mongo).
Or, open a Command Interpreter with Administrative privileges and run:
```
"C:\Program Files\MongoDB\Server\4.4\bin\mongo.exe"
```

### Start MongoDB Service

To start/restart the MongoDB service, use the Services console:
1. From the Services console, locate the MongoDB service.
2. Right-click on the MongoDB service and click **Start**.

To begin using MongoDB, connect a [mongo.exe](https://docs.mongodb.com/manual/reference/program/mongo/#mongodb-binary-bin.mongo) shell to the running MongoDB instance. To connect, open a **Command Interpreter** with Administrative privileges and run:
```
"C:\Program Files\MongoDB\Server\4.4\bin\mongo.exe"
```

### Stop MongoDB Service
To stop/pause the MongoDB service, use the Services console:

1. From the Services console, locate the MongoDB service.
2. Right-click on the MongoDB service and click **Stop** (or **Pause**).

### Remove MongoDB Service
To remove the MongoDB service, first use the Services console to stop the service. Then open a Windows [command prompt/interpreter](https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/cmd) (*cmd.exe*) as an **Administrator**, and run the following command:
```
sc.exe delete MongoDB
```

### Run MongoDB from the Command Interpreter

You can run MongoDB Community Edition from the [Windows command prompt/interpreter](https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/cmd) (*cmd.exe*) instead of as a service.

Open a Windows command prompt/interpreter (*cmd.exe*) as an *Administrator*.

#### 1. Create database directory.
Create the data directory where MongoDB stores data. MongoDB's default data directory path is the absolute path \data\db on the drive from which you start MongoDB.

From the Command Interpreter, create the data directories:
```
cd C:\
md "\data\db"
```

#### 2. Start your MongoDB database.
To start MongoDB, run [exe](https://docs.mongodb.com/manual/reference/program/mongod.exe/#mongodb-binary-bin.mongod.exe).
```
"C:\Program Files\MongoDB\Server\4.4\bin\mongod.exe" --dbpath="c:\data\db"
```
The [--dbpath](https://docs.mongodb.com/manual/reference/program/mongod/#std-option-mongod.--dbpath) option points to your database directory.

If the MongoDB database server is running correctly, the *Command Interpreter* displays:
```
[initandlisten] waiting for connections
```
#### 3. Connect to MongoDB.

To connect a [mongo.exe](https://docs.mongodb.com/manual/reference/program/mongo/#mongodb-binary-bin.mongo) shell to the MongoDB instance, open another **Command Interpreter** with Administrative privileges and run:

```
"C:\Program Files\MongoDB\Server\4.4\bin\mongo.exe"
```

### Additional Considerations
#### Localhost Binding by Default

By default, MongoDB launches with [bindIp](https://docs.mongodb.com/manual/reference/configuration-options/#mongodb-setting-net.bindIp) set to 127.0.0.1, which binds to the localhost network interface.
This means that the mongod.exe can only accept connections from clients that are running on the same machine.
Remote clients will not be able to connect to the mongod.exe, and the mongod.exe will not be able to initialize a [replica](https://docs.mongodb.com/manual/reference/glossary/#std-term-replica-set) set unless this value is set to a valid network interface.


This value can be configured either:
- in the MongoDB configuration file with bindIp, or
- via the command-line argument --bind_ip

#### WARNING
Before binding to a non-localhost (e.g. publicly accessible) IP address, ensure you have secured your cluster from unauthorized access. For a complete list of security recommendations, see [Security Checklist](https://docs.mongodb.com/manual/administration/security-checklist/). At minimum, consider [enabling authentication](https://docs.mongodb.com/manual/administration/security-checklist/#std-label-checklist-auth) and [hardening network infrastructure](https://docs.mongodb.com/manual/core/security-hardening/).

For more information on configuring [bindIp](https://docs.mongodb.com/manual/reference/configuration-options/#mongodb-setting-net.bindIp), see [IP Binding](https://docs.mongodb.com/manual/core/security-mongodb-configuration/).

### Point Releases and .msi

If you installed MongoDB with the Windows installer (*.msi*), the *.msi* automatically upgrades within its [release series](https://docs.mongodb.com/manual/reference/versioning/#std-label-release-version-numbers) (e.g. 4.2.1 to 4.2.2).

### Add MongoDB binaries to the System PATH

All command-line examples in this tutorial are provided as absolute paths to the MongoDB binaries. You can add **C:\Program Files\MongoDB\Server\4.4\bin** to your System **PATH** and then omit the full path to the MongoDB binaries.


## Add MongoDB to ASP.NET Project
Visit the [uGet Gallery: MongoDB.Driver](https://www.nuget.org/packages/MongoDB.Driver/) to determine the latest stable version of the .NET driver for MongoDB. 

In the **Package Manager Console** window, navigate to the project root. Run the following command to install the .NET driver for MongoDB:
```PowerShell
Install-Package MongoDB.Driver -Version {VERSION}
```

### Add an entity model
1. Add a *Models* directory to the project root.
2. Add a `Book` class to the Models directory with the following code:
```c#
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BooksApi.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string BookName { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }
    }
}
```

In the preceding class, the Id property:
- Is required for mapping the **Common Language Runtime (CLR)** object to the MongoDB collection.
- Is annotated with [[BsonId]](https://api.mongodb.com/csharp/current/html/T_MongoDB_Bson_Serialization_Attributes_BsonIdAttribute.htm) to designate this property as the document's primary key.
- Is annotated with [[BsonRepresentation(BsonType.ObjectId)]](https://api.mongodb.com/csharp/current/html/T_MongoDB_Bson_Serialization_Attributes_BsonRepresentationAttribute.htm) to allow passing the parameter as type `string` instead of an [ObjectId](https://api.mongodb.com/csharp/current/html/T_MongoDB_Bson_ObjectId.htm) structure. Mongo handles the conversion from `string` to `ObjectId`.

The `BookName` property is annotated with the [[BsonElement]](https://api.mongodb.com/csharp/current/html/T_MongoDB_Bson_Serialization_Attributes_BsonElementAttribute.htm) attribute. The attribute's value of `Name` represents the property name in the MongoDB collection.

### Add a configuration model

1. Add the following database configuration values to appsettings.json:
```json
{
  "BookstoreDatabaseSettings": {
    "BooksCollectionName": "Books",
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "BookstoreDb"
  },
  "Logging": {
    ...
```

2. Add a BookstoreDatabaseSettings.cs file to the Models directory with the following code:
```c#
namespace BooksApi.Models
{
    public class BookstoreDatabaseSettings : IBookstoreDatabaseSettings
    {
        public string BooksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBookstoreDatabaseSettings
    {
        string BooksCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
```
The preceding `BookstoreDatabaseSettings` class is used to store the *appsettings.json* file's `BookstoreDatabaseSettings` property values. The JSON and C# property names are named identically to ease the mapping process.

3. Add the following code to Startup.ConfigureServices:
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<BookstoreDatabaseSettings>(
        Configuration.GetSection(nameof(BookstoreDatabaseSettings)));

    // requires using Microsoft.Extensions.Options
    services.AddSingleton<IBookstoreDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);
```
In the preceding code:

- The configuration instance to which the *appsettings.json* file's `BookstoreDatabaseSettings` section binds is registered in the **Dependency Injection (DI) container**. For example, a `BookstoreDatabaseSettings` object's `ConnectionString` property is populated with the `BookstoreDatabaseSettings:ConnectionString` property in *appsettings.json*.
- The `IBookstoreDatabaseSettings` interface is registered in DI with a singleton service lifetime. When injected, the interface instance resolves to a BookstoreDatabaseSettings object.

### Add a CRUD operations service

1. Add a *Services* directory to the project root.

2. Add a `BookService` class to the *Services* directory with the following code:
```
using BooksApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace BooksApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) => 
            _books.DeleteOne(book => book.Id == id);
    }
}
```
In the preceding code, an `IBookstoreDatabaseSettings` instance is retrieved from DI via constructor injection. This technique provides access to the appsettings.json configuration values that were added in the [Add a configuration model](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-5.0&tabs=visual-studio#add-a-configuration-model) section.

3. Add the following highlighted code to `Startup.ConfigureServices`:

```c#
public void ConfigureServices(IServiceCollection services)
{
    // place under database configuration model
    services.AddSingleton<BookService>();
```

In the preceding code, the `BookService` class is registered with DI to support constructor injection in consuming classes. The singleton service lifetime is most appropriate because `BookService` takes a direct dependency on `MongoClient`. Per the official [Mongo Client reuse guidelines](https://mongodb.github.io/mongo-csharp-driver/2.8/reference/driver/connecting/#re-use), `MongoClient` should be registered in DI with a singleton service lifetime.


The BookService class uses the following MongoDB.Driver members to perform CRUD operations against the database:
- MongoClient: Reads the server instance for performing database operations. The constructor of this class is provided the MongoDB connection string:
```c#
public BookService(IBookstoreDatabaseSettings settings)
{
    var client = new MongoClient(settings.ConnectionString);
    var database = client.GetDatabase(settings.DatabaseName);

    _books = database.GetCollection<Book>(settings.BooksCollectionName);
}
```c#
MongoClient: Reads the server instance for performing database operations. The constructor of this class is provided the MongoDB connection string:

```c#
public BookService(IBookstoreDatabaseSettings settings)
{
    var client = new MongoClient(settings.ConnectionString);
    ...
```

- [IMongoDatabase](https://api.mongodb.com/csharp/current/html/T_MongoDB_Driver_IMongoDatabase.htm): Represents the Mongo database for performing operations. This tutorial uses the generic [GetCollection<TDocument>(collection)](https://api.mongodb.com/csharp/current/html/M_MongoDB_Driver_IMongoDatabase_GetCollection__1.htm) method on the interface to gain access to data in a specific collection. Perform CRUD operations against the collection after this method is called. In the `GetCollection<TDocument>(collection)` method call:
-- `collection` represents the collection name.
-- `TDocument` represents the CLR object type stored in the collection.

`GetCollection<TDocument>(collection)` returns a [MongoCollection](https://api.mongodb.com/csharp/current/html/T_MongoDB_Driver_MongoCollection.htm) object representing the collection. In this tutorial, the following methods are invoked on the collection:

- [DeleteOne](https://api.mongodb.com/csharp/current/html/M_MongoDB_Driver_IMongoCollection_1_DeleteOne.htm): Deletes a single document matching the provided search criteria.
- [Find<TDocument>](https://api.mongodb.com/csharp/current/html/M_MongoDB_Driver_IMongoCollectionExtensions_Find__1_1.htm): Returns all documents in the collection matching the provided search criteria.
- [InsertOne](https://api.mongodb.com/csharp/current/html/M_MongoDB_Driver_IMongoCollection_1_InsertOne.htm): Inserts the provided object as a new document in the collection.
- [ReplaceOne](https://api.mongodb.com/csharp/current/html/M_MongoDB_Driver_IMongoCollection_1_ReplaceOne.htm): Replaces the single document matching the provided search criteria with the provided object.

### Add a controller

Add a `BooksController` class to the Controllers directory with the following code:
```c#
using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<Book>> Get() =>
            _bookService.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            _bookService.Create(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);

            return NoContent();
        }
    }
}
```
The preceding web API controller:

- Uses the `BookService` class to perform CRUD operations.
- Contains action methods to support GET, POST, PUT, and DELETE HTTP requests.
- Calls [CreatedAtRoute](https://docs.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller.createdatroute) in the `Create` action method to return an [HTTP 201](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html) response. Status code 201 is the standard response for an HTTP POST method that creates a new resource on the server. `CreatedAtRoute` also adds a `Location` header to the response. The `Location` header specifies the URI of the newly created book.

### Test the web API

1. Build and run the app.
2. Navigate to `http://localhost:<port>/api/books` to test the controller's parameterless Get action method. Json is displayed for the elements in the database
3. Navigate to `http://localhost:<port>/api/books/{id here}` to test the controller's overloaded Get action method.

### Configure JSON serialization options

There are two details to change about the JSON responses returned in the [Test the web API](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-5.0&tabs=visual-studio#test-the-web-api) section:
- The property names' default camel casing should be changed to match the Pascal casing of the CLR object's property names.
- The `bookName` property should be returned as `Name`.

To satisfy the preceding requirements, make the following changes:
1. JSON.NET has been removed from ASP.NET shared framework. Add a package reference to [Microsoft.AspNetCore.Mvc.NewtonsoftJson](https://nuget.org/packages/Microsoft.AspNetCore.Mvc.NewtonsoftJson).

2. In `Startup.ConfigureServices`, chain the following highlighted code on to the `AddControllers` method call:

```c#
public void ConfigureServices(IServiceCollection services)
{
    ...
    
    services.AddControllers()
        .AddNewtonsoftJson(options => options.UseMemberCasing());
}
```
With the preceding change, property names in the web API's serialized JSON response match their corresponding property names in the CLR object type. For example, the `Book` class's `Author` property serializes as `Author`.

3. In Models/Book.cs, annotate the BookName property with the following [[JsonProperty]](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonPropertyAttribute.htm) attribute:
```c#
[BsonElement("Name")]
[JsonProperty("Name")]
public string BookName { get; set; }
```
The `[JsonProperty]` attribute's value of `Name` represents the property name in the web API's serialized JSON response.

5. Repeat the steps defined in the Test the web API section. Notice the difference in JSON property names.


