# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy everything
COPY . ./

# Restore and build
RUN dotnet restore "./CourseHubApi/CourseHubApi.csproj"
RUN dotnet publish "./CourseHubApi/CourseHubApi.csproj" -c Release -o /publish

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CourseHubApi.dll"]
