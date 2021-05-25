# build image
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR ./StatlerWaldorfCorp.LocationService

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish --output /out/ --configuration Release

# runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR ./StatlerWaldorfCorp.LocationService
COPY --from=build /out .
#EXPOSE 5001
#ENV ASPNETCORE_URLS=http://+:5001
ENTRYPOINT [ "dotnet", "StatlerWaldorfCorp.LocationService.dll" ]