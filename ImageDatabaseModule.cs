using Oxide.Core;
using Oxide.Core.Plugins;
using System;
using System.Collections.Generic;
using Pool = Facepunch.Pool;

namespace Oxide.Ext.CarbonAliases
{
    public class ImageDatabaseModule
    {
        private static Plugin ImageLibrary;

        public ImageDatabaseModule() {
            ImageLibrary = Interface.GetMod().RootPluginManager.GetPlugin("ImageLibrary");
            if (ImageLibrary == null)
                Interface.Oxide.LogWarning("ImageLibrary not found! UI building will print errors!");
        }

        public void QueueBatch(bool @override, IEnumerable<string> urls) => QueueBatch(@override, null, urls);
        
        public void QueueBatch(bool @override, Action<List<ImageQueueResult>> onComplete, IEnumerable<string> urls)
        {
            Dictionary<string, string> images = Pool.Get<Dictionary<string, string>>();
            images.Clear();
            foreach (string url in urls)
            {
                if (images.ContainsKey(url)) continue;
                images.Add(url, url);
            }
            ImageLibrary.Call("ImportImageList", "CarbonAliasesRequest", images, 0UL, @override, () => onComplete(null));
            Pool.FreeUnmanaged(ref images);
        }

        public class ImageQueueResult
        {
            
        }

        public void Queue(Dictionary<string, string> urls)
        {
            foreach (var kv in urls)
                ImageLibrary.Call<bool>("AddImage", kv.Value, kv.Key, 0uL);
        }
        public void DeleteImage(string url)
        {
            ImageLibrary.Call("RemoveImage", url);
        }
        
        public void AddMap(string key, string url)
        {
            ImageLibrary.Call<bool>("AddImage", url, key, 0uL);
        }
        public void RemoveMap(string key, string url)
        {

        }
        
        public bool HasImage(string key)
        {
            return ImageLibrary.Call<bool>("HasImage", key, 0UL);
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
