namespace RetroLibrary.Loader.Common;

public interface IVariableReplacer
{
    public Dictionary<string, string> ReplaceVariables(
            RetroGameContext gameContext,
            List<string> variables);
}
