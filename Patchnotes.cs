namespace DevCli;

using System.Diagnostics;
using DevCli.Service;

class Patchnotes
{
    public string? repository { get; set; }
    private string diff = @"
        Use following information to create the patchnotes (this is data from a git diff):
    ";
    private string? version = null;

    private string fullPrompt;

    public Patchnotes(string[] args)
    {
        // Set arguments
        SetRepoPath(args);
        SetVersion(args);
        // Create diff with git
        CreateDiffWithGit();
        // Update prompt with markdown template
        string promtMarkdown = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "patchnotes-prompt.md"));
        fullPrompt = promtMarkdown + "\n" + diff;
    }

    private void SetRepoPath(string[] args)
    {
        var argRepo = (string?)null;
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "--repository" || args[i] == "--repo")
            {
                argRepo = args[i + 1] ?? ".";
                break;
            }
            else
            {
                argRepo = ".";
            }
        }
        if(argRepo == null)
        {
            ConsoleError("No repository provided for patchnotes creation.");
            return;
        }
        repository = Path.GetFullPath(argRepo, Directory.GetCurrentDirectory());
        if (!Directory.Exists(repository))
        {
            ConsoleError($"Repository path does not exist: {repository}");
            return;
        }
    }
    private void SetVersion(string[] args)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "--version" || args[i] == "-v")
            {
                version = args[i + 1];
                if (!version.StartsWith("v"))
                {
                    version = "v" + version;
                }
                break;
            }
        }
        if(version != null)
        {
            fullPrompt = $"Create patchnotes for version {version}.\n" + fullPrompt;
        }
    }

    private void CreateDiffWithGit()
    {
        Console.WriteLine("Creating git diff for repository: " + repository);
        if(repository == null)
        {
            ConsoleError("Cannot create git diff without a valid repository.");
            return;
        }

        string gitNewestTag;
        if(version == null)
        {
            // Get the newest tag
            ProcessStartInfo gitNewestTagProcess = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = "describe --tags --abbrev=0",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = repository
            };

            using Process gitNewestTagProcessInstance = Process.Start(gitNewestTagProcess)!;

            gitNewestTag = gitNewestTagProcessInstance.StandardOutput.ReadToEnd().Trim();
            string gitNewestTagError = gitNewestTagProcessInstance.StandardError.ReadToEnd();
            gitNewestTagProcessInstance.WaitForExit();
            if (!string.IsNullOrEmpty(gitNewestTagError))
            {
                die(gitNewestTagError);
            }
            else
            {
                ConsoleLog($"Newest git tag found: {gitNewestTag}");
            }
        }
        else
        {
            gitNewestTag = version;
        }

        // Create diff file
        ProcessStartInfo gitDiffProcess = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = $"diff {gitNewestTag}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = repository
        };

        using Process gitDiffProcessInstance = Process.Start(gitDiffProcess)!;
        string gitDiffOutput = gitDiffProcessInstance.StandardOutput.ReadToEnd();
        string gitDiffError = gitDiffProcessInstance.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(gitDiffError))
        {
            die(gitDiffError);
        }
        gitDiffProcessInstance.WaitForExit();

        File.WriteAllText(
            Path.Combine(repository, "diff.txt"),
            gitDiffOutput
        );

        diff = diff + "\n" + File.ReadAllText(Path.Combine(repository, "diff.txt"));
    }

    public async Task CreateAsync()
    {
        if (repository == null)
        {
            ConsoleError("Cannot create patchnotes without a valid repository.");
            return;
        }

        await CreatePatchnotes();
    }

    /// <summary>
    /// Creates patchnotes based on the provided repository and other arguments.
    /// </summary>
    private async Task CreatePatchnotes()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Creating patchnotes for repository");
        Console.ResetColor();

        if (string.IsNullOrWhiteSpace(diff))
        {
            ConsoleError("Cannot create patchnotes because the git diff is empty.");
            return;
        }

        var ollamaService = new OllamaService();
        string antwort = await ollamaService.GenerateAsync(fullPrompt);
        // Console.WriteLine($"Ollama response: {antwort}");
        File.WriteAllText(
            Path.Combine(repository!, "patchnotes.md"),
            antwort
        );
    }
}