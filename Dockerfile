# Use uma imagem base do SDK .NET para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/ .

WORKDIR backend/myBookSolution.API

# Clone o seu repositório do GitHub
# OBS: Substitua 'SEU_USUARIO' e 'SEU_REPOSITORIO' pelos seus dados
RUN git clone https://github.com/ndrxy/Projeto-Redes.git .

# Restaure as dependências e compile a aplicação
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Use uma imagem de runtime .NET para executar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_ENVIRONMENT=Development

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "myBookSolution.API.dll"]