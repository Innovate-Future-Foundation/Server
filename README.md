# Server
Backend Server code

# Database setup:
** Modify your local database configuration in file "src\InnovateFuture.Api\appsettings.Development.json"
** Run migration command `dotnet ef database update --project "fullpath to InnovateFuture.Infrastructure.csproj" --startup-project fullpath to InnovateFuture.Api.csproj`

# Deploy Locally
You can deploy the backend and the database by simply `docker compose up`, here's how you can achieve that.
## Configure your Env vars
Copy file `.env.example` to `.env` and do your settings.
```properties
DB_HOST=postgres                # Don't modify if you don't understand how hostname works in docker
DB_PORT=5432                    
DB_NAME=InnovateFuture
DB_USER=db_admin                # The admin username of your postgreSQL
DB_PASS=123321abb               # The admin password ...
PG_USER=db@inff.com             # PgAdmin web client username  https://localhost:5050
PG_PASS=q4hbGe7N3EAy            # PgAdmin password ...

JWT_SECRET=u68-03Pn4n_@w@fM
DEP_ENV=Development             # Env flag
```
> Don't copy from the above code block, comments are not supported in properties file
## Build
You only need to rebuild your docker images if there are **changes applied**, otherwise if you just want to fire up your local backend & db set just go `docker compose up`
```bash
docker compose build
```
## Dockerfiles Explanation
... todo
