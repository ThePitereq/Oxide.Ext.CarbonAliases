using Oxide.Core;
using Oxide.Core.Extensions;

namespace Oxide.Ext.CarbonAliases
{
    public class CarbonExtension : Extension
    {
        public CarbonExtension(ExtensionManager manager) : base(manager) { }
        public override string Name => "CarbonAliases";
        public override VersionNumber Version => new VersionNumber(1, 0, 2);
        public override string Author => "ThePitereq";

        internal static CUI carbonCUI;

        public override void Load() => Interface.Oxide.LogWarning("\nCarbon to Oxide Extension Loaded!\nPlease consider switching to Carbon.\nThis will have a positive impact on the performance of at least some of your plugins!\nhttps://carbonmod.gg/\n");

        public override void LoadPluginWatchers(string plugindir) { }

        public override void OnModLoad() => Manager.RegisterLibrary(nameof(carbonCUI), carbonCUI);

        public override void OnShutdown() { }
    }
}