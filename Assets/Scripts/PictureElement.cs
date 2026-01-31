using System;
using UnityEngine;
using UnityEngine.UI;

public class PictureElement : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private GameObject premiumTag;
    [SerializeField] private GameObject loaderImage;

    public bool IsPremium => premiumTag.activeSelf;

    private string _url;

    public void Initialize(Action onClickAction, string url)
    {
        button.onClick.AddListener(() => onClickAction?.Invoke());
        _url = url;
        ShowImage();
    }

    public void SetPremiumTagState(bool isActive)
    {
        premiumTag.SetActive(isActive);
    }

    public void ShowImage()
    {
        if (TexturesStorage.Cache.ContainsKey(_url))
            return;
        else
            LoadImage();
    }

    private async void LoadImage()
    {
        Debug.Log("скачиваем");
        Texture2D tex = await TexturesStorage.Load(_url);
        Debug.Log("скачал");
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        loaderImage.SetActive(false);
    }
}
