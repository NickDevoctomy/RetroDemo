using System.Xml.Linq;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Xna.Framework;
using RetroLibrary.Core;
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

    protected static float ToFloat(
        XAttribute? attribute,
        float defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return float.Parse(attribute.Value);
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

        return Enum.Parse<T>(attribute.Value, false);
    }

    protected T? GetResource<T>(XAttribute? attribute)
    {
        if (attribute == null)
        {
            return default;
        }

        return resourceManager.GetResource<T>(attribute.Value);
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

        var alphaValue = alpha != null ? float.Parse(alpha.Value) : 1.0f;
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
}