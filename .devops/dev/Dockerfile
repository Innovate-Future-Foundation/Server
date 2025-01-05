FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5091 7173

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InnovateFuture.sln", "."]
COPY ["src/InnovateFuture.Api/InnovateFuture.Api.csproj", "InnovateFuture.Api/InnovateFuture.Api.csproj"]
COPY ["src/InnovateFuture.Application/InnovateFuture.Application.csproj", "InnovateFuture.Application/InnovateFuture.Application.csproj"]
COPY ["src/InnovateFuture.Domain/InnovateFuture.Domain.csproj", "InnovateFuture.Domain/InnovateFuture.Domain.csproj"]
COPY ["src/InnovateFuture.Infrastructure/InnovateFuture.Infrastructure.csproj", "InnovateFuture.Infrastructure/InnovateFuture.Infrastructure.csproj"]
RUN dotnet restore "InnovateFuture.Api/InnovateFuture.Api.csproj"
WORKDIR /
COPY . .
WORKDIR /src/InnovateFuture.Api
RUN dotnet build "InnovateFuture.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InnovateFuture.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV DOTNET_SYSTEM_CONSOLE_ALLOW_ANSI_COLOR_REDIRECTION=1
ENV ConnectionStrings__DBConnection=Server=postgres;Database=InnovateFuture;User=admin;Password=root
ENV JWTConfig__SecretKey="MY_SECRET_KEY"
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5091/

ENTRYPOINT ["dotnet", "InnovateFuture.Api.dll"]