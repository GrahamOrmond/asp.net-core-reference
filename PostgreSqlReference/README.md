ASP.NET Web API With PostgreSQL
Personal referece for asp.net with PostgreSQL
README copied from [PostgreSQL](https://www.postgresqltutorial.com/) and [Npgsql Documentation](https://www.npgsql.org/index.html)

## Install Postgres on Windows

PostgreSQL was developed for UNIX-like platforms, however, it was designed to be portable. It means that PostgreSQL can also run on other platforms such as macOS, Solaris, and Windows.

There are three steps to complete the PostgreSQL installation:
#### 1. Download  [PostgreSQL EnterpriseDB installer](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads) for Windows.
   
#### 2. Install PostgreSQL

To install PostgreSQL on Windows, you need to have administrator privileges.

Select software components to install:
- The PostgreSQL Server to install the PostgreSQL database server
- pgAdmin 4 to install the PostgreSQL database GUI management tool.
- Command Line Tools to install command-line tools such as psql, pg_restore, etc. These tools allow you to interact with the PostgreSQL database server using the command-line interface.
- Stack Builder provides a GUI that allows you to download and install drivers that work with PostgreSQL.

Enter the password for the database superuser (postgres)

PostgreSQL runs as a service in the background under a service account named `postgres`. If you already created a service account with the name `postgres`, you need to provide the password of that account in the following window.

Enter a port number on which the PostgreSQL database server will listen. The default port of PostgreSQL is 5432. You need to make sure that no other applications are using this port.

Choose the default locale used by the PostgreSQL database. If you leave it as default locale, PostgreSQL will use the operating system locale. After that click the Next button.

#### 3. Verify the installation

There are several ways to verify the PostgreSQL installation. You can try to [connect to the PostgreSQL](https://www.postgresqltutorial.com/connect-to-postgresql-database/) database server from any client application e.g.,  psql and pgAdmin.

The quick way to verify the installation is through the psql program.

First, click the psql application to launch it. The psql command-line program will display.

Second, enter all the necessary information such as the server, database, port, username, and password. To accept the default, you can press Enter.  Note that you should provide the password that you entered during installing the PostgreSQL.

Third, issue the command SELECT version(); you will see the following output:

## Connect to PostgreSQL Database Server

#### 1) Connect to PostgreSQL database server using psql

psql is an interactive terminal program provided by PostgreSQL. It allows you to interact with the PostgreSQL database server such as executing SQL statements and managing database objects.

The following steps show you how to connect to the PostgreSQL database server via the psql program:

1. launch the psql program and connect to the PostgreSQL Database Server using the postgres user:
2. enter all the information such as Server, Database, Port, Username, and Password. If you press Enter, the program will use the default value specified in the square bracket [] and move the cursor to the new line.
3. interact with the PostgreSQL Database Server by issuing an SQL statement. The following statement returns the current version of PostgreSQL: `SELECT version();`

Please do not forget to end the statement with a semicolon (;). After pressing Enter, psql will return the current PostgreSQL version on your system.

#### 2) Connect to PostgreSQL database server using pgAdmin

The second way to connect to a database is by using a pgAdmin application. The pgAdmin application allows you to interact with the PostgreSQL database server via an intuitive user interface.

The following illustrates how to connect to a database using pgAdmin GUI application:

1. Launch the pgAdmin application.
a. The pgAdmin application will launch on the web browser
2. Right-click the Servers node and select **Create** > **Server...** menu to create a server
3. Enter the server name e.g., **PostgreSQL** and click the **Connection** tab
4. Enter the host and password for the **postgres** user and click the **Save** button:
5. Click on the Servers node to expand the server. By default, PostgreSQL has a database named postgres
6. Open the query tool by choosing the menu item **Tool** > **Query Tool** or click the lightning icon.
7. Enter the query in the **Query Editor**, click the **Execute** button, you will see the result of the query displaying in the **Data Output** tab: `SELECT version();`

## Setup ASP.Net Project

Npgsql has an Entity Framework (EF) Core provider. It behaves like other EF Core providers (e.g. SQL Server), so the [general EF Core docs](https://docs.microsoft.com/ef/core/index) apply here as well.

Development happens in the [Npgsql.EntityFrameworkCore.PostgreSQL](https://github.com/npgsql/Npgsql.EntityFrameworkCore.PostgreSQL) repository, all issues should be reported there.

### Configuring the project file

To use the Npgsql EF Core provider, add a dependency on Npgsql.EntityFrameworkCore.PostgreSQL. You can follow the instructions in the general EF Core Getting Started docs.
1. Go to **Tools** > **NuGet Package Manager** > **NuGet Package Manager for solution**
2. Select the **Browse** tab and search for `Npgsql.EntityFrameworkCore.PostgreSQL`
3. Select the package and install

### Add Models
1. Right-click solution > **Add** > **New Folder** name it *Models*
2. Right click *Models* folder > **Add** > **Class..**

Example Blog Model
```c#
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; set; }
}
```

Example Post Model
```
public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```


### Define a DBContext

Create the DBContext file
1. Right-click solution > **Add** > **New Folder** name it *Data*
2. Right click *Data* folder > **Add** > **Class..**

The following code outlines the DBContext
```c#
using Microsoft.EntityFrameworkCore;

namespace PostgreSqlReference.Models
{
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
        : base(options)
        { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
```
### Add a connection string to appsettings.json
```json
"ConnectionStrings": {
    "BloggingContext": "Host=localhost;Database=Blog;Username=postgres;Password=admin"
}
```

### Configure Startup.cs

Inside of `ConfigureServices` you must declare the dependency injection
```c#
public void ConfigureServices(IServiceCollection services)
{
    // Other DI initializations

    services.AddDbContext<BloggingContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("BloggingContext")));
}
```

### Migrations

Install `Microsoft.EntityFrameworkCore.Design`
1. Go to **Tools** > **NuGet Package Manager** > **NuGet Package Manager for solution**
2. Select the **Browse** tab and search for `Microsoft.EntityFrameworkCore.Tools`
3. Select the package and install

Initial migration
1. From the Tools menu, select NuGetPackage Manager > Package Manager Console
2. In the PMC, enter the following commands: Add-Migration InitialCreate Update-Database



