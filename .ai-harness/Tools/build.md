# Tools — Build

## Genérico / arquivos

| ID | Ação | Notas |
|----|------|-------|
| `fs.read` | Ler arquivos | auto |
| `fs.write` | Editar no escopo | auto |
| `fs.delete` | Apagar arquivo | **ask** |
| `fs.write.secrets` | Secrets | **deny** |

## .NET

| ID | Comando | Notas |
|----|---------|-------|
| `dotnet.restore` | `dotnet restore` | auto |
| `dotnet.build` | `dotnet build` | auto |
| `dotnet.format` | `dotnet format` | auto escopo |
| `dotnet.add.package` | `dotnet add package` | **ask** |
