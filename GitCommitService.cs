using System.Diagnostics;
using DevCli.Service;

namespace DevCli;

class GitCommitService
{
    public async Task CommitAsync(string[] args)
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

        if (args.Contains("-a", StringComparer.Ordinal))
        {
            var addResult = await RunGitAsync(repository, "add", ".", "--", ":(exclude)patchnotes.md");
            if (addResult.ExitCode != 0)
            {
                ConsoleError(addResult.StandardError);
                return;
            }
        }

        var patchnotesResult = await RunGitAsync(repository, "diff", "--cached", "--name-only", "--", "patchnotes.md");
        if (patchnotesResult.ExitCode != 0)
        {
            ConsoleError(patchnotesResult.StandardError);
            return;
        }
        if (!string.IsNullOrWhiteSpace(patchnotesResult.StandardOutput))
        {
            ConsoleError("patchnotes.md is excluded from smart commits. Unstage it before committing.");
            return;
        }

        var diffResult = await RunGitAsync(repository, "diff", "--cached", "--no-ext-diff", "--");
        if (diffResult.ExitCode != 0)
        {
            ConsoleError(diffResult.StandardError);
            return;
        }
        if (string.IsNullOrWhiteSpace(diffResult.StandardOutput))
        {
            ConsoleWarning("No staged changes to commit.");
            return;
        }

        const string prompt = "Write one short, clear commit message in English describing these staged changes. " +
            "Use imperative mood. Return only the message, without quotes or markdown.";
        string message = await new OllamaService().GenerateAsync(prompt, diffResult.StandardOutput);
        message = message.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault()?.Trim().Trim('"', '\'', '`') ?? "";

        if (string.IsNullOrWhiteSpace(message) ||
            message.StartsWith("Ollama Error:", StringComparison.OrdinalIgnoreCase) ||
            message.Equals("Ollama returned no response.", StringComparison.OrdinalIgnoreCase))
        {
            ConsoleError($"Could not generate a commit message: {message}");
            return;
        }

        Console.WriteLine($"Commit message: {message}");
        var commitResult = await RunGitAsync(repository, "commit", "-m", message);
        if (commitResult.ExitCode != 0)
        {
            ConsoleError(commitResult.StandardError);
            return;
        }

        Console.Write(commitResult.StandardOutput);
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
