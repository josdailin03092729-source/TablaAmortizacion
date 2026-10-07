
using System;
using Spectre.Console;

class Program
{
    static void Main()
    {
        // Título de la aplicación
        AnsiConsole.Write(
            new FigletText("AMORTIZACION")
                .Centered()
                .Color(Color.Green));

        AnsiConsole.MarkupLine(
            "[yellow]Calculadora de préstamo con cuotas fijas[/]\n");

        // Entrada de datos
        decimal monto = AnsiConsole.Ask<decimal>(
            "[green]Ingrese el monto del préstamo:[/]");

        decimal tasaAnual = AnsiConsole.Ask<decimal>(
            "[green]Ingrese la tasa de interés anual (%):[/]");

        int plazo = AnsiConsole.Ask<int>(
            "[green]Ingrese el plazo del préstamo en meses:[/]");

        // Cálculo de la tasa mensual
        decimal tasaMensual = tasaAnual / 12 / 100;

        // Cálculo de la cuota fija
        decimal cuota;

        if (tasaMensual == 0)
        {
            // Caso en que el préstamo no tiene intereses
            cuota = monto / plazo;
        }
        else
        {
            decimal factor = (decimal)Math.Pow(
                (double)(1 + tasaMensual),
                plazo);

            cuota = monto *
                    (tasaMensual * factor) /
                    (factor - 1);
        }

        // Crear tabla
        Table tabla = new Table();

        tabla.Border(TableBorder.Rounded);

        tabla.AddColumn("[yellow]No. Cuota[/]");
        tabla.AddColumn("[yellow]Pago de Cuota[/]");
        tabla.AddColumn("[yellow]Interés[/]");
        tabla.AddColumn("[yellow]Abono a Capital[/]");
        tabla.AddColumn("[yellow]Saldo Pendiente[/]");

        // Saldo inicial
        decimal saldo = monto;

        // Generar tabla mediante un ciclo for
        for (int numeroCuota = 1; numeroCuota <= plazo; numeroCuota++)
        {
            // Calcular interés del período
            decimal interes = saldo * tasaMensual;

            // Calcular abono a capital
            decimal abonoCapital = cuota - interes;

            // En la última cuota ajustamos los valores
            // para evitar pequeños errores de redondeo
            decimal pagoActual = cuota;

            if (numeroCuota == plazo)
            {
                abonoCapital = saldo;
                pagoActual = interes + abonoCapital;
            }

            // Actualizar saldo
            saldo -= abonoCapital;

            if (saldo < 0)
            {
                saldo = 0;
            }

            // Agregar fila a la tabla
            tabla.AddRow(
                numeroCuota.ToString(),
                pagoActual.ToString("N2"),
                interes.ToString("N2"),
                abonoCapital.ToString("N2"),
                saldo.ToString("N2")
            );
        }

        // Mostrar información de la cuota
        AnsiConsole.WriteLine();

        AnsiConsole.Write(
            new Panel(
                $"[bold]Cuota fija mensual:[/] [green]{cuota:N2}[/]\n" +
                $"[bold]Tasa mensual:[/] [green]{tasaMensual:P4}[/]\n" +
                $"[bold]Total de meses:[/] [green]{plazo}[/]")
            .Header("Resumen del préstamo")
            .Border(BoxBorder.Rounded));

        AnsiConsole.WriteLine();

        // Mostrar tabla completa
        AnsiConsole.Write(tabla);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(
            "[bold green]Cálculo de amortización completado.[/]");
    }
}