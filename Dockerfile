FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
# copy csproj and restore as distinct layers
COPY *.sln .
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Web/*.csproj ./Web/
RUN dotnet restore
# copy everything else and build
COPY Application/. ./Application/
COPY Domain/. ./Domain/
COPY Infrastructure/. ./Infrastructure/
COPY Web/. ./Web/
COPY . .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "Web.csproj" --configuration Release --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Web.dll"]