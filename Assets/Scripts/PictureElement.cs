using System;
using UnityEngine;
using UnityEngine.UI;

public class PictureElement : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private GameObject premiumTag;
    [SerializeField] private GameObject loaderImage;

    private RectTransform _rect;
    private RectTransform _viewport;

    private string _url;
    private bool _isLoaded;
    private bool _isLoading;
    private bool _isInitialized;

    public bool IsPremium => premiumTag.activeSelf;
    public Sprite Sprite => image.sprite;
    public bool IsLoaded => _isLoaded;

    public event Action<PictureElement> Clicked;

    public void Initialize(string url, RectTransform viewport)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => Clicked?.Invoke(this));

        _url = url;
        _viewport = viewport;
        _rect = transform as RectTransform;

        loaderImage.SetActive(true);
        _isInitialized = true;
    }

    private void Update()
    {
        if (_isLoaded || _isLoading || !_isInitialized)
            return;

        if (IsVisible())
            LoadImage();
    }

    private bool IsVisible()
    {
        Vector3[] itemCorners = new Vector3[4];
        Vector3[] viewportCorners = new Vector3[4];

        _rect.GetWorldCorners(itemCorners);
        _viewport.GetWorldCorners(viewportCorners);

        Rect itemRect = new Rect(itemCorners[0], itemCorners[2] - itemCorners[0]);
        Rect viewportRect = new Rect(viewportCorners[0], viewportCorners[2] - viewportCorners[0]);

        return itemRect.Overlaps(viewportRect);
    }

    private async void LoadImage()
    {
        if (TexturesStorage.Cache.ContainsKey(_url))
        {
            ApplyTexture(TexturesStorage.Cache[_url]);
            return;
        }

        _isLoading = true;
        Texture2D tex = await TexturesStorage.Load(_url);
        ApplyTexture(tex);
    }

    private void ApplyTexture(Texture2D tex)
    {
        _isLoaded = true;
        _isLoading = false;

        image.sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );

        loaderImage.SetActive(false);
    }

    public void SetPremiumTagState(bool isActive)
    {
        premiumTag.SetActive(isActive);
    }
}
