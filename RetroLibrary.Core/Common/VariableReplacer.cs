using DynamicExpresso;

namespace RetroLibrary.Core.Common;

public class VariableReplacer : IVariableReplacer
{
    public Dictionary<string, object> DefaultParameters { get; } = [];

    public string ReplaceAllVariables(
        RetroGameContext gameContext,
        string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var parameterList = new List<Parameter>
        {
            new("GameWidth", typeof(int), gameContext.GraphicsDeviceManager!.PreferredBackBufferWidth),
            new("GameHeight", typeof(int), gameContext.GraphicsDeviceManager!.PreferredBackBufferHeight)
        };

        if (DefaultParameters.Count > 0)
        {
            foreach (var parameter in DefaultParameters)
            {
                parameterList.Add(new Parameter(parameter.Key, parameter.Value.GetType(), parameter.Value));
            }
        }

        var interpreter = new Interpreter();
        var result = interpreter.Eval(
            input,
            parameterList.ToArray());

        return result == null
            ? throw new Exception("Variable replacement resulted in null value.")
            : ((int)result).ToString();
    }
}