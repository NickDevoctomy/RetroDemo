
namespace RetroLibrary.Loader.Common;

public class VariableReplacer : IVariableReplacer
{
    public Dictionary<string, string> ReplaceVariables(
        RetroGameContext gameContext,
        List<string> variables)
    {
        var results = new Dictionary<string, string>();
        foreach (var variable in variables)
        {
            switch(variable)
            {
                case "GameWidth":
                    {
                        results.Add(variable, gameContext.GraphicsDeviceManager.PreferredBackBufferWidth.ToString());
                        break;
                    }

                case "GameHeight":
                    {
                        results.Add(variable, gameContext.GraphicsDeviceManager.PreferredBackBufferHeight.ToString());
                        break;
                    }
            }
        }

        return results;
    }
}
