using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Text.RegularExpressions;
using System;

public class LinksStorage
{
    private const string BaseUrl = "https://data.ikppbb.com/test-task-unity-data/pics/";

    public List<string> Urls { get; private set; } = new();

    public LinksStorage()
    {
        Initialize().Forget();
    }

    private async UniTask Initialize()
    {
        using var req = UnityWebRequest.Get(BaseUrl);
        await req.SendWebRequest();

        string html = req.downloadHandler.text;

        var matches = Regex.Matches(html, @"href=""([^""]+\.(jpg|png))""");

        Uri baseUri = new Uri(BaseUrl);

        foreach (Match match in matches)
        {
            string file = match.Groups[1].Value;

            Uri fullUri = new Uri(baseUri, file);

            Urls.Add(fullUri.ToString());
        }

        Debug.Log($"Found {Urls.Count} images");
    }
}
