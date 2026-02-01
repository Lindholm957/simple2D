using UnityEngine;
using UnityEngine.UI;

public class PreviewPopup : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(Hide);
        gameObject.SetActive(false);
    }

    public void Show(Sprite sprite)
    {
        image.sprite = sprite;
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
