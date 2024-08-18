#This Dockerfile helps to provide an image for an ARM64 system 
#under dotnet SDK version 8. 
#E.g. for a Raspberry PI 4 under Raspberry PI OS.
#For example, the Dockerfile was successfully tested with an asp.net C# web API.

FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install locales package and set the locale
#RUN apt-get update
#RUN apt-get install -y locales
#RUN locale-gen en_US.UTF-8
#RUN update-locale LANG=en_US.UTF-8

# Set environment variables for locale
ENV LANG=en_US.UTF-8
ENV LANGUAGE=en_US:en
ENV LC_ALL=en_US.UTF-8


FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["webapi-iot-iounitdata.csproj", "webapi-iot-iounitdata/"]
RUN dotnet restore "./webapi-iot-iounitdata/webapi-iot-iounitdata.csproj"

COPY ["*", "webapi-iot-iounitdata/"]
WORKDIR "/src/webapi-iot-iounitdata"
RUN dotnet build "webapi-iot-iounitdata.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "webapi-iot-iounitdata.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "webapi-iot-iounitdata.dll"]