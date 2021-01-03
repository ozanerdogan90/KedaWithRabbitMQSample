FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . ./src/
RUN for file in $(ls */*.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore "src/KedaWithRabbitMQSample.Api/KedaWithRabbitMQSample.Api.csproj"
COPY . .
WORKDIR "/src/src/KedaWithRabbitMQSample.Api"
RUN dotnet build "KedaWithRabbitMQSample.Api.csproj" -c Release -o /app/build

FROM build AS publish
ARG VERSION=0.0.0.0
RUN dotnet publish "KedaWithRabbitMQSample.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KedaWithRabbitMQSample.Api.dll"]