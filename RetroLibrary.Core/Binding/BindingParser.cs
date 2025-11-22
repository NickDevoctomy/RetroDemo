namespace RetroLibrary.Core.Binding;

public class BindingParser : IBindingParser
{
    public bool IsBindingString(string? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        var work = value.Trim();
        if (work.StartsWith('{') && work.EndsWith('}'))
        {
            work = work[1..^1].Trim();
        }
        else
        {
            return false;
        }

        const string keyword = "binding";
        if (work.StartsWith(keyword, StringComparison.OrdinalIgnoreCase))
        {
            work = work[keyword.Length..].Trim();
        }
        else
        {
            return false;
        }

        return true;
    }

    public BindingInfo Parse(
        object boundObject,
        string bindingString)
    {
        ArgumentException.ThrowIfNullOrEmpty(bindingString);

        var work = bindingString.Trim();
        if (work.StartsWith('{') && work.EndsWith('}'))
        {
            work = work[1..^1].Trim();
        }

        const string keyword = "binding";
        if (work.StartsWith(keyword, StringComparison.OrdinalIgnoreCase))
        {
            work = work[keyword.Length..].Trim();
        }
        else
        {
            throw new FormatException("Binding string must start with '{binding'.");
        }

        var result = new BindingInfo
        {
            BoundObject = boundObject
        };
        if (string.IsNullOrWhiteSpace(work))
        {
            throw new FormatException("Binding string is empty.");
        }

        // Expected tokens: path=PropertyName, source=ViewModelPropertyName, target=BoundPropertyName
        var tokens = work.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var rawToken in tokens)
        {
            var token = rawToken.TrimEnd(',');
            if (token.Length == 0)
            {
                continue;
            }

            if (token.Contains('='))
            {
                var parts = token.Split('=', 2, StringSplitOptions.TrimEntries);
                var name = parts[0];
                var value = parts.Length > 1 ? parts[1].TrimEnd(',') : string.Empty;
                value = UnwrapQuotes(value);

                switch (name.ToLowerInvariant())
                {
                    case "path": // backward compatible original usage meaning source path
                    case "source":
                        {
                            result.Path = value; // store source property name
                            break;
                        }
                    case "target":
                        {
                            result.BoundPropertyName = value; // explicit bound property
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            else
            {
                // If single token provided treat as source path
                result.Path ??= UnwrapQuotes(token);
            }
        }

        return result;
    }

    private static string UnwrapQuotes(string value)
    {
        if (value.Length >= 2 &&
            ((value.StartsWith('"') && value.EndsWith('"')) || (value.StartsWith('\'') && value.EndsWith('\''))))
        {
            return value[1..^1];
        }

        return value;
    }
}