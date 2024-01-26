using System.Collections.Generic;

public abstract class CommandBase
{
    public abstract string Name { get; }
    public abstract List<string> Execute(string[] args);
}