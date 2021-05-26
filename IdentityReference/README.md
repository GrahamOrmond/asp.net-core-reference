ASP.NET Core Identification Reference
Personal referece for asp.net identity
**README copied from [ASP.NET Core authentication docs](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-5.0)**

## Overview

ASP.NET Core Identity:

- Is an API that supports user interface (UI) login functionality.
- Manages users, passwords, profile data, roles, claims, tokens, email confirmation, and more.
Users can create an account with the login information stored in Identity or they can use an external login provider. Supported external login providers include [Facebook, Google, Microsoft Account, and Twitter](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-5.0).

[Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-5.0)
- The process of determining a user's identity.

[Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction?view=aspnetcore-5.0)
- the process of determining whether a user has access to a resource.

 In ASP.NET Core, authentication is handled by the `IAuthenticationService`, which is used by authentication [middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0).

The authentication service uses registered authentication handlers to complete authentication-related actions. Examples of authentication-related actions include:
- Authenticating a user.
- Responding when an unauthenticated user tries to access a restricted resource.

The registered authentication handlers and their configuration options are called "schemes".

Authentication schemes are specified by registering authentication services in `Startup.ConfigureServices`:
- By calling a scheme-specific extension method after a call to `services.AddAuthentication` (such as `AddJwtBearer` or `AddCookie`, for example). These extension methods use [AuthenticationBuilder.AddScheme][1] to register schemes with appropriate settings.
- Less commonly, by calling [AuthenticationBuilder.AddScheme][1] directly.

[1]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationbuilder.addscheme

#### Authentication concepts
Authentication is responsible for providing the [ClaimsPrincipal](https://docs.microsoft.com/en-us/dotnet/api/system.security.claims.claimsprincipal) for authorization to make permission decisions against. There are multiple authentication scheme approaches to select which authentication handler is responsible for generating the correct set of claims:
- [Authentication scheme](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-5.0#authentication-scheme)
- The default authentication scheme.
- Directly set [HttpContext.User](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext.user#Microsoft_AspNetCore_Http_HttpContext_User).

There is no automatic probing of schemes. If the default scheme is not specified, the scheme must be specified in the authorize attribute, otherwise, an error is thrown.

#### Authentication scheme

The authentication scheme can select which authentication handler is responsible for generating the correct set of claims. more information on [Authorize with a specific scheme](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-5.0).

An authentication scheme is a name which corresponds to:
- An authentication handler.
- Options for configuring that specific instance of the handler.

When configuring authentication, it's common to specify the default authentication scheme. The default scheme is used unless a resource requests a specific scheme. It's also possible to:
- Specify different default schemes to use for authenticate, challenge, and forbid actions.
- Combine multiple schemes into one using policy schemes.

#### Authentication handler

An authentication handler:
- Is a type that implements the behavior of a scheme.
- Is derived from [IAuthenticationHandler](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.iauthenticationhandler) or [AuthenticationHandler<TOptions>](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhandler-1).
- Has the primary responsibility to authenticate users.

Based on the authentication scheme's configuration and the incoming request context, authentication handlers:
- Construct [AuthenticationTicket](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationticket) objects representing the user's identity if authentication is successful.
- Return 'no result' or 'failure' if authentication is unsuccessful.
- Have methods for challenge and forbid actions for when users attempt to access resources:
-- They are unauthorized to access (forbid).
-- When they are unauthenticated (challenge).

#### Authenticate
An authentication scheme's authenticate action is responsible for constructing the user's identity based on request context. It returns an [AuthenticateResult](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticateresult) indicating whether authentication was successful and, if so, the user's identity in an authentication ticket. See [AuthenticateAsync](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhttpcontextextensions.authenticateasync). Authenticate examples include:

A cookie authentication scheme constructing the user's identity from cookies.
A JWT bearer scheme deserializing and validating a JWT bearer token to construct the user's identity.

#### Challenge 
An authentication challenge is invoked by Authorization when an unauthenticated user requests an endpoint that requires authentication. Authorization invokes a challenge using the specified authentication scheme(s), or the default if none is specified. See [ChallengeAsync](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhttpcontextextensions.challengeasync).

Authentication challenge examples include:
- A cookie authentication scheme redirecting the user to a login page.
- A JWT bearer scheme returning a 401 result with a www-authenticate: bearer header.

A challenge action should let the user know what authentication mechanism to use to access the requested resource.

#### Forbid
An authentication scheme's forbid action is called by Authorization when an authenticated user attempts to access a resource they are not permitted to access. See [ForbidAsync](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhttpcontextextensions.forbidasync).

Authentication forbid examples include:
- A cookie authentication scheme redirecting the user to a page indicating access was forbidden.
- A JWT bearer scheme returning a 403 result.
- A custom authentication scheme redirecting to a page where the user can request access to the resource.

A forbid action can let the user know:
- They are authenticated.
- They aren't permitted to access the requested resource.

