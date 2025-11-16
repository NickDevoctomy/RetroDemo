namespace RetroLibrary.Loader.Common;

public interface IVariableReplacer
{
    public string ReplaceAllVariables(
        RetroGameContext gameContext,
        string input);

    public Dictionary<string, string> ReplaceVariables(
            RetroGameContext gameContext,
            List<string> variables);
}
