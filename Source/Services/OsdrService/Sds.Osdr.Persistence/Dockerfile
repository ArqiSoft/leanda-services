# Use SDK image to build solution
FROM microsoft/dotnet:2.1-sdk AS build-env

ARG RID=linux-x64

WORKDIR /build

COPY . .

RUN dotnet restore --configfile /build/Nuget.config Sds.Osdr.Persistence/Sds.Osdr.Persistence.csproj

# RUN dotnet build OsdrService.sln

RUN dotnet publish Sds.Osdr.Persistence/Sds.Osdr.Persistence.csproj -r $RID -o /dist

# Build runtime image
FROM microsoft/dotnet:2.1-runtime-deps

LABEL maintainer="rick.zakharov@gmail.com"

WORKDIR /app

COPY --from=build-env /dist ./

ENTRYPOINT ["./Sds.Osdr.Persistence"]
