# Copy csproj and restore as layer 1
# FROM microsoft/dotnet:5.0-sdk-alpine AS build
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
ARG KCSNUGET=https://artifact.kymetacorp.com/artifactory/api/nuget/KCS-nuget
ARG NUGET=https://api.nuget.org/v3/index.json
ARG BUILD_NUMBER=1.0.0.0
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore --source $KCSNUGET --source $NUGET

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS final
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
EXPOSE 443
EXPOSE 80
COPY --from=build /app/out .
CMD ["dotnet", "Kymeta.Cloud.Services.EnterpriseBroker.dll"]