#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=opencvsharp:latest /usr/lib /usr/lib
RUN sed -i s@/deb.debian.org/@/mirrors.aliyun.com/@g /etc/apt/sources.list && \
    apt-get update && apt-get -y install --no-install-recommends \
    libgdiplus \
    wget \
    gcc \
    ncurses-dev \
    vim

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PathPlanning/PathPlanning.csproj", "PathPlanning/"]
RUN dotnet restore "PathPlanning/PathPlanning.csproj"
COPY . .
WORKDIR "/src/PathPlanning"
RUN dotnet build "PathPlanning.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PathPlanning.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["PathPlanning/Tools/DLinkNetInference/dlinknet.onnx","/app/onnx/"]
RUN mkdir Images && \
    cd Images && \
    mkdir Input && \
    mkdir Output && \
    mkdir RoadNetworkConstruction
ENTRYPOINT ["dotnet", "PathPlanning.dll"]