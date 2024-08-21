
# Sistema de Gerenciamento de Pedidos para Logística

Este projeto é um sistema para gerenciamento de pedidos de transporte, incluindo funcionalidades para criar, atualizar, visualizar e excluir pedidos, além de gerenciar informações de clientes e calcular o frete através de uma API externa.

## Pré-Requisitos

Antes de iniciar a aplicação, certifique-se de ter os seguintes itens configurados:

- **.NET SDK** instalado (versão compatível com seu projeto, preferencialmente .NET 8).
- **Banco de dados SQL Server** configurado e acessível.
- **NLog** configurado para logging.

## Configuração do Ambiente

1. **Crie o arquivo `appsettings.json`** na raiz do projeto com o seguinte conteúdo:

    ```json
    {
      "ConnectionStrings": {
        "FrenetShipManagementContext": "Server=seu_servidor;Database=seu_banco_de_dados;User Id=seu_usuario;Password=sua_senha;"
      },
      "Authentication": {
        "Secret": "sua_chave_secreta_base64",
        "Issuer": "seu_emissor",
        "Audience": "seu_publico"
      }
    }
    ```

    Substitua os valores pelos detalhes do seu banco de dados e as configurações do JWT.

2. **Configure o NLog**

   O NLog está configurado para registrar mensagens no console. Certifique-se de que o arquivo `NLog.config` ou as configurações de log estejam apropriados para o seu ambiente.

## Configuração do Banco de Dados

1. **Crie o banco de dados** SQL Server se ainda não existir.
2. **Configure a conexão** com o banco de dados no arquivo `appsettings.json`.

## Inicialização do Projeto

1. **Restaure os pacotes NuGet**:

    Abra o terminal ou prompt de comando e navegue até o diretório do projeto. Execute o seguinte comando:

    ```bash
    dotnet restore
    ```

2. **Crie e aplique as migrações** se estiver usando Entity Framework:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```


3. **Compile o projeto**:

    ```bash
    dotnet build
    ```

4. **Execute a aplicação**:

    ```bash
    dotnet run
    ```

## Verificação

1. **Acesse a Swagger UI** (se em ambiente de desenvolvimento) para verificar a documentação da API:

    Navegue até `https://localhost:5001/swagger` ou o endpoint definido no `app.UseSwaggerUI()`.

2. **Teste as rotas da API** usando ferramentas como Postman ou diretamente na interface Swagger para garantir que tudo esteja funcionando conforme esperado.

## Acesso à Aplicação

Para obter um JWT e acessar a aplicação, siga estas etapas:

1. **Faça login** usando as credenciais padrão hardcoded no código-fonte:

   - **Email**: `uncle.bob@frenet.com.br`
   - **Senha**: `QK8uZ*ZorO46`

   Faça uma requisição POST para o endpoint de autenticação, normalmente algo como `/api/login`. Você receberá um token JWT em resposta.

2. **Use o JWT** obtido para acessar as rotas protegidas da API. Adicione o token ao cabeçalho de autorização das suas requisições usando o formato `{token}`.


## Logging e Monitoramento

- **Verifique os logs** gerados no console para assegurar que a aplicação está registrando informações corretamente.
- 
---
## Testes Unitários

Este projeto utiliza testes unitários para garantir que as funcionalidades dos serviços. Para configurar e executar os testes, siga as etapas abaixo.

### Configuração dos Testes

1. **Certifique-se de que todas as dependências estão instaladas ao acessar o projeto de teste**:
   O projeto utiliza o framework `xUnit` e a biblioteca `FluentAssertions` para os testes unitários. As dependências podem ser instaladas com os seguintes comandos:

   ```bash
   dotnet add package xunit
   dotnet add package FluentAssertions
   dotnet add package Moq
   ````
    ou
    ```bash
   dotnet restore
    ```

 2.  Antes de executar os testes, compile o projeto para garantir que tudo está construído corretamente:
    ```bash
        dotnet build
    ```
 3. Acesse o projeto `Frenet.ShipManagement.UnitTests` e execute
    ```bash
     dotnet test
     ```
---
# Documentação para Rodar Testes de Integração - ClienteRepository

Esta documentação fornece instruções para executar os testes de integração no projeto.

## Requisitos

Antes de executar os testes de integração, certifique-se de que você tenha o seguinte configurado:

- **.NET SDK** instalado (preferencialmente .NET 8).
- **Docker** instalado e em execução para suportar o Testcontainers.
- **Testcontainers.MsSql** e outras dependências de teste devem estar incluídas no projeto.

## Configuração do Ambiente

1. **Instale as Dependências do Projeto**

   Certifique-se de que todas as dependências necessárias para os testes estão instaladas. Execute o seguinte comando no diretório do projeto:

   ```bash
   dotnet restore
    ```
2. Configure o Docker
    Testcontainers usa Docker para criar um contêiner SQL Server. Verifique se o Docker está instalado e em execução no seu sistema.

3. Executando
 Compile o projeto   Antes de executar os testes, compile o projeto para garantir que tudo está construído corretamente:
    ```bash       dotnet build
      ```
4. Execute os testes
    Use o seguinte comando para rodar os testes de integração com xUnit:
    ```bash       dotnet build
      ```


---
# Tecnologias 
- **Linguagem de Programação:** C#
- **Framework:** .NET 8.0
- **Banco de Dados:** Microsoft SQL Server
- **Documentação de API:** Swagger/OpenAPI
- **Logging:** NLog
- **Controle de Versão:** Git
- **Testes:** Unitários e de Integração
- **Docker:** Para realizar os testes de integração
---

---
# Questionário - Avaliação de Programador Backend (.NET C#) Pleno

#### **Seção 1: C# e Desenvolvimento de API RESTful**

1. **Qual o principal benefício do uso de Dependency Injection (DI) no desenvolvimento com ASP.NET Core?**
   - a) Reduz a quantidade de memória usada
   - b) Melhora a performance do código
   - **c) Facilita a criação de testes unitários e promove a modularidade do código**
   - d) Aumenta a quantidade de código escrito

2. **Ao desenvolver uma API RESTful em ASP.NET Core, qual código de resposta HTTP deve ser retornado para uma requisição de recurso que não foi encontrado?**
   - a) 200 OK
   - b) 400 Bad Request
   - **c) 404 Not Found**
   - d) 500 Internal Server Error

3. **No contexto de APIs RESTful, o que significa o termo "Idempotência"?**
   - a) A ação de garantir que todas as requisições HTTP sejam seguras
   - **b) O comportamento de métodos que podem ser repetidos sem efeitos colaterais adicionais**
   - c) A capacidade de uma API de suportar várias versões
   - d) O processo de cache de respostas para melhorar a performance

4. **Em ASP.NET Core, qual é a finalidade do atributo `[HttpGet]`?**
   - a) Definir um método como seguro
   - **b) Indicar que um método manipula requisições GET**
   - c) Garantir que o método retorne sempre uma string
   - d) Definir a resposta como JSON

5. **Qual é o principal propósito de usar o Entity Framework Core ao desenvolver com .NET?**
   - a) Aumentar a velocidade de processamento da aplicação
   - b) Facilitar a manipulação de arquivos no sistema de arquivos
   - c) Gerar consultas SQL manualmente
   - **d) Mapear objetos de domínio para tabelas de banco de dados relacional**

6. **Quando uma API RESTful deve usar o método HTTP PUT em vez de POST?**
   - a) Quando se deseja criar um novo recurso
   - **b) Quando se deseja atualizar ou criar um recurso em uma URI específica**
   - c) Quando se deseja excluir um recurso
   - d) Quando se deseja enviar dados sem persistência

7. **Em APIs RESTful, o que é HATEOAS (Hypermedia as the Engine of Application State)?**
   - a) Um padrão para documentação de APIs
   - b) Um tipo de autenticação para APIs
   - **c) Um princípio que descreve a navegação baseada em hipermídia nas respostas da API**
   - d) Uma técnica para compressão de dados JSON

8. **Qual a diferença entre `IEnumerable<T>` e `IQueryable<T>` em C#?**
   - a) `IEnumerable<T>` executa consultas diretamente no banco de dados
   - **b) `IQueryable<T>` permite a execução de consultas no banco de dados, enquanto `IEnumerable<T>` realiza consultas na memória**
   - c) `IQueryable<T>` é usado apenas com listas
   - d) `IEnumerable<T>` sempre retorna resultados de forma assíncrona

9. **Qual é a vantagem de usar async/await no C# em APIs RESTful?**
   - a) Reduz o tamanho do código escrito
   - **b) Melhora a performance de I/O, permitindo a execução assíncrona de tarefas sem bloquear a thread principal**
   - c) Permite a execução de código paralelo
   - d) Diminui a quantidade de erros em tempo de execução

10. **Qual é o padrão mais adequado para a versão de uma API RESTful?**
    - a) Usar a versão no cabeçalho HTTP
    - **b) Incluir a versão como parte da URL**
    - c) Não é necessário versionar APIs RESTful
    - d) Depende do tipo de autenticação usada

11. **Ao configurar um serviço em ASP.NET Core, qual é a diferença entre `AddScoped`, `AddSingleton` e `AddTransient`?**
    - **a) `AddScoped` cria uma instância por requisição, `AddSingleton` uma instância única e `AddTransient` uma nova instância a cada solicitação**
    - b) `AddScoped` cria uma instância por aplicação, `AddSingleton` uma instância por requisição, e `AddTransient` nunca cria instâncias
    - c) Todos são equivalentes e a escolha depende do gosto do desenvolvedor
    - d) `AddSingleton` sempre cria uma instância por sessão

12. **Como utilizar o atributo `[Route("api/[controller]")]` em ASP.NET Core?**
    - a) Definir um caminho específico para todos os métodos em um controlador
    - b) Configurar o controlador para responder a todas as rotas
    - **c) Definir o prefixo de rota para todos os métodos do controlador**
    - d) Restringir o controlador para responder apenas a métodos POST

#### **Seção 2: Banco de Dados Microsoft SQL Server**

1. **O que significa a sigla ACID no contexto de transações em banco de dados?**
   - **a) Atomicidade, Consistência, Isolamento, Durabilidade**
   - b) Adaptação, Confiabilidade, Independência, Desempenho
   - c) Acessibilidade, Controle, Integridade, Distribuição
   - d) Asynchronous, Cache, Indexing, Durability

2. **Qual é o comando SQL para criar uma nova tabela em um banco de dados Microsoft SQL Server?**
   - a) `CREATE NEW TABLE`
   - b) `INSERT TABLE`
   - **c) `CREATE TABLE`**
   - d) `ADD TABLE`

3. **Qual comando SQL é usado para retornar apenas registros distintos em uma consulta?**
   - a) `UNIQUE`
   - **b) `SELECT DISTINCT`**
   - c) `SELECT UNIQUE`
   - d) `FILTER DISTINCT`

4. **Qual é o propósito do comando `JOIN` em SQL?**
   - **a) Combinar dados de duas ou mais tabelas**
   - b) Excluir registros duplicados
   - c) Filtrar registros com base em uma condição
   - d) Modificar a estrutura de uma tabela

5. **Qual é a diferença entre uma `PRIMARY KEY` e uma `FOREIGN KEY` em SQL?**
   - **a) Uma `PRIMARY KEY` identifica exclusivamente um registro em uma tabela, enquanto uma `FOREIGN KEY` refere-se a uma `PRIMARY KEY` em outra tabela**
   - b) Ambas são usadas para criar índices em uma tabela
   - c) Uma `FOREIGN KEY` cria uma nova tabela, enquanto uma `PRIMARY KEY` define o tipo de dados de uma coluna
   - d) Não há diferença, ambas são a mesma coisa

6. **Qual dos seguintes tipos de índices pode melhorar o desempenho de consultas em colunas que possuem valores duplicados?**
   - a) Índice Clustered
   - **b) Índice Não Clustered**
   - c) Índice Único
   - d) Índice de Texto Completo

7. **Como você pode garantir que uma coluna em uma tabela SQL Server não aceitará valores nulos?**
   - **a) Definindo a coluna como `NOT NULL`**
   - b) Usando `NULLABLE FALSE`
   - c) Aplicando uma `PRIMARY KEY` na coluna
   - d) Aplicando um índice único na coluna

8. **Qual é o propósito da cláusula `GROUP BY` em uma consulta SQL?**
   - a) Organizar os dados em ordem crescente
   - b) Filtrar registros duplicados
   - **c) Agrupar registros que têm os mesmos valores em colunas específicas**
   - d) Ordenar os registros com base em um campo específico

9. **No SQL Server, qual é a função do comando `TRIGGER`?**
   - **a) Executar uma ação quando um evento específico ocorre em uma tabela**
   - b) Fazer backup automático de uma tabela
   - c) Restaurar dados em uma tabela
   - d) Indexar automaticamente uma coluna

10. **Qual é a finalidade de usar a palavra-chave `WITH (NOLOCK)` em uma consulta SQL Server?**
    - a) Ignorar as permissões de segurança para acessar a tabela
    - **b) Ler os dados sem adquirir bloqueios, evitando possíveis deadlocks**
    - c) Excluir temporariamente os índices
    - d) Bloquear outras consultas até que a atual seja concluída

11. **Qual é o resultado de executar o seguinte código SQL?**
    ```sql
    SELECT COUNT(*) FROM Tabela WHERE Coluna IS NULL;
    ```
    - a) Retorna o número total de registros na tabela
    - **b) Retorna o número de registros onde a coluna é nula**
    - c) Exclui os registros onde a coluna é nula
    - d) Retorna todos os registros que não possuem valores nulos

12. **Como você pode alterar o nome de uma coluna em uma tabela existente no SQL Server?**
    - a) `ALTER COLUMN`
    - b) `RENAME COLUMN`
    - c) `UPDATE COLUMN`
    - **d) `SP_RENAME`**

#### **Seção 3: Swagger/OpenAPI**

1. **O que é o Swagger/OpenAPI?**
   - a) Um framework para autenticação de APIs
   - **b) Um padrão para modelar e documentar APIs RESTful**
   - c) Um sistema de cache para APIs
   - d) Um banco de dados em tempo real

2. **Qual a principal vantagem de usar o Swagger para documentação de APIs?**
   - **a) Gerar código automaticamente para diferentes linguagens**
   - b) Aumentar a performance da API
   - c) Automatizar a autenticação da API
   - d) Controlar o acesso de usuários à API

3. **Como você pode adicionar a documentação Swagger a uma API ASP.NET Core?**
   - **a) Instalando o pacote `Swashbuckle.AspNetCore` e configurando-o no `Startup.cs`**
   - b) Escrevendo manualmente o código Swagger em um arquivo `.json`
   - c) Usando o atributo `[SwaggerDocument]` nos métodos
   - d) Configurando o Swagger diretamente na interface do Visual Studio

4. **O que é um contrato OpenAPI?**
   - a) Um contrato legal para o uso de uma API
   - **b) Uma especificação para descrever a interface de uma API RESTful, incluindo métodos, parâmetros e respostas**
   - c) Um arquivo de configuração para a segurança da API
   - d) Um módulo de segurança em APIs RESTful