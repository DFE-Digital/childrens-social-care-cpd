#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Childrens-Social-Care-CPD.csproj", "."]
RUN dotnet restore "./Childrens-Social-Care-CPD.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Childrens-Social-Care-CPD.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Childrens-Social-Care-CPD.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ARG VCSREF=0
ENV VCS-REF=$VCSREF
ARG VCSTAG=0.0.0
ENV VCS-TAG=$VCSTAG
ENTRYPOINT ["dotnet", "Childrens-Social-Care-CPD.dll"]
