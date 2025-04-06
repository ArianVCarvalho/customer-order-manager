
# Sistema de Gerenciamento de Pedidos para Logística

Este projeto é um sistema para gerenciamento de pedidos de transporte, incluindo funcionalidades para criar, atualizar, visualizar e excluir pedidos, além de gerenciar informações de clientes e calcular o frete através de uma API externa.

## Pré-Requisitos

Antes de iniciar a aplicação, certifique-se de ter os seguintes itens configurados:

- **.NET SDK** instalado (versão compatível com seu projeto, preferencialmente .NET 8).
- **Banco de dados SQL Server** configurado e acessível.
- **NLog** configurado para logging.

## Configuração do Ambiente
1. Crie um banco de dados SQL SERVER e rode o scrip que consta na pasta SQL

2. **Crie/altere o arquivo `appsettings.json`** na raiz do projeto com o seguinte conteúdo:

    ```json
    {
      "ConnectionStrings": {
        "FrenetShipManagementContext": "Server=seu_servidor;Database=seu_banco_de_dados;User Id=seu_usuario;Password=sua_senha;"
      }
    }
    ```
     Substitua os valores pelos detalhes do seu banco de dados.

OBS: Crie uma conta na frenet e gere suas chaves de acess key e authentication
3. **Adicione em suas `secrets.json` no visual studio ou em `.NET User Secrets` se for rodar a aplicação no rider** os seguintes dados
    ```json
    {    
    "FreteApiConfig": {
        "FreteApiBaseUrl": "http://api.frenet.com.br",
        "AccessToken": "ADICIONE AQUI SUA ACCESS KEY"
      },
      "Authentication": {
        "Secret": "ADICIONE AQUI SUA SECRET",
        "Issuer": "http://frenet.com.br",
        "Audience": "Frenet.ShipManagementClient",
        "ExpiresInMinutes": 60
      }
    }
    ```

   

## Inicialização do Projeto
Acesse `src/Frenet.ShipManagement`
1. **Restaure os pacotes NuGet**:

    Abra o terminal ou prompt de comando e navegue até o diretório do projeto. Execute o seguinte comando:

    ```bash
    dotnet restore
    ```

2. **Compile o projeto**:

    ```bash
    dotnet build
    ```

3. **Execute a aplicação**:

    ```bash
    dotnet run
    ```

## Como obter o JWT

Para obter o JWT e acessar a aplicação, siga estas etapas:

1. **Faça login** usando as credenciais padrão hardcoded no código-fonte:

   - **Email**: `uncle.bob@frenet.com.br`
   - **Senha**: `QK8uZ*ZorO46`

   Faça uma requisição POST para o endpoint de autenticação, no endpoint `/api/login`. Você receberá um token JWT em resposta.

2. **Use o JWT** obtido para acessar as rotas protegidas da API. Adicione o token ao cabeçalho de autorização das suas requisições usando o formato `{token}`.

## Logging e Monitoramento

- **Verifique os logs** gerados no console para assegurar que a aplicação está registrando informações corretamente.
- 
---
## Testes Unitários

Este projeto utiliza testes unitários para garantir que as funcionalidades dos serviços estejam funcionando corretamente. Para configurar e executar os testes, siga as etapas abaixo.

### Configuração dos Testes
Acesse Acesse `tests/Frenet.ShipManagement.UnitTests`

1.  Antes de executar os testes, compile o projeto para garantir que tudo está construído corretamente:
     ```bash
     dotnet build
     ```
 2. para rodar os testes execute
    ```bash
     dotnet test
     ```
---
# Testes de Integração

Este projeto utiliza testes de integração para garantir que as funcionalidades dos serviços estejam funcionando conforme o esperado. Siga as etapas abaixo para configurar e executar os testes de integração.
## Requisitos

Antes de executar os testes de integração, certifique-se de que você tenha o seguinte configurado:

- **.NET SDK** instalado (preferencialmente .NET 8).
- **Docker** instalado e ter 2gb de ram configurado, o mesmo deve estar em execução para suportar o Testcontainers.
- **Testcontainers.MsSql** e outras dependências de teste devem estar incluídas no projeto.

## Configuração do Ambiente
Acesse `tests/Frenet.ShipManagement.IntegrationTests` 
1. **Instale as Dependências do Projeto**
   Certifique-se de que todas as dependências necessárias para os testes estão instaladas. Execute o seguinte comando no diretório do projeto:
 
   ```bash
   dotnet restore
    ```
2. Mantenha o docker aberto/rodando

3. Executando
 Compile o projeto 
  Antes de executar os testes, compile o projeto para garantir que tudo está construído corretamente:
    ```bash
       dotnet build
      ```
4. Execute os testes
    Use o seguinte comando para rodar os testes de integração com xUnit:
    ```bash
       dotnet test
      ```
4. **O que é um contrato OpenAPI?**
   - a) Um contrato legal para o uso de uma API
   - **b) Uma especificação para descrever a interface de uma API RESTful, incluindo métodos, parâmetros e respostas**
   - c) Um arquivo de configuração para a segurança da API
   - d) Um módulo de segurança em APIs RESTful
