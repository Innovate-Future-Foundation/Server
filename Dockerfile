FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/InnovateFuture.Api/InnovateFuture.Api.csproj", "src/InnovateFuture.Api/"]
COPY ["src/InnovateFuture.Application/InnovateFuture.Application.csproj", "src/InnovateFuture.Application/"]
COPY ["src/InnovateFuture.Domain/InnovateFuture.Domain.csproj", "src/InnovateFuture.Domain/"]
COPY ["src/InnovateFuture.Infrastructure/InnovateFuture.Infrastructure.csproj", "src/InnovateFuture.Infrastructure/"]
RUN dotnet restore "src/InnovateFuture.Api/InnovateFuture.Api.csproj"
COPY . .
WORKDIR "/src/src/InnovateFuture.Api"
RUN dotnet build "InnovateFuture.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InnovateFuture.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnovateFuture.Api.dll"]