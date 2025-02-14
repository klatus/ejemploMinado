//      _________      __ __ __      __                  ___   ____ ___   ______
//     _/_/ ____/ |   / //_// /___ _/ /___  _______     |__ \ / __ \__ \ / ____/
//    / // /    / /  / ,<  / / __ `/ __/ / / / ___/     __/ // / / /_/ //___ \  
//   / // /___ / /  / /| |/ / /_/ / /_/ /_/ (__  )     / __// /_/ / __/____/ /  
//  / / \____//_/  /_/ |_/_/\__,_/\__/\__,_/____( )   /____/\____/____/_____/   
//  |_|     /_/                                 |/                              

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Spectre.Console;

AnsiConsole.MarkupLine("[bold darkgreen](C) 2025 Klatus[/], Ejemplo de minado\n");

DateTime now = DateTime.Now;
string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
AnsiConsole.MarkupLine($"Hora inicial: \t\t[red]{formattedTime}[/]");
string feed = formattedTime.ToString();

// Parámetros de entrada
int longTest = 4;
if (args.Length > 0 && int.TryParse(args[0], out int parsedValue))
{
    longTest = parsedValue;
}
AnsiConsole.MarkupLine($"Complejidad: \t\t[yellow]{longTest}[/]");

// Cadena de comparación para la prueba de trabajo
string comparisonString = new string('0', longTest);

// Iniciar el cronómetro
Stopwatch stopwatch = Stopwatch.StartNew();

// Genera un hash candidato
string hash = ComputeSha256Hash(feed);

// Realiza la prueba de trabajo
int tries = 0;
// Spinner
AnsiConsole.Status()
    .Spinner(Spinner.Known.Clock)
    .Start($"Minando... ", ctx =>
    {
        while (!hash.EndsWith(comparisonString))
        {
            tries++;
            hash = ComputeSha256Hash(hash);
        }
    });

// Número de intentos
AnsiConsole.MarkupLine($"Intentos: \t\t[yellow]{tries}[/]");

// Hash encontrado
AnsiConsole.MarkupLine($"Hash encontrado: \t[green4]{hash}[/]");

// Detener el cronómetro
stopwatch.Stop();
AnsiConsole.MarkupLine($"Tiempo de ejecución: \t[slowblink blue]{stopwatch.ElapsedMilliseconds} ms[/]");

string ComputeSha256Hash(string rawData)
{    
    // Crear una instancia de SHA256
    using (SHA256 sha256Hash = SHA256.Create())
    {
        // Computar el hash - devuelve un array de bytes
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        // Convertir el array de bytes a una cadena hexadecimal
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}