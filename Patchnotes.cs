namespace DevCli;

class Patchnotes
{
    public string? argRepo { get; set; }

    public Patchnotes(string[] args)
    {
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
        
        var Repository = Path.GetFullPath(argRepo, Directory.GetCurrentDirectory());
        if (!Directory.Exists(Repository))
        {
            ConsoleError($"Repository path does not exist: {Repository}");
            return;
        }
        
        Console.WriteLine($"Create Patchnotes for repository: {Repository}");
        CreatePatchnotes();
    }

    /// <summary>
    /// Creates patchnotes based on the provided repository and other arguments.
    /// </summary>
    private void CreatePatchnotes()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Patchnotes creation logic goes here.");
        Console.ResetColor();
    }
}