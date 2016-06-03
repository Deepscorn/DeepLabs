// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Sources.Util.Extensions;
using Assets.Sources.Util.Pattern;
using UnityEngine;

namespace Assets.Sources.Util.UI
{
    public class SpriteDownloader: InGameSingleton<SpriteDownloader>
    {
        private readonly Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();

        // Note:
        // 1. You StartCoroutine in your object, so if it is destroyed,
        // image is not downloaded and callback is not called. 
        // Which is good - we have no one to notify about download finish
        // 2. It's up to you to apply DestroyObject() or DontDestroyOnLoad() to downloaded sprite if you need it
        public IEnumerator DownloadSprite(string url, Action<Sprite> callback)
        {
            Sprite result = null;
            if (!string.IsNullOrEmpty(url) && (!cache.TryGetValue(url, out result) || result == null))
            {
                var www = new WWW(url);
                yield return www;

                if (www.error != null || !www.isDone)
                {
                    yield break;
                }

                var texture = www.texture;
                if (texture != null)
                {
                    result = texture.ToSprite();
                    cache[url] = result;
                }
            }
            callback.NotNullCall(result);
        }
    }
}

