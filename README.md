# Auth0 VSTO Excel 2016 Add-in

This sample demonstrates how to build a VSTO Excel 2016 add-in (C#).  This sample creates a Ribbon with a single button.  When the user clicks the button a web browser will open redirecting the user the `/authiorize` endpoint.  Once the user completes login the code and code verifier will be returned and exchanged for tokens.  This sample uses Auth0 as an authentication and authorization server using [Authorization Code Grant Flow with PKCE](https://auth0.com/docs/api-auth/tutorials/authorization-code-grant-pkce).

A special note about Excel.  If you are migrating from the legacy (deprecated) SDK [Auth0.WinformsOrWPF](https://github.com/auth0/Auth0.WinformsWPF) there is a significant breaking change.  In Excel the `System.Threading.SynchronizationContext.Current` is set to `null`.  With the old SDK this was not problematic.  However, in the new SDK `Auth0.OidcClient.WinForms` takes on a dependency to `IdentityModel.OidcClient2`.  This SDK does a lot asynchronous calls with `await/async` and utilizes `ConfigureAwait(false)`.  At some point the SDK makes a call to `System.Net.Http.HttpClient.GetUrl(string)`.  This class is thread safe and when used will run on a new thread in a mutli-tenant apartment.  After the call is made .NET will look to see if the `SynchronizationContext` is not null and use that for swithing the current thread.  If .NET finds this as `null` it falls back to using the `ThreadScheduler`.  In the case of ThreadScheduler the remaining method calls will continue on the MTA thread.  Eventually the method attempts to invoke the `WebBrowser` object and this fails, because it requies to be invoked in a single-threaded apartment.    The following example solves this by setting up a `SynchronizationContext` before invoking login.


## Setup

### Prerequisites

The following tools are required to build and run this sample.

1. Excel 2016
2. Visual Studio 2016 Community Edition or better
3. VSTO Visual Studio Tools
4. .NET 4.6.1

### Setting You Tenant

After you have a working development environment you must setup your Auth0 tenant.  We will need to setup a new client with appropriate settings.

1. Open manage and go to [Clients](https://manage.auth0.com/#/clients)
2. Click on the _CREATE CLIENT_ button.
3. Give the client a name, select _Native_ application, and click _CREATE_
4. Add `https:{your-tenant}.auth0.com/mobile` to the _Allowed Callback URLs_
5. At the bottom click on the _Show Advanced Settings_ url and the click on the _OAuth_ link.
6. Ensure _OIDC Conformant_ option is selected
7. Ensure the _JsonWebToken Signature Algorithm_ is `RS256`

### Configuring the Application

Setting up the add-in is very simple.  Now that you have created a new client you will need two things, the `client_id` and the auth0 domain (e.g. your-tenant.auth0.com).  After you have these you can copy them into the `app.config` of `Auth0.ExcelAddin`.

```
<add key="Auth0ClientId" value="{your-client-id}"/>
<add key="Auth0Domain" value="{your-auth0-domain}"/>
```

After upating these settings hit F5 and you are ready to go.  By default the login burtton will be in the `Add-in` ribbon.  Click the login button and a new window will open redirect the user to the `/authorize` endpoint to begin authentication.

## Doing More With The Extension

This extension simply does a standard authentication.  You can incorporate and audience for authorization, do single sign on, or more by updating the action in the button action for the `btnLogin`.

---

## What is Auth0?

Auth0 helps you to:

* Add authentication with [multiple authentication sources](https://docs.auth0.com/identityproviders), either social like **Google, Facebook, Microsoft Account, LinkedIn, GitHub, Twitter, Box, Salesforce, amont others**, or enterprise identity systems like **Windows Azure AD, Google Apps, Active Directory, ADFS or any SAML Identity Provider**.
* Add authentication through more traditional **[username/password databases](https://docs.auth0.com/mysql-connection-tutorial)**.
* Add support for **[linking different user accounts](https://docs.auth0.com/link-accounts)** with the same user.
* Support for generating signed [Json Web Tokens](https://docs.auth0.com/jwt) to call your APIs and **flow the user identity** securely.
* Analytics of how, when and where users are logging in.
* Pull data from other sources and add it to the user profile, through [JavaScript rules](https://docs.auth0.com/rules).

## Create a free account in Auth0

1. Go to [Auth0](https://auth0.com) and click Sign Up.
2. Use Google, GitHub or Microsoft Account to login.

## Issue Reporting

If you have found a bug or if you have a feature request, please report them at this repository issues section. Please do not report security vulnerabilities on the public GitHub issue tracker. The [Responsible Disclosure Program](https://auth0.com/whitehat) details the procedure for disclosing security issues.

## Author

[Auth0](https://auth0.com)

## License

This project is licensed under the MIT license. See the [LICENSE](LICENSE.txt) file for more info.
