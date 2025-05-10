FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MSProfessionals.API/MSProfessionals.API.csproj", "MSProfessionals.API/"]
COPY ["MSProfessionals.Application/MSProfessionals.Application.csproj", "MSProfessionals.Application/"]
COPY ["MSProfessionals.Domain/MSProfessionals.Domain.csproj", "MSProfessionals.Domain/"]
COPY ["MSProfessionals.Infrastructure/MSProfessionals.Infrastructure.csproj", "MSProfessionals.Infrastructure/"]
RUN dotnet restore "MSProfessionals.API/MSProfessionals.API.csproj"
COPY . .
RUN dotnet build "MSProfessionals.API/MSProfessionals.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MSProfessionals.API/MSProfessionals.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MSProfessionals.API.dll"] 