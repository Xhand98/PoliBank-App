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

1. **Install ODBC Driver**: Ensure you have the appropriate ODBC driver installed for your database. For example, for SQL Server, you can download the ODBC driver from the [Microsoft website](https://docs.microsoft.com/en-us/sql/connect/odbc/download-odbc-driver-for-sql-server).
2. **Set Up ODBC Data Source**: Configure an ODBC Data Source Name (DSN) on your system:

   - On Windows, open the ODBC Data Source Administrator and add a new DSN.
   - On macOS or Linux, configure the DSN in the `odbc.ini` file.
3. **Update Connection String**: In the `Program.cs` file, update the connection string with your DSN:

   ```cs
   private static Database _database = new Database("DSN=YourDSNName");
   ```
4. **Database Class**: The `Database` class in `DBHelper.cs` handles all database operations using ODBC. Here is an example of how to use the `Database` class:

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

## Database Diagram

Below is a diagram representing the database schema for the PoliBank-App:
![Diagrama](repo-assets\PoliBank-App.svg)

### Database Setup

To set up the database, follow these steps:

1. **Run the MasterSQL Script**: The `MasterSQL.sql` file contains all the necessary SQL commands to create and populate the database. Run the script using your preferred SQL client (e.g., SQL Server Management Studio, Azure Data Studio).

   ```sql
   -- SCRIPT BASE DE DATOS
   -- Create the database
   CREATE DATABASE [Pomaray Bank];
   GO
   -- Use the created database
   USE [Pomaray Bank];
   GO
   -- Create the Clientes table
   CREATE TABLE Clientes (
       ClienteID INT PRIMARY KEY IDENTITY(1,1),
       ```sql
       Primer_Nombre VARCHAR(100) NOT NULL,
       Segundo_Nombre VARCHAR(100),
       Primer_Apellido VARCHAR(100) NOT NULL,
       Segundo_Apellido VARCHAR(100),
       Email VARCHAR(100)
       );
       GO
       -- Create the TiposCuentas table
       CREATE TABLE TiposCuentas (
           TipoCuentaID INT PRIMARY KEY IDENTITY(1,1),
           TipoCuenta VARCHAR(50)
       );
       GO
       -- Create the TiposTransacciones table
       CREATE TABLE TiposTransacciones (
           TipoTransaccionID INT PRIMARY KEY IDENTITY(1,1),
           TipoTransaccion VARCHAR(50)
       );
       GO
       -- Create the Cuentas table
       CREATE TABLE Cuentas (
           CuentaID INT PRIMARY KEY IDENTITY(1,1),
           ClienteID INT,
           TipoCuentaID INT,
           Saldo DECIMAL(10, 2) DEFAULT 0.00,
           FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID),
           FOREIGN KEY (TipoCuentaID) REFERENCES TiposCuentas(TipoCuentaID)
       );
       GO
       -- Create the Contactos table
       CREATE TABLE Contactos (
           ContactoID INT PRIMARY KEY IDENTITY(1,1),
           ClienteID INT,
           Direccion VARCHAR(255),
           FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
       );
       GO
       ```
   ```

This project is licensed under the [MIT License](LICENSE)
