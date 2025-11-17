namespace RetroLibrary.Core.Common;

public interface IVariableReplacer
{
    public string ReplaceAllVariables(
        RetroGameContext gameContext,
        string input);
}
