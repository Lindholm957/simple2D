using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public static class TexturesStorage
{
    private static Dictionary<string, Texture2D> _cache = new();

    public static async UniTask<Texture2D> Load(string url)
    {
        if (_cache.TryGetValue(url, out var tex))
            return tex;

        using var req = UnityWebRequestTexture.GetTexture(url);
        await req.SendWebRequest();

        tex = DownloadHandlerTexture.GetContent(req);
        _cache[url] = tex;

        return tex;
    }
}
