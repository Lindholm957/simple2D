using UnityEngine;
using UnityEngine.UI;

public class PreviewPopup : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(Close);
    }

    public void Show(Sprite sprite)
    {
        image.sprite = sprite;
    }

    private void Close()
    {
        Destroy(gameObject);
    }
}
