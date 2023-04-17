#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install nodejs -yq
RUN npm install -g @angular/cli
WORKDIR /src
COPY [".nuget/NuGet.Config", ".nuget/NuGet.Config"]
COPY ["UCare.Web/UCare.Web.csproj", "UCare.Web/"]
COPY ["UCare.Application/UCare.Application.csproj", "UCare.Application/"]
COPY ["UCare.Domain/UCare.Domain.csproj", "UCare.Domain/"]
COPY ["UCare.Shared/UCare.Shared.csproj", "UCare.Shared/"]
COPY ["UCare.Comunication.Service/UCare.Comunication.Service.csproj", "UCare.Comunication.Service/"]
COPY ["UCare.Infrastructure.Firebase/UCare.Infrastructure.Firebase.csproj", "UCare.Infrastructure.Firebase/"]
COPY ["UCare.Infrastructure.AlertService/UCare.Infrastructure.AlertService.csproj", "UCare.Infrastructure.AlertService/"]
COPY ["UCare.Infrastructure.GoogleApi/UCare.Infrastructure.GoogleApi.csproj", "UCare.Infrastructure.GoogleApi/"]
COPY ["UCare.Infrastructure.SendinBlue/UCare.Infrastructure.SendinBlue.csproj", "UCare.Infrastructure.SendinBlue/"]
COPY ["UCare.Infrastructure.SMS.Twilio/UCare.Infrastructure.SMS.Twilio.csproj", "UCare.Infrastructure.SMS.Twilio/"]
COPY ["UCare.Web/appsettings.production.json", "UCare.Web/appsettings.json"]
COPY ["UCare.Web/firebase.production.json", "UCare.Web/firebase.json"]
RUN dotnet restore "UCare.Web/UCare.Web.csproj" --configfile ".nuget/NuGet.Config"
COPY . .
RUN echo export const VERSION_APP = '1.0.00.38'; >> UCare.Web/ClientApp/src/version.ts
WORKDIR "/src/UCare.Web"
RUN dotnet build "UCare.Web.csproj" -c Release -o /app/build /p:FileVersion=1.0.00.38 /p:PackageVersion=1.0.00.38 /p:Version=1.0.00.38 /p:AssemblyVersion=1.0.00.38 /p:BASE_URL=/

FROM build AS publish
RUN dotnet publish "UCare.Web.csproj" -c Release -o /app/publish /p:FileVersion=1.0.00.38 /p:PackageVersion=1.0.00.38 /p:Version=1.0.00.38 /p:AssemblyVersion=1.0.00.38 /p:UseAppHost=false /p:BASE_URL=/

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UCare.Web.dll"]