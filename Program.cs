namespace DevCli;

using System;
using DevCli.Service;

class Program
{
    public static string[]? Args { get; set; }

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        if(args.Length > 0)
        {
            Args = args;
            showArgs(Args);
        }
    }

    static void showArgs(string[] args)
    {
        Console.WriteLine("Arguments:");
        int i = 0;
        foreach(var arg in args)
        {
            Console.WriteLine($"{i}: {arg}");
            i++;
        }
        switch(args[0])
        {
            case "create":
                var createService = new CreateService();
                createService.Name = args.Length > 1 ? args[1] : "";
                createService.Create();
                break;
            default:
                Console.WriteLine($"Unknown command: {args[0]}");
                break;
        }
    }
}