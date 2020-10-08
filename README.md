# Auth Micoservice

> Authorization server built on ASP.NET Identity and IdentityServer4
* Has permission based authorizaation.
* Integrated with Entity Framework
* Has api endpoints with cookie authentication and csrf protection for SPAs.
* Has api endpoints with JWT bearer authorization and no csrf protection for native clients and servers.
* Comprehensive Swagger documentation.
* Employs best practices such as SOLID and onion architecture.


### Build Setup ###

* Ensure you have installed [dotnet 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1), [Entity Framework tools](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet) and [Microsoft SQL Server](https://docs.microsoft.com/en-us/sql/database-engine/install-windows/install-sql-server) at least developer edition.
* Open the Web folder of the solution, copy appsettings.example.json and save as appsettings.json. Set the DefaultConnection field in the ConnectionStrings section of appsettings.json to point to a database for the app in Microsoft SQL Server. Set your desired value for other fields in appsettings.json
* From your CLI, navigate to Infrastructure folder of the solution and run the following commands:

```bash
# run migrations
$ dotnet ef database update --startup-project ../Web/Web.csproj  --context  DbContext
$ dotnet ef database update --startup-project ../Web/Web.csproj  --context  PersistedGrantDbContext
$ dotnet ef database update --startup-project ../Web/Web.csproj  --context  ConfigurationDbContext

# serve at http://localhost:5000 and https://localhost:5001
$ dotnet run --project ../Web/Web.csproj
```

* Open https://localhost:5001/.well-known/openid-configuration in your browser to see openid configuration.
* In Microsoft SQL Server, enable [full text search](https://docs.microsoft.com/en-us/sql/relational-databases/search/get-started-with-full-text-search)
 on NormalizedUserName, NormlizedEmail and PhoneNumber columns of Users table, also on NormalizedName column of Roles and Permissions tables.


### In Progress ###
* Add admin API endpoints for manging users, roles, permissions, api clients, resources and scopes.
* Add API endpoints for users to manage their profile.


### Who do I talk to? ###

*  [Muyiwa Awoniyi](mailto:muyiwaawoniyi@yahoo.com)