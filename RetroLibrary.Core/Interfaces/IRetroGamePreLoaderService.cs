using RetroLibrary.Core.Configuration;

namespace RetroLibrary.Core.Interfaces;

public interface IRetroGamePreLoaderService
{
    public RetroGameConfiguration PreLoad(RetroGameContext retroGameContext);
}