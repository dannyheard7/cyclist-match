FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["RuntimeService/RuntimeService.csproj", "RuntimeService/"]
RUN dotnet restore "RuntimeService/RuntimeService.csproj"

# Copy everything else and build
COPY . ./

ARG CONF=Release
RUN dotnet publish -c ${CONF} -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "RuntimeService.dll"]