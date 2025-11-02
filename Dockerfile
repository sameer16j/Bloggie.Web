# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY Bloggie.Web/Bloggie.Web.csproj Bloggie.Web/
RUN dotnet restore Bloggie.Web/Bloggie.Web.csproj

# Copy everything else and build the app
COPY . .
WORKDIR /src/Bloggie.Web
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bloggie.Web.dll"]
