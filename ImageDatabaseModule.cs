using Oxide.Core;
using Oxide.Core.Plugins;
using System;

namespace Oxide.Ext.CarbonAliases
{
    public class ImageDatabaseModule
    {
        private static Plugin ImageLibrary;

        public ImageDatabaseModule() {
            ImageLibrary = Interface.GetMod().RootPluginManager.GetPlugin("ImageLibrary");
            if (ImageLibrary == null)
                Interface.Oxide.LogWarning("ImageLibrary not found! Images will print errors!");
            else
                Interface.Oxide.LogInfo("ImageLibrary is being used in Carbon to Oxide Conversion...");
        }

        public void QueueBatch(bool @override, params string[] urls)
        {
            foreach (var url in urls)
                ImageLibrary.Call<bool>("AddImage", url, url, 0uL);
        }
        public void QueueBatch(float scale, bool @override, params string[] urls)
        {
            foreach (var url in urls)
                ImageLibrary.Call<bool>("AddImage", url, url, 0uL);
        }
        public void AddMap(string key, string url)
        {
            ImageLibrary.Call<bool>("AddImage", url, key, 0uL);
        }
        public void RemoveMap(string key, string url)
        {

        }
        public uint GetImage(string key, float scale = 0, bool silent = false)
        {
            return Convert.ToUInt32(ImageLibrary.Call<string>("GetImage", key));
        }
        public string GetImageString(string key, float scale = 0, bool silent = false)
        {
            return ImageLibrary.Call<string>("GetImage", key);
        }
        public void DeleteImage(string url, float scale = 0)
        {

        }
    }
}
