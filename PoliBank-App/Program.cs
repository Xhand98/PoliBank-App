class Program
{
    private static Database _database = new Database("DSN=ExpoJL");

    static void Main(string[] args)
    {
        if (!Login())
        {
            Console.WriteLine("Acceso denegado. Cerrando el programa...");
            return;
        }

        while (true)
        {
            Console.WriteLine(
                @"Bienvenido a PomarayBank!
Seleccione una opción:
1. Gestión de Clientes
2. Gestión de Cuentas
3. Gestión de Transacciones
4. Salir"
            );

            switch (Console.ReadLine())
            {
                case "1":
                    MenuGestionClientes();
                    break;
                case "2":
                    MenuGestionCuentas();
                    break;
                case "3":
                    MenuGestionTransacciones();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static bool Login()
    {
        Console.Write("Ingrese su nombre de usuario: ");
        string username = Console.ReadLine();

        Console.Write("Ingrese su contraseña: ");
        string password = Console.ReadLine();

        var user = _database.GetAdminUser(username, password);
        
        if (user != null)
        {
            Console.WriteLine("Inicio de sesión exitoso.");
            Console.Clear();
            return true;
        }
        else
        {
            Console.WriteLine("Nombre de usuario o contraseña incorrectos.");
            return false;
        }
    }

    static void MenuGestionClientes()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine(
@"Gestión de Clientes:
Seleccione una opción:
1. Agregar Cliente
2. Buscar Cliente
3. Actualizar Cliente
4. Eliminar Cliente
5. Volver al Menú Principal"
            );

            switch (Console.ReadLine())
            {
                case "1":
                    dbHelper.AddCliente();
                    break;
                case "2":
                    dbHelper.SearchCliente();
                    break;
                case "3":
                    dbHelper.UpdateCliente();
                    break;
                case "4":
                    dbHelper.DeleteCliente();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void MenuGestionCuentas()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine(
                @"Gestión de Cuentas:
Seleccione una opción:
1. Agregar Cuenta
2. Buscar Cuenta
3. Actualizar Cuenta
4. Eliminar Cuenta
5. Añadir Saldo a Cuenta
6. Volver al Menú Principal"
            );

            switch (Console.ReadLine())
            {
                case "1":
                    dbHelper.AddCuenta();
                    break;
                case "2":
                    dbHelper.SearchCuenta();
                    break;
                case "3":
                    dbHelper.UpdateCuenta();
                    break;
                case "4":
                    dbHelper.DeleteCuenta();
                    break;
                case "5":
                    dbHelper.AddSaldoCuenta();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void MenuGestionTransacciones()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine(
                @"Gestión de Transacciones:
Seleccione una opción:
1. Depósito
2. Retiro
3. Transferencia
4. Pago de Tarjeta de Crédito
5. Pago de Servicios
6. Interés Generado
7. Comisión por Retiro
8. Ajuste de Cuenta
9. Ver Historial de Transacciones
10. Volver al Menú Principal"
            );

            switch (Console.ReadLine())
            {
                case "1":
                    dbHelper.RealizarDeposito();
                    break;
                case "2":
                    dbHelper.RealizarRetiro();
                    break;
                case "3":
                    dbHelper.RealizarTransferencia();
                    break;
                case "4":
                    dbHelper.RealizarPagoTarjetaCredito();
                    break;
                case "5":
                    dbHelper.RealizarPagoServicios();
                    break;
                case "6":
                    dbHelper.GenerarInteres();
                    break;
                case "7":
                    dbHelper.RealizarComisionRetiro();
                    break;
                case "8":
                    dbHelper.AjustarCuenta();
                    break;
                case "9":
                    dbHelper.VerHistorialTransacciones();
                    break;
                case "10":
                    return;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }

            Console.WriteLine();
        }
    }
}

class dbHelper
{
    private static Database _database = new Database("DSN=ExpoBank");

    public static void AddCliente()
    {
        Console.Write("Primer Nombre: ");
        string primerNombre = Console.ReadLine();
        Console.Write("Segundo Nombre: ");
        string? segundoNombre = Console.ReadLine();
        Console.Write("Primer Apellido: ");
        string primerApellido = Console.ReadLine();
        Console.Write("Segundo Apellido: ");
        string? segundoApellido = Console.ReadLine();
        Console.Write("Email: ");
        string? email = Console.ReadLine();

        _database.AddCliente(primerNombre, segundoNombre, primerApellido, segundoApellido, email);
        Console.WriteLine("Cliente agregado con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void SearchCliente()
    {
        Console.Write("Introducir número de tarjeta del cliente: ");
        string numeroTarjeta = Console.ReadLine();
    
        var cliente = _database.GetCliente(numeroTarjeta);

        Console.WriteLine("Buscando...");
        Thread.Sleep(1300);
        if (cliente != null)
        {
            Console.WriteLine(
@$"Usuario nº:{cliente.ClienteID}
Nombre: {cliente.Primer_Nombre} {cliente.Segundo_Nombre} {cliente.Primer_Apellido} {cliente.Segundo_Nombre}
Correo: {cliente.Email}
Direccion: {cliente.Direccion}
"
);
            Console.WriteLine("Presione cualquier tecla para volver.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Cliente no encontrado.");
            Console.WriteLine("Presione cualquier tecla para volver.");
            Console.ReadKey();
        }
    }

    public static void UpdateCliente()
{
    Console.Write("Ingrese el ID del Cliente a actualizar: ");
    int clienteId = int.Parse(Console.ReadLine());

    Console.Clear();
    while (true)
    {
        Console.WriteLine(
@"Seleccione el dato a actualizar:
1. Primer Nombre
2. Segundo Nombre
3. Primer Apellido
4. Segundo Apellido
5. Email
6. Volver al Menú de Clientes"
        );

        string nuevoValor;
        switch (Console.ReadLine())
        {
            case "1":
                Console.Write("Nuevo Primer Nombre: ");
                nuevoValor = Console.ReadLine();
                _database.UpdateClienteField(clienteId, "Primer_Nombre", nuevoValor);
                Console.WriteLine("Primer Nombre actualizado con éxito.");
                break;
            case "2":
                Console.Write("Nuevo Segundo Nombre: ");
                nuevoValor = Console.ReadLine();
                _database.UpdateClienteField(clienteId, "Segundo_Nombre", nuevoValor);
                Console.WriteLine("Segundo Nombre actualizado con éxito.");
                break;
            case "3":
                Console.Write("Nuevo Primer Apellido: ");
                nuevoValor = Console.ReadLine();
                _database.UpdateClienteField(clienteId, "Primer_Apellido", nuevoValor);
                Console.WriteLine("Primer Apellido actualizado con éxito.");
                break;
            case "4":
                Console.Write("Nuevo Segundo Apellido: ");
                nuevoValor = Console.ReadLine();
                _database.UpdateClienteField(clienteId, "Segundo_Apellido", nuevoValor);
                Console.WriteLine("Segundo Apellido actualizado con éxito.");
                break;
            case "5":
                Console.Write("Nuevo Email: ");
                nuevoValor = Console.ReadLine();
                _database.UpdateClienteField(clienteId, "Email", nuevoValor);
                Console.WriteLine("Email actualizado con éxito.");
                break;
            case "6":
                return;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }

        Console.WriteLine();
    }
}


    public static void DeleteCliente()
    {
        Console.Write("Ingrese el ID del Cliente a eliminar: ");
        int clienteId = int.Parse(Console.ReadLine());

        _database.DeleteCliente(clienteId);
        Console.WriteLine("Cliente eliminado con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void AddCuenta()
    {
        Console.Write("Cliente ID: ");
        int clienteId = int.Parse(Console.ReadLine());
        Console.Write("Tipo de Cuenta ID: ");
        int tipoCuentaId = int.Parse(Console.ReadLine());

        _database.AddCuenta(clienteId, tipoCuentaId);
        Console.WriteLine("Cuenta agregada con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void SearchCuenta()
    {
        Console.Write("Ingrese el ID de la cuenta: ");
        int cuentaId = int.Parse(Console.ReadLine());

        Console.Write("Buscando...");
        Thread.Sleep(1300);
        var cuenta = _database.GetCuenta(cuentaId);

        if (cuenta != null)
        {
            Console.WriteLine(
@$"Cuenta nº: {cuenta.CuentaID}
Tipo de Cuenta: {cuenta.TipoCuenta}
Saldo: {cuenta.Saldo}
Cliente: {cuenta.Primer_Nombre} {cuenta.Primer_Apellido}");
            Console.WriteLine("Presione cualquier tecla para volver.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Cuenta no encontrada.");
            Console.WriteLine("Presione cualquier tecla para volver.");
            Console.ReadKey();
        }
    }

    public static void UpdateCuenta()
    {
        Console.Write("Ingrese el ID de la cuenta a actualizar: ");
        if (!int.TryParse(Console.ReadLine(), out int cuentaId))
        {
            Console.WriteLine("ID de cuenta inválido.");
            return;
        }

        Console.WriteLine("Seleccione el dato a actualizar:");
        Console.WriteLine("1. Saldo");
        Console.WriteLine("2. Tipo de Cuenta ID");
        Console.WriteLine("3. Volver al Menú de Cuentas");

        var opcion = Console.ReadLine();

        switch (opcion)
        {
            case "1":
                Console.Write("Nuevo Saldo: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal nuevoSaldo))
                {
                    _database.UpdateCuentaField(cuentaId, "Saldo", nuevoSaldo);
                    Console.WriteLine("Saldo actualizado correctamente.");
                }
                else
                {
                    Console.WriteLine("Valor de saldo inválido.");
                }
                break;

            case "2":
                Console.Write("Nuevo Tipo de Cuenta ID: ");
                if (int.TryParse(Console.ReadLine(), out int nuevoTipoCuentaId))
                {
                    _database.UpdateCuentaField(cuentaId, "TipoCuentaID", nuevoTipoCuentaId);
                    Console.WriteLine("Tipo de Cuenta actualizado correctamente.");
                }
                else
                {
                    Console.WriteLine("ID de Tipo de Cuenta inválido.");
                }
                break;

            case "3":
                Console.WriteLine("Volviendo al menú de cuentas.");
                break;

            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }


    public static void DeleteCuenta()
    {
        Console.Write("Ingrese el número de cuenta a eliminar: ");
        int cuentaId = int.Parse(Console.ReadLine());

        _database.DeleteCuenta(cuentaId);
        Console.WriteLine("Cuenta eliminada con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void AddSaldoCuenta()
    {
        Console.Write("Ingrese el número de cuenta: ");
        int cuentaId = int.Parse(Console.ReadLine());
        Console.Write("Ingrese la cantidad a añadir: ");
        decimal cantidad = decimal.Parse(Console.ReadLine());

        _database.AddSaldo(cuentaId, cantidad);
        Console.WriteLine("Saldo añadido con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void RealizarDeposito()
    {
        Console.Write("Ingrese el número de cuenta: ");
        int cuentaId = int.Parse(Console.ReadLine());
        Console.Write("Ingrese la cantidad a depositar: ");
        decimal cantidad = decimal.Parse(Console.ReadLine());

        _database.Depositar(cuentaId, cantidad);
        Console.WriteLine("Depósito realizado con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void RealizarRetiro()
    {
        Console.Write("Ingrese el número de cuenta: ");
        int cuentaId = int.Parse(Console.ReadLine());
        Console.Write("Ingrese la cantidad a retirar: ");
        decimal cantidad = decimal.Parse(Console.ReadLine());

        _database.Retirar(cuentaId, cantidad);
        Console.WriteLine("Retiro realizado con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void RealizarTransferencia()
    {
        Console.Write("Ingrese el número de cuenta de origen: ");
        int cuentaOrigenId = int.Parse(Console.ReadLine());
        Console.Write("Ingrese el número de cuenta de destino: ");
        int cuentaDestinoId = int.Parse(Console.ReadLine());
        Console.Write("Ingrese la cantidad a transferir: ");
        decimal cantidad = decimal.Parse(Console.ReadLine());

        _database.Transferir(cuentaOrigenId, cuentaDestinoId, cantidad);
        Console.WriteLine("Transferencia realizada con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void RealizarPagoTarjetaCredito()
    {
        Console.Write("Ingrese el número de tarjeta de crédito: ");
        string tarjetaId = Console.ReadLine();
        Console.Write("Ingrese la cantidad a pagar: ");
        decimal cantidad = decimal.Parse(Console.ReadLine());

        _database.PagarTarjetaCredito(tarjetaId, cantidad);
        Console.WriteLine("Pago realizado con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void RealizarPagoServicios()
    {
        Console.Write("Ingrese el número de cuenta: ");
        int cuentaId = int.Parse(Console.ReadLine());
        Console.Write("Ingrese la cantidad a pagar: ");
        decimal cantidad = decimal.Parse(Console.ReadLine());

        _database.PagarServicios(cuentaId, cantidad);
        Console.WriteLine("Pago de servicios realizado con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void GenerarInteres()
    {
        Console.Write("Ingrese el ID de la cuenta: ");
        int cuentaId = int.Parse(Console.ReadLine());
        Console.Write("Ingrese la tasa de interés: ");
        decimal tasaInteres = decimal.Parse(Console.ReadLine());

        _database.GenerarInteres(cuentaId, tasaInteres);
        Console.WriteLine("Intereses generados con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void RealizarComisionRetiro()
    {
        Console.Write("Ingrese el número de cuenta: ");
        int cuentaId = int.Parse(Console.ReadLine());
        Console.Write("Ingrese la cantidad a retirar (incluyendo comisiones): ");
        decimal cantidad = decimal.Parse(Console.ReadLine());

        _database.Retirar(cuentaId, cantidad);
        Console.WriteLine("Comisión por retiro aplicada con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void AjustarCuenta()
    {
        Console.Write("Ingrese el número de cuenta: ");
        int cuentaId = int.Parse(Console.ReadLine());
        Console.Write("Ingrese la cantidad a ajustar: ");
        decimal cantidad = decimal.Parse(Console.ReadLine());

        _database.AjustarCuenta(cuentaId, cantidad);
        Console.WriteLine("Cuenta ajustada con éxito.");
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }

    public static void VerHistorialTransacciones()
    {
        Console.Write("Ingrese el número de cuenta: ");
        int cuentaId = int.Parse(Console.ReadLine());

        var transacciones = _database.GetHistorialTransacciones(cuentaId);
        Console.WriteLine("Historial de transacciones:");
        foreach (var transaccion in transacciones)
        {
            Console.WriteLine(
@$"Fecha: {transaccion.Fecha}, 
Tipo: {transaccion.Tipo}, 
Monto: {transaccion.Monto}");
        }
        Console.WriteLine("Presione cualquier tecla para volver.");
        Console.ReadKey();
    }
}
