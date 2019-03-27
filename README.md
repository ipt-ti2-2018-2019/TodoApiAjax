# Todo API

Uma API de TODOs para usar nas aulas, de forma a explicar os conceitos de AJAX.

## Como usar

Esta API está disponível no seguinte link: https://ipt-ti2-todo.azurewebsites.net/.

Opcionalmente, esta API pode ser executada localmente (ver secção "Executar localmente").

### Executar localmente

Para usar localmente, não é preciso uma base de dados. A desvantagem é que, de cada vez que a aplicação reinicia, os dados perdem-se.

É possível configurar também a aplicação para usar um SQL Server, ou um SQL Server Local DB. Deve-se editar o ficheiro `appsettings.json` e mudar a connection string `TodoDb` para algo como:

```jsonc
{
    "ConnectionStrings": {
        "TodoDb": "Server=(localdb)\\mssqllocaldb;Database=TodoApi;Integrated Security=True"
    }
}
```

Para usar Local DB (como nas aulas do professor José Casimiro).

É também necessário editar o ficheiro `Startup.cs` para ativar a opção `UseSqlServer` e comentar a opção `UseInMemoryDatabase`.

Depois, pode-se iniciar o projeto.

Para executar o projeto no Visual Studio, deve-se abrir o ficheiro `TodoApi.csproj`, e executar como outro projeto qualquer. Um browser deve abrir automaticamente.

Para executar via linha de comandos, deve-se executar o seguinte comando:

```
dotnet run
```

Na pasta onde está o ficheiro `TodoApi.csproj`. A aplicação fica a correr no porto 5000 (`http://localhost:5000`).

## Acessos

Esta aplicação não tem autenticação. No entanto, implementa uma _whitelist_ rudimentar (por favor, não façam isto em produção!), no ficheiro `Data/TodoAppUser.cs`. Se o número de aluno não está na lista, deve ser adicionado.

Tem-se os seguintes "users":

-   `afecarvalho` (Prof. André Carvalho)
-   `casimiro` (Prof. José Casimiro Pereira)
-   `alunoXXXXX` (Aluno com número)

## Modelo de dados

Esta API lida com tarefas (TODOs). O modelo de dados de cada tarefa é:

```jsonc
{
    "id": 1, // ID da tarefa (long, obrigatório, PK)
    "description": "Olá, Mundo!", // Descrição da tarefa (string, obrigatório, máx. 512 chars)
    "userName": "afecarvalho", // Username de quem fez a tarefa (string, obrigatório, máx. 64 chars)
    "isComplete": false, // Se está completo, ou não (boolean, obrigatório)
    "addedAt": "2019-03-23T11:19:05.5380968+00:00", // Data de criação (DateTime, obrigatório)
    "lastUpdatedAt": "2019-03-23T11:19:05.5388266+00:00" // Data de última edição (DateTime, obrigatório)
}
```

## Operações

### Listar tarefas: `GET /api/todos/{user}`

Devolve um array de tarefas para um utilizador.

#### Parâmetros:

Obrigatório:

-   Username, no URL (substituir `{user}` pelo user pretendido)

Opcional:

-   `description`, na query string (string, se estiver presente, é feita uma pesquisa pela descrição (usando o `LIKE`))
-   `completed`, na query string, (boolean, se estiver presente, é feita uma pesquisa apenas pelas tarefas com o estado pretendido)

Os parâmetros `description` e `completed` são cumulativos, isto é, o seguinte link `/api/todos/afecarvalho?completed=true&description=olá` iria buscar as tarefas concluídas e com o texto `olá` na descrição.

#### Resultado:

-   O array das tarefas do utilizador. Se o utilizador não tiver tarefas, ou não existir nenhuma tarefa que corresponde aos parâmetros opcionais, este array vem vazio.

```jsonc
[
    {
        "id": 1,
        "description": "Olá, Mundo!",
        "userName": "afecarvalho",
        "isComplete": false,
        "addedAt": "2019-03-23T11:19:05.5380968+00:00",
        "lastUpdatedAt": "2019-03-23T11:19:05.5388266+00:00"
    },
    {
        "id": 2,
        "description": "Tarefa editada",
        "userName": "afecarvalho",
        "isComplete": true,
        "addedAt": "2019-03-23T11:55:11.0840147+00:00",
        "lastUpdatedAt": "2019-03-23T12:02:16.3476088+00:00"
    }
]
```

### Criar uma tarefa: `POST /api/todos/{user}`

Cria uma tarefa na base de dados para esse utilizador.

#### Parâmetros:

-   Username, no URL (substituir `{user}` pelo user pretendido)
-   Dados, no body, como JSON:

```jsonc
{
    "description": "Nova tarefa a ser criada" // Descrição da tarefa (string, obrigatório, máx. 512 chars)
}
```

#### Resultado:

-   Objeto da tarefa criada, como JSON, com o ID, informação do user e datas de criação.

```jsonc
{
    "id": 2,
    "description": "Nova tarefa a ser criada",
    "userName": "afecarvalho",
    "isComplete": false,
    "addedAt": "2019-03-23T11:55:11.0840147+00:00",
    "lastUpdatedAt": "2019-03-23T11:55:11.0840774+00:00"
}
```

### Editar uma tarefa: `PUT /api/todos/{user}/{id}`

Atualiza uma tarefa na base de dados, dado o utilizador e o ID da tarefa. Este método pode ser usado para marcar tarefas como concluídas, ou alterar as suas descrições.

#### Parâmetros:

-   Username, no URL (substituir `{user}` pelo user pretendido)
-   ID da tarefa, no URL (substituir `{id}` pelo ID pretendido)
-   Dados da tarefa, no body, como JSON:

```jsonc
{
    "description": "Tarefa editada", // Descrição da tarefa (string, obrigatório, máx. 512 chars)
    "isComplete": true // Se está completo, ou não (boolean, obrigatório)
}
```

#### Resultado:

-   Objeto da tarefa editada, como JSON, com os dados alterados.
-   Se a tarefa não existir, ou não for do user: 404 (Not Found)

```jsonc
{
    "id": 2,
    "description": "Tarefa editada",
    "userName": "afecarvalho",
    "isComplete": true,
    "addedAt": "2019-03-23T11:55:11.0840147+00:00",
    "lastUpdatedAt": "2019-03-23T12:02:16.3476088+00:00" // A data de edição é atualizada a cada edição
}
```

### Eliminar uma tarefa: `DELETE /api/todos/{user}/{id}`

Atualiza uma tarefa na base de dados, dado o utilizador e o ID da tarefa.

#### Parâmetros:

-   Username, no URL (substituir `{user}` pelo user pretendido)
-   ID da tarefa, no URL (substituir `{id}` pelo ID pretendido)

#### Resultado:

-   Se a tarefa for apagada corretamente: nada (204 No Content)
-   Se a tarefa não existir, ou não for do user: 404 (Not Found)
