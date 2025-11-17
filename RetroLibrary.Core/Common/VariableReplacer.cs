using System.Text.RegularExpressions;
using DynamicExpresso;

namespace RetroLibrary.Core.Common;

public class VariableReplacer : IVariableReplacer
{
    public string ReplaceAllVariables(
        RetroGameContext gameContext,
        string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var interpreter = new Interpreter();
        var result = interpreter.Eval(
            input,
            new Parameter("GameWidth", gameContext.GraphicsDeviceManager!.PreferredBackBufferWidth),
            new Parameter("GameHeight", gameContext.GraphicsDeviceManager!.PreferredBackBufferHeight));

        return result == null
            ? throw new Exception("Variable replacement resulted in null value.")
            : ((int)result).ToString();
    }
}
