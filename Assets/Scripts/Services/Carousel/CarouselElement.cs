using System;
using UnityEngine;
using UnityEngine.UI;

public class CarouselElement : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private LayoutElement layoutElement;

    private string _url;

    private void Awake()
    {
        button.onClick.AddListener(GoToNewPage);

        Resize();
    }

    private void Resize()
    {
        float screenWidth = ((RectTransform)transform.parent).rect.width;


        layoutElement.preferredWidth = screenWidth;
    }

    public void Initialize(Sprite previewSprite, string url)
    {
        image.sprite = previewSprite;
        _url = url;
    }

    private void GoToNewPage()
    {
        throw new NotImplementedException();
    }
}
