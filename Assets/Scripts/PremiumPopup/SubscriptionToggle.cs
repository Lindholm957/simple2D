using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SubscriptionToggle : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text extraText;
    [SerializeField] private Image icon;

    [Header("Colors")]
    [SerializeField] private Color mainActiveColor;
    [SerializeField] private Color mainInactiveColor;
    [SerializeField] private Color extraActiveColor;
    [SerializeField] private Color extraInactiveColor;

    [Header("Sprites")]
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnToggleChanged);

        ApplyState(_toggle.isOn);
    }

    private void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        ApplyState(isOn);
    }

    private void ApplyState(bool isOn)
    {
        mainText.color = isOn ? mainActiveColor : mainInactiveColor;

        if (icon != null)
            icon.sprite = isOn ? activeSprite : inactiveSprite;

        if (extraText != null)
            extraText.color = isOn ? extraActiveColor : extraInactiveColor;
    }
}
