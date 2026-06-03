namespace DevCli;

using DevCli.Service;

class Patchnotes
{
    public string? repository { get; set; }
    public string? diff { get; set; }
    private const string PROMT = @$"
        Create patchnotes for my repository. 
        Use following information to create the patchnotes (this is data from a git diff):
    ";

    public Patchnotes(string[] args)
    {
        // Set repository path from arguments
        SetRepoPath(args);
        // Create diff with git
        CreateDiffWithGit();
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

    private void CreateDiffWithGit()
    {
        Console.WriteLine("Creating git diff...");
        
    }

    public async Task CreateAsync()
    {
        if (repository == null)
        {
            ConsoleError("Cannot create patchnotes without a valid repository.");
            return;
        }

        Console.WriteLine($"Creating patchnotes for repository: {repository}");
        await CreatePatchnotes();
    }

    /// <summary>
    /// Creates patchnotes based on the provided repository and other arguments.
    /// </summary>
    private async Task CreatePatchnotes()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Patchnotes creation logic goes here.");
        Console.ResetColor();

        var ollamaService = new OllamaService();
        string antwort = await ollamaService.GenerateAsync("say 'hello world'");
        Console.WriteLine($"Ollama response: {antwort}");
    }
}