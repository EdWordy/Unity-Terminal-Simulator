using System.Collections.Generic;

public class HelpCommand : CommandBase
{
    public override string Name => "HELP";

    public override List<string> Execute(string[] args)
    {
        List<string> response = new List<string>
        {
            "HELP: returns the help menu -- Format: HELP",
            "PRINT: print a command -- Format: PRINT <message>"
            // Add more command descriptions here
        };

        return response;
    }
}

public class PrintCommand : CommandBase
{
    public override string Name => "PRINT";

    public override List<string> Execute(string[] args)
    {
        string msg = string.Join(" ", args, 1, args.Length - 1);
        return new List<string> { msg };
    }
}