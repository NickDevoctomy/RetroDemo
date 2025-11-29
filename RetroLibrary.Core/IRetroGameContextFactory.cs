namespace RetroLibrary.Core;

public interface IRetroGameContextFactory
{
    public RetroGameContext CreateRetroGameContext(
        string[] args,
        int width,
        int height,
        bool isFullScreen,
        string gameDefinitionFilePath);
}