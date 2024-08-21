

# Tecnologias 
- **Linguagem de Programa��o:** C#
- **Framework:** .NET 8.0
- **Banco de Dados:** Microsoft SQL Server
- **Documenta��o de API:** Swagger/OpenAPI
- **Logging:** NLog
- **Controle de Vers�o:** Git
- **Testes:** Unit�rios e de Integra��o
- **Docker:** Para realizar os testes de integra��o
---

---
# Question�rio - Avalia��o de Programador Backend (.NET C#) Pleno

#### **Se��o 1: C# e Desenvolvimento de API RESTful**

1. **Qual o principal benef�cio do uso de Dependency Injection (DI) no desenvolvimento com ASP.NET Core?**
   - a) Reduz a quantidade de mem�ria usada
   - b) Melhora a performance do c�digo
   - **c) Facilita a cria��o de testes unit�rios e promove a modularidade do c�digo**
   - d) Aumenta a quantidade de c�digo escrito

2. **Ao desenvolver uma API RESTful em ASP.NET Core, qual c�digo de resposta HTTP deve ser retornado para uma requisi��o de recurso que n�o foi encontrado?**
   - a) 200 OK
   - b) 400 Bad Request
   -**c) 404 Not Found**
   - d) 500 Internal Server Error

3. **No contexto de APIs RESTful, o que significa o termo "Idempot�ncia"?**
   - a) A a��o de garantir que todas as requisi��es HTTP sejam seguras
   - **b) O comportamento de m�todos que podem ser repetidos sem efeitos colaterais adicionais**
   - c) A capacidade de uma API de suportar v�rias vers�es
   - d) O processo de cache de respostas para melhorar a performance

4. **Em ASP.NET Core, qual � a finalidade do atributo `[HttpGet]`?**
   - a) Definir um m�todo como seguro
   - **b) Indicar que um m�todo manipula requisi��es GET**
   - c) Garantir que o m�todo retorne sempre uma string
   - d) Definir a resposta como JSON

5. **Qual � o principal prop�sito de usar o Entity Framework Core ao desenvolver com .NET?**
   - a) Aumentar a velocidade de processamento da aplica��o
   - b) Facilitar a manipula��o de arquivos no sistema de arquivos
   - c) Gerar consultas SQL manualmente
   - **d) Mapear objetos de dom�nio para tabelas de banco de dados relacional**

6. **Quando uma API RESTful deve usar o m�todo HTTP PUT em vez de POST?**
   - a) Quando se deseja criar um novo recurso
   - **b) Quando se deseja atualizar ou criar um recurso em uma URI espec�fica**
   - c) Quando se deseja excluir um recurso
   - d) Quando se deseja enviar dados sem persist�ncia

7. **Em APIs RESTful, o que � HATEOAS (Hypermedia as the Engine of Application State)?**
   - a) Um padr�o para documenta��o de APIs
   - b) Um tipo de autentica��o para APIs
   - **c) Um princ�pio que descreve a navega��o baseada em hiperm�dia nas respostas da API**
   - d) Uma t�cnica para compress�o de dados JSON

8. **Qual a diferen�a entre `IEnumerable<T>` e `IQueryable<T>` em C#?**
   - a) `IEnumerable<T>` executa consultas diretamente no banco de dados
   - **b) `IQueryable<T>` permite a execu��o de consultas no banco de dados, enquanto `IEnumerable<T>` realiza consultas na mem�ria**
   - c) `IQueryable<T>` � usado apenas com listas
   - d) `IEnumerable<T>` sempre retorna resultados de forma ass�ncrona

9. **Qual � a vantagem de usar async/await no C# em APIs RESTful?**
   - a) Reduz o tamanho do c�digo escrito
   - **b) Melhora a performance de I/O, permitindo a execu��o ass�ncrona de tarefas sem bloquear a thread principal**
   - c) Permite a execu��o de c�digo paralelo
   - d) Diminui a quantidade de erros em tempo de execu��o

10. **Qual � o padr�o mais adequado para a vers�o de uma API RESTful?**
    - a) Usar a vers�o no cabe�alho HTTP
    - **b) Incluir a vers�o como parte da URL**
    - c) N�o � necess�rio versionar APIs RESTful
    - d) Depende do tipo de autentica��o usada

11. **Ao configurar um servi�o em ASP.NET Core, qual � a diferen�a entre `AddScoped`, `AddSingleton` e `AddTransient`?**
    - **a) `AddScoped` cria uma inst�ncia por requisi��o, `AddSingleton` uma inst�ncia �nica e `AddTransient` uma nova inst�ncia a cada solicita��o**
    - b) `AddScoped` cria uma inst�ncia por aplica��o, `AddSingleton` uma inst�ncia por requisi��o, e `AddTransient` nunca cria inst�ncias
    - c) Todos s�o equivalentes e a escolha depende do gosto do desenvolvedor
    - d) `AddSingleton` sempre cria uma inst�ncia por sess�o

12. **Como utilizar o atributo `[Route("api/[controller]")]` em ASP.NET Core?**
    - a) Definir um caminho espec�fico para todos os m�todos em um controlador
    - b) Configurar o controlador para responder a todas as rotas
    - **c) Definir o prefixo de rota para todos os m�todos do controlador**
    - d) Restringir o controlador para responder apenas a m�todos POST

#### **Se��o 2: Banco de Dados Microsoft SQL Server**

1. **O que significa a sigla ACID no contexto de transa��es em banco de dados?**
   - **a) Atomicidade, Consist�ncia, Isolamento, Durabilidade**
   - b) Adapta��o, Confiabilidade, Independ�ncia, Desempenho
   - c) Acessibilidade, Controle, Integridade, Distribui��o
   - d) Asynchronous, Cache, Indexing, Durability

2. **Qual � o comando SQL para criar uma nova tabela em um banco de dados Microsoft SQL Server?**
   - a) `CREATE NEW TABLE`
   - b) `INSERT TABLE`
   - **c) `CREATE TABLE`**
   - d) `ADD TABLE`

3. **Qual comando SQL � usado para retornar apenas registros distintos em uma consulta?**
   - a) `UNIQUE`
   - **b) `SELECT DISTINCT`**
   - c) `SELECT UNIQUE`
   - d) `FILTER DISTINCT`

4. **Qual � o prop�sito do comando `JOIN` em SQL?**
   - **a) Combinar dados de duas ou mais tabelas**
   - b) Excluir registros duplicados
   - c) Filtrar registros com base em uma condi��o
   - d) Modificar a estrutura de uma tabela

5. **Qual � a diferen�a entre uma `PRIMARY KEY` e uma `FOREIGN KEY` em SQL?**
   - **a) Uma `PRIMARY KEY` identifica exclusivamente um registro em uma tabela, enquanto uma `FOREIGN KEY` refere-se a uma `PRIMARY KEY` em outra tabela**
   - b) Ambas s�o usadas para criar �ndices em uma tabela
   - c) Uma `FOREIGN KEY` cria uma nova tabela, enquanto uma `PRIMARY KEY` define o tipo de dados de uma coluna
   - d) N�o h� diferen�a, ambas s�o a mesma coisa

6. **Qual dos seguintes tipos de �ndices pode melhorar o desempenho de consultas em colunas que possuem valores duplicados?**
   - a) �ndice Clustered
   - **b) �ndice N�o Clustered**
   - c) �ndice �nico
   - d) �ndice de Texto Completo

7. **Como voc� pode garantir que uma coluna em uma tabela SQL Server n�o aceitar� valores nulos?**
   - **a) Definindo a coluna como `NOT NULL`**
   - b) Usando `NULLABLE FALSE`
   - c) Aplicando uma `PRIMARY KEY` na coluna
   - d) Aplicando um �ndice �nico na coluna

8. **Qual � o prop�sito da cl�usula `GROUP BY` em uma consulta SQL?**
   - a) Organizar os dados em ordem crescente
   - b) Filtrar registros duplicados
   - **c) Agrupar registros que t�m os mesmos valores em colunas espec�ficas**
   - d) Ordenar os registros com base em um campo espec�fico

9. **No SQL Server, qual � a fun��o do comando `TRIGGER`?**
   - **a) Executar uma a��o quando um evento espec�fico ocorre em uma tabela**
   - b) Fazer backup autom�tico de uma tabela
   - c) Restaurar dados em uma tabela
   - d) Indexar automaticamente uma coluna

10. **Qual � a finalidade de usar a palavra-chave `WITH (NOLOCK)` em uma consulta SQL Server?**
    - a) Ignorar as permiss�es de seguran�a para acessar a tabela
    - **b) Ler os dados sem adquirir bloqueios, evitando poss�veis deadlocks**
    - c) Excluir temporariamente os �ndices
    - d) Bloquear outras consultas at� que a atual seja conclu�da

11. **Qual � o resultado de executar o seguinte c�digo SQL?**
    ```sql
    SELECT COUNT(*) FROM Tabela WHERE Coluna IS NULL;
    ```
    - a) Retorna o n�mero total de registros na tabela
    - **b) Retorna o n�mero de registros onde a coluna � nula**
    - c) Exclui os registros onde a coluna � nula
    - d) Retorna todos os registros que n�o possuem valores nulos

12. **Como voc� pode alterar o nome de uma coluna em uma tabela existente no SQL Server?**
    - a) `ALTER COLUMN`
    - b) `RENAME COLUMN`
    - c) `UPDATE COLUMN`
    - **d) `SP_RENAME`**

#### **Se��o 3: Swagger/OpenAPI**

1. **O que � o Swagger/OpenAPI?**
   - a) Um framework para autentica��o de APIs
   - **b) Um padr�o para modelar e documentar APIs RESTful**
   - c) Um sistema de cache para APIs
   - d) Um banco de dados em tempo real

2. **Qual a principal vantagem de usar o Swagger para documenta��o de APIs?**
   - **a) Gerar c�digo automaticamente para diferentes linguagens**
   - b) Aumentar a performance da API
   - c) Automatizar a autentica��o da API
   - d) Controlar o acesso de usu�rios � API

3. **Como voc� pode adicionar a documenta��o Swagger a uma API ASP.NET Core?**
   - **a) Instalando o pacote `Swashbuckle.AspNetCore` e configurando-o no `Startup.cs`**
   - b) Escrevendo manualmente o c�digo Swagger em um arquivo `.json`
   - c) Usando o atributo `[SwaggerDocument]` nos m�todos
   - d) Configurando o Swagger diretamente na interface do Visual Studio

4. **O que � um contrato OpenAPI?**
   - a) Um contrato legal para o uso de uma API
   - **b) Uma especifica��o para descrever a interface de uma API RESTful, incluindo m�todos, par�metros e respostas**
   - c) Um arquivo de configura��o para a seguran�a da API
   - d) Um m�dulo de seguran�a em APIs RESTful