using Newtonsoft.Json;

namespace Orca.Lib;

public static class Globals
{
    public static JsonSerializerSettings Settings = new JsonSerializerSettings 
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };
}
