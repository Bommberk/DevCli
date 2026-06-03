namespace DevCli.Service;

// using DevCli;

class CreateService
{
    public string? Name { get; set; }

    public void Create()
    {
        switch(Name)
        {
            case "patchnotes":
                if(Program.Args == null)
                {
                    ConsoleError("No arguments provided for patchnotes creation.");
                    return;
                }
                Console.WriteLine("Creating patchnotes...");
                new Patchnotes(Program.Args);
                break;
            default:
                ConsoleWarning($"Unknown create target: {Name}");
                break;
        }
    }
}