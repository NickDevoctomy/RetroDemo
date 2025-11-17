using System.Text.RegularExpressions;

namespace RetroLibrary.Core.Common;

public class VariableReplacer : IVariableReplacer
{
    public string ReplaceAllVariables(
        RetroGameContext gameContext,
        string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var matches = Regex.Matches(input, "\\$\\{(?<name>[A-Za-z0-9_]+)\\}");
        if (matches.Count == 0)
        {
            return input;
        }

        var variableNames = matches
            .Select(m => m.Groups["name"].Value)
            .Distinct()
            .ToList();

        var replacements = ReplaceVariables(gameContext, variableNames);

        var missing = variableNames.Where(v => !replacements.ContainsKey(v)).ToList();
        if (missing.Count > 0)
        {
            throw new KeyNotFoundException($"No replacements found for variables: {string.Join(", ", missing)}");
        }

        var result = input;
        foreach (var kvp in replacements)
        {
            result = result.Replace($"${{{kvp.Key}}}", kvp.Value, StringComparison.Ordinal);
        }

        return result;
    }

    public Dictionary<string, string> ReplaceVariables(
        RetroGameContext gameContext,
        List<string> variables)
    {
        var results = new Dictionary<string, string>();
        foreach (var variable in variables)
        {
            switch (variable)
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
