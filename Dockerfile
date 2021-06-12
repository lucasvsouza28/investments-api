# Create build image
FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine as BUILD
WORKDIR /app
COPY . ./
RUN dotnet restore CaseBackend.sln
COPY . ./
RUN dotnet publish -c Release -o out

# Create runner image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine as RUNNER
WORKDIR /app
COPY --from=Build /app/out .
EXPOSE 80
ENTRYPOINT [ "dotnet", "CaseBackend.Api.dll"]