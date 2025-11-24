using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Xna.Framework;
using RetroLibrary.Core;
using RetroLibrary.Core.Attributes;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Resources;
using RetroLibrary.XmlLoader.Extensions;

namespace RetroLibrary.XmlLoader.Components;

public class ComponentLoaderBase(
    IResourceManager resourceManager,
    IColorLoader colorLoader,
    IVariableReplacer variableReplacer)
{
    protected IColorLoader ColorLoader => colorLoader;

    protected IVariableReplacer VariableReplacer => variableReplacer;

    protected static void ApplyBindings(
        object boundObject,
        RetroGameContext gameContext,
        IBindingParser bindingParser)
    {
        if (boundObject == null)
        {
            return;
        }

        var type = boundObject.GetType();

        var propertyTargets = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<RetroSpriteBindablePropertyAttribute>() != null)
            .ToList();

        var fieldTargets = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(f => f.GetCustomAttribute<RetroSpriteBindablePropertyAttribute>() != null)
            .Select(f => char.ToUpperInvariant(f.Name[0]) + f.Name[1..])
            .Select(propName => type.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance))
            .Where(p => p != null)
            .ToList();

        foreach (var prop in propertyTargets.Concat(fieldTargets))
        {
            var supported = prop!.PropertyType.IsAssignableTo(typeof(IBindingValue));
            if (!supported)
            {
                continue;
            }

            var currentValue = prop.GetValue(boundObject) as IBindingValue;
            if (!currentValue!.HasBindingString)
            {
                continue;
            }

            var bindingInfo = bindingParser.Parse(boundObject, currentValue.BindingString!);
            bindingInfo.BoundPropertyName ??= prop.Name;
            gameContext.RetroGameLoaderService.ViewModel?.Binder.AddBinding(bindingInfo);
        }
    }

    protected static float ToFloat(
        XAttribute? attribute,
        float defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return float.Parse(attribute.Value, CultureInfo.InvariantCulture);
    }

    protected static bool ToBool(
        XAttribute? attribute,
        bool defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return bool.Parse(attribute.Value);
    }

    protected static RelayCommand? GetRelayCommand(
        XAttribute? attribute,
        RetroGameContext gameContext)
    {
        if (attribute != null && gameContext.RetroGameLoaderService.ViewModel != null)
        {
            var path = attribute.Value;
            var relayCommand = gameContext.RetroGameLoaderService.ViewModel.GetRelayCommandByPath(path);
            return relayCommand;
        }

        return null;
    }

    protected static T ToEnum<T>(
        XAttribute? attribute,
        T defaultValue)
        where T : struct
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return Enum.Parse<T>(attribute.Value, ignoreCase: false);
    }

    protected T? GetResource<T>(
        XAttribute? attribute)
    {
        if (attribute == null)
        {
            return default;
        }

        return resourceManager.GetResource<T>(attribute.Value);
    }

    protected BindingValue<T>? ToBindingValue<T>(
        XAttribute? attribute,
        IBindingParser bindingParser,
        BindingValue<T>? defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        var isBindingString = bindingParser.IsBindingString(attribute.Value);

        return isBindingString ?
            new BindingValue<T>(bindingString: attribute.Value) :
            new BindingValue<T>(ConvertAttributeValue<T>(attribute.Value));
    }

    protected int ToInt(
        XAttribute? attribute,
        RetroGameContext gameContext,
        int defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return attribute.ToInt(gameContext, VariableReplacer);
    }

    protected Color? ToColor(
        XAttribute? attribute,
        XAttribute? alpha,
        Color? defaultColor)
    {
        if (attribute == null)
        {
            return defaultColor;
        }

        var alphaValue = alpha != null ? float.Parse(alpha.Value, CultureInfo.InvariantCulture) : 1.0f;
        return attribute.ToColor(ColorLoader, defaultColor, alphaValue);
    }

    protected Point ToPoint(
        XAttribute? attribute,
        RetroGameContext gameContext,
        Point defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return attribute.ToPoint(gameContext, VariableReplacer);
    }

    protected Rectangle ToRectangle(
        XAttribute? attribute,
        RetroGameContext gameContext,
        Rectangle defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return attribute.ToRectangle(gameContext, VariableReplacer);
    }

    private static T? ConvertAttributeValue<T>(string raw)
    {
        var type = typeof(T);
        object? result = null;
        try
        {
            if (type == typeof(string))
            {
                result = raw;
            }
            else if (type == typeof(int) && int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            {
                result = i;
            }
            else if (type == typeof(float) && float.TryParse(raw, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var f))
            {
                result = f;
            }
            else if (type == typeof(double) && double.TryParse(raw, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var d))
            {
                result = d;
            }
            else if (type == typeof(bool) && bool.TryParse(raw, out var b))
            {
                result = b;
            }
            else if (type.IsEnum && Enum.TryParse(type, raw, true, out var e))
            {
                result = e;
            }
            else
            {
                result = Convert.ChangeType(raw, type, CultureInfo.InvariantCulture);
            }
        }
        catch
        {
            result = null;
        }

        return (T?)result;
    }
}