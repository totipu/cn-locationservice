# build image
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR ./StatlerWaldorfCorp.LocationService

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish --output /out/ --configuration Release

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR ./StatlerWaldorfCorp.LocationService
COPY --from=build /out .
#EXPOSE 5001
#ENV ASPNETCORE_URLS=http://+:5001
ENTRYPOINT [ "dotnet", "StatlerWaldorfCorp.LocationService.dll" ]
