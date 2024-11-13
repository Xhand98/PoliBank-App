# PoliBank-App

PoliBank-App is a simple banking application developed as part of a school presentation. The application allows users to manage clients, accounts, and transactions.

## Features

- **Client Management**: Add, search, update, and delete clients.
- **Account Management**: Manage client accounts.
- **Transaction Management**: Handle transactions between accounts.
- **Interest Generation**: Generate interest for accounts.

## Technologies Used

- C#
- .NET
- SQL

## Getting Started

### ODBC Configuration

To configure and use ODBC with the application, follow these steps:

1. **Install ODBC Driver**:Ensure you have the appropriate ODBC driver installed for your database. For example, for SQL Server, you can download the ODBC driver from the [Microsoft website](https://docs.microsoft.com/en-us/sql/connect/odbc/download-odbc-driver-for-sql-server).
2. **Set Up ODBC Data Source**:Configure an ODBC Data Source Name (DSN) on your system:

   - On Windows, open the ODBC Data Source Administrator and add a new DSN.
   - On macOS or Linux, configure the DSN in the `odbc.ini` file.
3. **Update Connection String**:In the `Program.cs` file, update the connection string with your DSN:

   ```cs
   private static Database _database = new Database("DSN=YourDSNName");
   ```
4. **Database Class**:The `Database` class in `DBHelper.cs` handles all database operations using ODBC. Here is an example of how to use the `Database` class:

   ```cs
   public class Database
   {
       private readonly string _connectionString;

       public Database(string connectionString)
       {
           _connectionString = connectionString;
       }

       public void AddCliente(string primerNombre, string? segundoNombre, string primerApellido, string? segundoApellido, string? email)
       {
           using (var connection = new OdbcConnection(_connectionString))
           {
               string query = @$"INSERT INTO Clientes (Primer_Nombre, Segundo_Nombre, Primer_Apellido, Segundo_Apellido, Email)
                                VALUES ({primerNombre}, {segundoNombre}, {primerApellido}, {segundoApellido}, {email})";
               connection.Execute(query);
           }
       }
       // Other methods...
   }
   ```
5. **Running the Application**:After configuring the ODBC connection, build and run the application:

   ```sh
   dotnet build
   dotnet run
   ```
6. **Usage**:
   Upon running the application, you will be prompted to log in. After a successful login, you can navigate through the menu to manage clients, accounts, and transactions using the ODBC connection.

For more details on configuring ODBC, refer to the official documentation of your database and ODBC driver.

### Prerequisites

- .NET SDK
- SQL Server

### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/Xhand98/PoliBank-App.git
   ```
2. Open the solution in your preferred IDE (e.g., Visual Studio, Rider).
3. Restore the NuGet packages:
   ```sh
   dotnet restore
   ```
4. Update the database connection string in `Program.cs`:
   ```cs
   private static Database _database = new Database("YourConnectionStringHere");
   ```

### Running the Application

1. Build the solution:
   ```sh
   dotnet build
   ```
2. Run the application:
   ```sh
   dotnet run
   ```

## Usage

Upon running the application, you will be prompted to log in. After a successful login, you can navigate through the menu to manage clients, accounts, and transactions.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.
