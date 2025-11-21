namespace RetroLibrary.Core.Common;

public interface IVariableReplacer
{
    public Dictionary<string, object> DefaultParameters { get; }

    public string ReplaceAllVariables(
        RetroGameContext gameContext,
        string input);
}