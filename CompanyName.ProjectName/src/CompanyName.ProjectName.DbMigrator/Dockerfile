FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /
COPY . .
WORKDIR /src/CompanyName.ProjectName.DbMigrator
RUN dotnet restore -nowarn:msb3202,nu1503
# net9.0开始，wwwroot会自动作为嵌入资源，其中的映射机制，会受到build时的所在目录影响
# RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CompanyName.ProjectName.DbMigrator.dll"]
