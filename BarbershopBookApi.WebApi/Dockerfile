FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["BarbershopBookApi.WebApi/BarbershopBookApi.WebApi.csproj", "BarbershopBookApi.WebApi/"]
COPY ["BarbershopBookApi.Application/BarbershopBookApi.Application.csproj", "BarbershopBookApi.Application/"]
COPY ["BarbershopBookApi.Domain/BarbershopBookApi.Domain.csproj", "BarbershopBookApi.Domain/"]
COPY ["BarbershopBookApi.Infrastructure/BarbershopBookApi.Infrastructure.csproj", "BarbershopBookApi.Infrastructure/"]

RUN dotnet restore "BarbershopBookApi.WebApi/BarbershopBookApi.WebApi.csproj"
COPY . .
WORKDIR "/src/BarbershopBookApi.WebApi"
RUN dotnet build "BarbershopBookApi.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BarbershopBookApi.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BarbershopBookApi.WebApi.dll"]