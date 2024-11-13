using System.Data;
using System.Data.Odbc;
using Dapper;

// ODBC

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
    
    public void UpdateCuentaField(int cuentaId, string campo, object nuevoValor)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            connection.Open();
            using (var command = new OdbcCommand($"UPDATE Cuentas SET {campo} = ? WHERE CuentaID = ?", connection))
            {
                command.Parameters.AddWithValue("@nuevoValor", nuevoValor);
                command.Parameters.AddWithValue("@cuentaId", cuentaId);
                command.ExecuteNonQuery();
            }
        }
    }

    
    public void UpdateClienteField(int clienteId, string campo, string nuevoValor)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            string query = @$"UPDATE Clientes SET {campo} = ? WHERE ClienteID = ?";
            connection.Execute(query, new { nuevoValor, clienteId });
        }
    }

    
    

    public dynamic GetCliente(string numeroTarjeta)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            string query = @$"
            SELECT 
                c.ClienteID,
                c.Primer_Nombre,
                c.Segundo_Nombre,
                c.Primer_Apellido,
                c.Segundo_Apellido,
                c.Email,
                d.Direccion
            FROM 
                Clientes AS c
            JOIN 
                TarjetasCredito AS t ON c.ClienteID = t.ClienteID
            JOIN 
                Contactos AS d ON c.ClienteID = d.ClienteID
            WHERE 
                t.NumeroTarjeta = '{numeroTarjeta}'";

            return connection.QueryFirstOrDefault(query);
        }
    }

    

    public void DeleteCliente(int clienteId)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            string query = @$"DELETE FROM Clientes WHERE ClienteID = '{clienteId}'";
            connection.Execute(query);
        }
    }

    public void AddCuenta(int clienteId, int tipoCuentaId)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            string query = @$"INSERT INTO Cuentas (ClienteID, TipoCuentaID, Saldo) 
                             VALUES ({clienteId}, {tipoCuentaId}, 0)";
            connection.Execute(query);
        }
    }

    public dynamic GetCuenta(int cuentaId)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            string query = @$"
            SELECT 
                c.CuentaID,
                c.Saldo,
                t.TipoCuenta AS TipoCuenta,
                cl.Primer_Nombre,
                cl.Primer_Apellido
            FROM 
                Cuentas AS c
            JOIN 
                TiposCuentas AS t ON c.TipoCuentaID = t.TipoCuentaID
            JOIN 
                Clientes AS cl ON c.ClienteID = cl.ClienteID
            WHERE 
                c.CuentaID = {cuentaId}";

            return connection.QueryFirstOrDefault(query);
        }
    }
    
    public void DeleteCuenta(int cuentaId)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            string query = @$"DELETE FROM Cuentas WHERE CuentaID = {cuentaId}";
            connection.Execute(query);
        }
    }
    
    public AdminUser GetAdminUser(string username, string password)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            string query = @"SELECT * FROM AdminUser WHERE username = ? AND password = ?";
            return connection.QueryFirstOrDefault<AdminUser>(query, new { username, password });
        }
    }
    
    public void Transferir(int cuentaOrigenId, int cuentaDestinoId, decimal cantidad)
{
    using (var connection = new OdbcConnection(_connectionString))
    {
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                var queryOrigen = "UPDATE Cuentas SET Saldo = Saldo - ? WHERE CuentaID = ?";
                connection.Execute(queryOrigen, new { Cantidad = cantidad, CuentaID = cuentaOrigenId }, transaction);

                var queryDestino = "UPDATE Cuentas SET Saldo = Saldo + ? WHERE CuentaID = ?";
                connection.Execute(queryDestino, new { Cantidad = cantidad, CuentaID = cuentaDestinoId }, transaction);
                
                var tipoTransaccionId = 3;
                RegistrarTransaccion(cuentaOrigenId, -cantidad, tipoTransaccionId, transaction); 
                RegistrarTransaccion(cuentaDestinoId, cantidad, tipoTransaccionId, transaction); 

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

    public void RegistrarTransaccion(int cuentaId, decimal monto, int tipoTransaccionId, IDbTransaction transaction)
    {
        var query = "INSERT INTO Transacciones (CuentaID, Fecha, Monto, TipoTransaccionID) VALUES (?, ?, ?, ?)";
        var fecha = DateTime.Now;

        using (var command = new OdbcCommand(query, (OdbcConnection)transaction.Connection))
        {
            command.Transaction = (OdbcTransaction)transaction; // Realiza el casting aquí
            command.Parameters.AddWithValue("@CuentaID", cuentaId);
            command.Parameters.AddWithValue("@Fecha", fecha);
            command.Parameters.AddWithValue("@Monto", monto);
            command.Parameters.AddWithValue("@TipoTransaccionID", tipoTransaccionId);
            command.ExecuteNonQuery();
        }
    }


     
    public void Depositar(int cuentaId, decimal cantidad)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            connection.Open();
            using (var command = new OdbcCommand("UPDATE Cuentas SET Saldo = Saldo + ? WHERE CuentaID = ?", connection))
            {
                command.Parameters.AddWithValue("@Cantidad", cantidad);
                command.Parameters.AddWithValue("@CuentaID", cuentaId);
                command.ExecuteNonQuery();
            }
        }
    }
    
    public void Retirar(int cuentaId, decimal cantidad)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            connection.Open();
            using (var command = new OdbcCommand("UPDATE Cuentas SET Saldo = Saldo - ? WHERE CuentaID = ?", connection))
            {
                command.Parameters.AddWithValue("@Cantidad", cantidad);
                command.Parameters.AddWithValue("@CuentaID", cuentaId);
                command.ExecuteNonQuery();
            }
        }
    }

    public void PagarTarjetaCredito(string tarjetaId, decimal cantidad)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            var query = $"UPDATE TarjetasCredito SET Saldo = Saldo - {cantidad} WHERE Id = {tarjetaId}";
            connection.Execute(query);
        }
    }

    public void PagarServicios(int cuentaId, decimal cantidad)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            var query = $"UPDATE Cuentas SET Saldo = Saldo - {cantidad} WHERE Id = '{cuentaId}'";
            connection.Execute(query, new { CuentaId = cuentaId, Cantidad = cantidad });
        }
    }

    public void GenerarInteres(int cuentaId, decimal tasaInteres)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            var query = $"UPDATE Cuentas SET Saldo = Saldo + (Saldo * @TasaInteres / 100) WHERE Id = '{cuentaId}'";
            connection.Execute(query, new { CuentaId = cuentaId, TasaInteres = tasaInteres });
        }
    }
    
    public void AddSaldo(int cuentaId, decimal cantidad)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            var query = $"UPDATE Cuentas SET Saldo = Saldo + {cantidad} WHERE Id = {cuentaId}";
            connection.Execute(query, new { CuentaId = cuentaId, Cantidad = cantidad });
        }
    }


    public void AjustarCuenta(int cuentaId, decimal cantidad)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            var query = $"UPDATE Cuentas SET Saldo = {cantidad} WHERE Id = '{cuentaId}'";
            connection.Execute(query, new { CuentaId = cuentaId, Cantidad = cantidad });
        }
    }

    public IEnumerable<Transaccion> GetHistorialTransacciones(int cuentaId)
    {
        using (var connection = new OdbcConnection(_connectionString))
        {
            var query = $@"
            SELECT 
                t.Fecha, 
                tt.TipoTransaccion AS Tipo, 
                t.Monto 
            FROM Transacciones t
            INNER JOIN TiposTransacciones tt ON t.TipoTransaccionID = tt.TipoTransaccionID
            WHERE t.CuentaID = '{cuentaId}' 
            ORDER BY t.Fecha DESC";
        
            return connection.Query<Transaccion>(query);
        }
    }

}


public class Transaccion
{
    public DateTime Fecha { get; set; }
    public string Tipo { get; }
    public decimal Monto { get; set; }
    public decimal SaldoFinal { get; set; }
}
public class AdminUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class Cliente
{
    public int ClienteID { get; set; }
    public string Primer_Nombre { get; set; }
    public string Segundo_Nombre { get; set; }
    public string Primer_Apellido { get; set; }
    public string Segundo_Apellido { get; set; }
    public string Email { get; set; }
    public string Direccion { get; set;  }
}
