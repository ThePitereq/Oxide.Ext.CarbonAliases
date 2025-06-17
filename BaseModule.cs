using Oxide.Core;

namespace Oxide.Ext.CarbonAliases
{
    public class BaseModule
    {
        public static T GetModule<T>()
        {
            object obj;
            switch (typeof(T).Name)
            {
                case nameof(ImageDatabaseModule):
                    var plugin = Interface.GetMod().RootPluginManager.GetPlugin("ImageLibrary");
                    if (plugin == null)
                    {
                        Interface.Oxide.LogWarning("ImageLibrary plugin not found! UI building will print errors!");
                        return default;
                    }

                    obj = new ImageDatabaseModule(plugin);
                    break;

                default:
                    Interface.Oxide.LogWarning($"Module {nameof(T)} not supported! This may cause issues!");
                    return default;
            }

            return obj is T module ? module : default;
        }
    }
}
