using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

[RequireComponent(typeof(Toggle))]
public class Category : MonoBehaviour
{
    [SerializeField] private CategoryType categoryType;
    [SerializeField] private TMP_Text label;
    [SerializeField] private Image underlineImage;
    [SerializeField] private Color activeTextColor;
    [SerializeField] private float animationDuration = 0.25f;

    public CategoryType Type => categoryType;
    public event Action<CategoryType> Selected;

    private Toggle _toggle;
    private Color _inactiveTextColor = Color.black;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();

        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
            Selected?.Invoke(categoryType);

        Animate(isOn).Forget();
    }

    private async UniTask Animate(bool isOn)
    {
        float time = 0f;

        Color startColor = label.color;
        Color targetColor = isOn ? activeTextColor : _inactiveTextColor;

        float startAlpha = underlineImage.color.a;
        float targetAlpha = isOn ? 1f : 0f;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;

            label.color = Color.Lerp(startColor, targetColor, t);
            SetUnderlineAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));

            await UniTask.Yield();
        }

        label.color = targetColor;
        SetUnderlineAlpha(targetAlpha);
    }

    private void SetUnderlineAlpha(float alpha)
    {
        var c = underlineImage.color;
        c.a = alpha;
        underlineImage.color = c;
    }
}
