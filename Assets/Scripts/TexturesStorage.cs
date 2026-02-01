using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public static class TexturesStorage
{
    public static Dictionary<string, Texture2D> Cache = new();

    public static async UniTask<Texture2D> Load(string url)
    {
        if (Cache.TryGetValue(url, out var tex))
            return tex;

            using var req = UnityWebRequestTexture.GetTexture(url);
        await req.SendWebRequest();

            tex = DownloadHandlerTexture.GetContent(req);
            Cache[url] = tex;

            return tex;
        }
}
