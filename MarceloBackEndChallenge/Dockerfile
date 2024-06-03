#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MarceloDesafioBackEnd.csproj", "."]
RUN dotnet restore "./MarceloDesafioBackEnd.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MarceloDesafioBackEnd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarceloDesafioBackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarceloDesafioBackEnd.dll"]

# Use a imagem oficial do RabbitMQ como base
FROM rabbitmq:management

# Exponha as portas padrão do RabbitMQ e do RabbitMQ Management Console
EXPOSE 5672 15672

# Copie o arquivo de configuração personalizado para o contêiner
COPY rabbitmq.config /etc/rabbitmq/

# Defina as variáveis de ambiente para personalizar a configuração do RabbitMQ, se necessário
ENV RABBITMQ_DEFAULT_USER=user
ENV RABBITMQ_DEFAULT_PASS=password

# Comando de entrada padrão para iniciar o RabbitMQ
CMD ["rabbitmq-server"]