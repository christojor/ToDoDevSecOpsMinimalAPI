# ToDo DevSecOps Minimal API

## Project Description
A minimal ToDo web API demonstrating secure DevOps practices, using .NET 9 and a layered architecture for application, domain, and infrastructure concerns. The project includes unit tests and a security analysis document.

## Running Locally
### Prerequisites
- Install the .NET 9 SDK: [Download .NET 9 SDK](https://dotnet.microsoft.com/download)
- Optional: Visual Studio or Visual Studio Code with C# extensions

### Steps
1. Clone the repository and open a terminal in the repository root.
2. Restore packages: 
   ```bash
   dotnet restore
   ```
3. Build the solution: 
   ```bash
   dotnet build
   ```
4. Run the API project (from the project folder that contains `Program.cs`): 
   ```bash
   dotnet run
   ```
5. Open the API in a browser at the URL shown in the console (typically `https://localhost:5001` or `http://localhost:5000`).

## Running Tests
To run all tests from the solution root, use the following command:
```bash
dotnet test
```

To run a specific test project, specify its path:
```bash
dotnet test ./ToDoDevSecOpsMinimalAPI.Tests/ToDoDevSecOpsMinimalAPI.Tests.csproj
```

## Security Requirements
See the full security requirements and analysis: [Security Requirements](SECURITY-ANALYSIS.md)