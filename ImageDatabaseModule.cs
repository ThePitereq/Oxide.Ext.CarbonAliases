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

        public ImageDatabaseModule(Plugin plugin = null) {
            ImageLibrary = plugin;
        }

        public void QueueBatch(bool @override, IEnumerable<string> urls) => QueueBatch(@override, null, urls);
        
        public void QueueBatch(bool @override, Action<List<ImageQueueResult>> onComplete, IEnumerable<string> urls)
        {
            if (ImageLibrary == null) return;

            Dictionary<string, string> images = Pool.Get<Dictionary<string, string>>();
            images.Clear();
            foreach (string url in urls)
            {
                if (images.ContainsKey(url)) continue;
                images.Add(url, url);
            }
            ImageLibrary.Call("ImportImageList", "CarbonAliasesRequest", images, 0UL, @override, () => onComplete?.Invoke(null));
            Pool.FreeUnmanaged(ref images);
        }

        public class ImageQueueResult
        {
            
        }

        public void Queue(Dictionary<string, string> urls)
        {
            if (ImageLibrary == null) return;

            foreach (var kv in urls)
                ImageLibrary.Call<bool>("AddImage", kv.Value, kv.Key, 0uL);
        }
        public void DeleteImage(string url)
        {
            ImageLibrary?.Call("RemoveImage", url);
        }
        
        public void AddMap(string key, string url)
        {
            ImageLibrary?.Call<bool>("AddImage", url, key, 0uL);
        }
        public void RemoveMap(string key, string url)
        {

        }
        
        public bool HasImage(string key)
        {
            if (ImageLibrary == null) return false;

            return ImageLibrary.Call<bool>("HasImage", key, 0UL);
        }
        
        public uint GetImage(string key, float scale = 0, bool silent = false)
        {
            if (ImageLibrary == null) return 0;
            
            return Convert.ToUInt32(ImageLibrary.Call<string>("GetImage", key));
        }
        public string GetImageString(string key, float scale = 0, bool silent = false)
        {
            if (ImageLibrary == null) return null;

            return ImageLibrary.Call<string>("GetImage", key);
        }

        public void DeleteImage(string url, float scale = 0)
        {

        }
    }
}
