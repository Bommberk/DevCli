using System.Diagnostics;

namespace DevCli;

class GitPushService
{
    public async Task PushAsync(string[] args)
    {
        string? repository = GetRepositoryPath(args);
        if (repository == null)
        {
            return;
        }

        var gitStatus = await RunGitAsync(repository, "rev-parse", "--is-inside-work-tree");
        if (gitStatus.ExitCode != 0)
        {
            ConsoleError($"Not a git repository: {repository}");
            return;
        }

        if (args.Contains("--stage-all", StringComparer.Ordinal))
        {
            var addResult = await RunGitAsync(repository, "add", ".");
            if (addResult.ExitCode != 0)
            {
                ConsoleError(addResult.StandardError);
                return;
            }
        }

        string[] commitArgs = args.Where(arg => arg != "--stage-all").ToArray();
        bool committed = await new GitCommitService().CommitAsync(commitArgs);
        if (!committed)
        {
            return;
        }

        var pushResult = await RunGitAsync(repository, "push");
        if (pushResult.ExitCode != 0)
        {
            ConsoleError(pushResult.StandardError);
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Push successful.");
        Console.ResetColor();
    }

    private static string? GetRepositoryPath(string[] args)
    {
        string repository = Directory.GetCurrentDirectory();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] is "--repository" or "--repo")
            {
                if (i + 1 >= args.Length)
                {
                    ConsoleError("A repository path must follow --repo.");
                    return null;
                }

                repository = Path.GetFullPath(args[++i], Directory.GetCurrentDirectory());
            }
        }

        if (!Directory.Exists(repository))
        {
            ConsoleError($"Repository path does not exist: {repository}");
            return null;
        }

        return repository;
    }

    private static async Task<(int ExitCode, string StandardOutput, string StandardError)> RunGitAsync(
        string repository,
        params string[] arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "git",
            WorkingDirectory = repository,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (string argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)!;
        Task<string> standardOutput = process.StandardOutput.ReadToEndAsync();
        Task<string> standardError = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return (process.ExitCode, await standardOutput, await standardError);
    }
}
