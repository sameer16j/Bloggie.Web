# Use the official .NET 8 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build the app
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Use the smaller ASP.NET Core runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published output
COPY --from=build /app/publish .

# Expose port 8080 (Render expects this)
EXPOSE 8080

# Set environment variable for Render
ENV ASPNETCORE_URLS=http://+:8080

# Start the application
ENTRYPOINT ["dotnet", "Bloggie.Web.dll"]
