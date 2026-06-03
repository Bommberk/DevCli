namespace DevCli.Service;

// using DevCli;

class CreateService
{
    public string? Name { get; set; }

    public async Task CreateAsync()
    {
        switch(Name)
        {
            case "patchnotes":
                if(Program.Args == null)
                {
                    ConsoleError("No arguments provided for patchnotes creation.");
                    return;
                }
                var patchnotes = new Patchnotes(Program.Args);
                await patchnotes.CreateAsync();
                break;
            default:
                ConsoleWarning($"Unknown create target: {Name}");
                break;
        }
    }
}