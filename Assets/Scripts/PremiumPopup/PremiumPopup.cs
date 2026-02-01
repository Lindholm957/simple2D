using UnityEngine;
using UnityEngine.UI;

public class PremiumPopup : MonoBehaviour
{
    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(Close);
    }

    private void Close()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(Close);
    }
}
