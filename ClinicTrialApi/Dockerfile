## Set base image
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 80
#
## Set up SDK and app build
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#COPY ["ClinicTrialApi/ClinicTrialApi.csproj", "ClinicTrialApi/"]
#RUN dotnet restore "ClinicTrialApi/ClinicTrialApi.csproj"
#COPY . .
#WORKDIR "/src/ClinicTrialApi"
#RUN dotnet build "ClinicTrialApi.csproj" -c Release -o /app/build
#
#RUN dotnet tool install --global dotnet-ef --version 9.0.1
#RUN dotnet ef database update
#
#FROM build AS publish
#RUN dotnet publish "ClinicTrialApi.csproj" -c Release -o /app/publish
#
## Copy published output to base image
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#
## Ensure migrations are applied when the container starts
##RUN dotnet tool install --global dotnet-ef
#ENV PATH="$PATH:/root/.dotnet/tools"
#
## Add entrypoint script
##COPY entrypoint.sh .
##RUN chmod +x ./entrypoint.sh
#
##ENTRYPOINT ["entrypoint.sh"]
















# Set base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Set up SDK and app build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ClinicTrialApi/ClinicTrialApi.csproj", "ClinicTrialApi/"]
RUN dotnet restore "ClinicTrialApi/ClinicTrialApi.csproj"
COPY . .
WORKDIR "/src/ClinicTrialApi"
RUN dotnet build "ClinicTrialApi.csproj" -c Release -o /app/build

# Install dotnet-ef and update PATH for the global tools
RUN dotnet tool install --global dotnet-ef --version 9.0.1
ENV PATH="$PATH:/root/.dotnet/tools"

FROM build AS publish
RUN dotnet publish "ClinicTrialApi.csproj" -c Release -o /app/publish

# Copy published output to base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure migrations are applied at runtime
ENTRYPOINT ["dotnet", "ClinicTrialApi.dll"]

