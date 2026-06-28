using Oxide.Core.Plugins;
using System;
using System.Collections.Generic;
using Pool = Facepunch.Pool;

namespace Oxide.Ext.CarbonAliases
{
    public class ImageDatabaseModule
    {
        private Plugin ImageLibrary;

        public ImageDatabaseModule(Plugin plugin = null) {
            ImageLibrary = plugin;
        }
        
        public class ImageQueueResult;

        public void QueueBatch(bool @override, IEnumerable<string> urls) => QueueBatch(@override, null, urls);
        
        public void QueueBatch(bool @override, Action<List<ImageQueueResult>> onComplete, IEnumerable<string> urls)
        {
            if (ImageLibrary == null) return;
            Dictionary<string, string> images = Pool.Get<Dictionary<string, string>>();
            images.Clear();
            foreach (string url in urls)
                images.TryAdd(url, url);
            Queue(@override, onComplete, images);
            Pool.FreeUnmanaged(ref images);
        }

        public void Queue(bool @override, Dictionary<string, string> mappedUrls) => Queue(mappedUrls);

        public void Queue(bool @override, Action<List<ImageQueueResult>> onComplete, Dictionary<string, string> mappedUrls) => ImageLibrary?.Call("ImportImageList", "CarbonAliasesRequest", mappedUrls, 0UL, @override, () => onComplete?.Invoke(null));

        public void Queue(Dictionary<string, string> mappedUrls) => ImageLibrary?.Call("ImportImageList", "CarbonAliasesRequest", mappedUrls, 0UL);

        public void Queue(Action<List<ImageQueueResult>> onComplete, Dictionary<string, string> mappedUrls) => Queue(false, onComplete, mappedUrls);

        public void Queue(bool @override, string key, string url) => ImageLibrary?.Call<bool>("AddImage", url, key, 0uL);

        public void Queue(bool @override, string url) => Queue(@override, url, url);

        public void Queue(string key, string url) => Queue(false, key, url);

        public void Queue(string url) => Queue(false, url, url);
        
        public void DeleteImage(string url) => ImageLibrary?.Call("RemoveImage", url);

        public void AddImage(string keyOrUrl, byte[] imageData) => ImageLibrary?.Call<bool>("AddImageData", keyOrUrl, imageData, 0UL);

        public void AddMap(string key, string url) => Queue(false, key, url);
        
        public void RemoveMap(string key, string url) { }
        
        public bool HasImage(string key) => ImageLibrary?.Call<bool>("HasImage", key, 0UL) ?? false;
        
        public uint GetImage(string key, float scale = 0, bool silent = false) => uint.Parse(ImageLibrary?.Call<string>("GetImage", key) ?? "0");
        
        public string GetImageString(string key, float scale = 0, bool silent = false) => ImageLibrary?.Call<string>("GetImage", key) ?? null;

        public void DeleteImage(string url, float scale = 0) { }
    }
}
