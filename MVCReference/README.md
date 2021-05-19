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



