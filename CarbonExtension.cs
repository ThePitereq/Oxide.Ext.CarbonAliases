using Oxide.Core;
using Oxide.Core.Extensions;

namespace Oxide.Ext.CarbonAliases
{
    public class CarbonExtension : Extension
    {
        public CarbonExtension(ExtensionManager manager) : base(manager) { }
        public override string Name => "CarbonAliases";
        public override VersionNumber Version => new VersionNumber(1, 1, 3);
        public override string Author => "ThePitereq";

        internal static CUI carbonCUI;

        public override void Load() => Interface.Oxide.LogWarning($"\nCarbon to Oxide Extension Loaded!\nVersion: {Version.Major}.{Version.Minor}.{Version.Patch}\nSwitching to Carbon will have a positive impact on the performance of at least some of your plugins!\nConsider this!\nhttps://carbonmod.gg/\n");

        public override void LoadPluginWatchers(string plugindir) { }

        public override void OnModLoad() => Manager.RegisterLibrary(nameof(carbonCUI), carbonCUI);

        public override void OnShutdown() { }
    }
}