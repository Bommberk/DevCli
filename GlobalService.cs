namespace DevCli.Service;

class GlobalService
{
    public static void ConsoleError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {msg}");
        Console.ResetColor();
    }

    public static void ConsoleWarning(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Warning: {msg}");
        Console.ResetColor();
    }
}