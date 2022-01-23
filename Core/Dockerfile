
FROM node:16-alpine AS node
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env

COPY --from=node /usr/lib /usr/lib
COPY --from=node /usr/local/share /usr/local/share
COPY --from=node /usr/local/lib /usr/local/lib
COPY --from=node /usr/local/include /usr/local/include
COPY --from=node /usr/local/bin /usr/local/bin

WORKDIR /app

COPY ["RuntimeService/RuntimeService.csproj", "RuntimeService/"]
RUN dotnet restore "RuntimeService/RuntimeService.csproj"

COPY . ./

ARG CONF=Release
RUN dotnet publish -c ${CONF} -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .

RUN addgroup --system serviceuser && \
    adduser --system serviceuser -G serviceuser;

USER serviceuser

ENTRYPOINT ["dotnet", "RuntimeService.dll"]