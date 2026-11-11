namespace DevCli;

using System;
using DevCli.Service;

class Program
{
    public static string[]? Args { get; set; }

    static async Task Main(string[] args)
    {
        if(args.Length > 0)
        {
            Args = args;
            await showArgs(Args);
        }
        else
        {
            Console.WriteLine("DevCli - A CLI tool for developers");
        }
    }

    static async Task showArgs(string[] args)
    {
        switch(args[0])
        {
            case "create":
                var createService = new CreateService();
                createService.Name = args.Length > 1 ? args[1] : "";
                await createService.CreateAsync();
                break;
            case "commit":
                var gitCommitService = new GitCommitService();
                await gitCommitService.CommitAsync(args.Skip(1).ToArray());
                break;
            case "push":
                var gitPushService = new GitCommitService();
                await gitPushService.CommitAsync(args.Skip(1).Append("--push").ToArray());
                break;
            default:
                Console.WriteLine($"Unknown command: {args[0]}");
                break;
        }
    }
}