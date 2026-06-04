using Oxide.Core;
using Oxide.Core.Extensions;

namespace Oxide.Ext.CarbonAliases
{
    public class CarbonExtension(ExtensionManager manager) : Extension(manager)
    {
        public override string Name => "CarbonAliases";
        public override VersionNumber Version => new(1, 3, 0);
        public override string Author => "ThePitereq";

        internal static CUI carbonCUI;

        public override void Load() => Interface.Oxide.LogWarning($"\n[CarbonAliases] Carbon to Oxide Extension Loaded\n[CarbonAliases] Version: {Version.Major}.{Version.Minor}.{Version.Patch}\n[CarbonAliases] Switching to Carbon will have a positive impact on the performance of at least some of your plugins!\n[CarbonAliases] Consider this!\n[CarbonAliases] https://carbonmod.gg/\n");

        public override void LoadPluginWatchers(string plugindir) { }

        public override void OnModLoad() => Manager.RegisterLibrary(nameof(carbonCUI), carbonCUI);

        public override void OnShutdown() { }
    }
}