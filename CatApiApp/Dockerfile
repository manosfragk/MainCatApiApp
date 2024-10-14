# Use the official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY CatApiApp.csproj ./
RUN dotnet restore "CatApiApp.csproj"

# Copy the remaining files and build the project
COPY . . 
RUN dotnet publish "CatApiApp.csproj" -c Release -o /app/publish

# Step 2: Use a smaller runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published files from the build stage to the runtime image
COPY --from=build /app/publish .

# Set the environment to production
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the port the app listens on
EXPOSE 80

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "CatApiApp.dll"]
