using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Toggle))]
public class Category : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Image underlineImage;
    [SerializeField] private Color activeTextColor;
    [Space]
    [SerializeField] private float animationDuration = 1f;

    private Color _inactiveTextColor;
    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        _inactiveTextColor = label.color;
    }

    private void OnToggleValueChanged(bool isOn)
    {
        var targetColor = isOn ? activeTextColor : _inactiveTextColor;
        var targetAlpha = isOn ? 1f : 0f;

        AnimateTransition(targetColor, targetAlpha).Forget();
    }

    private async UniTask AnimateTransition(Color newColor, float targetAlpha)
    {
        float time = 0f;

        Color startColor = label.color;
        float startAlpha = underlineImage.color.a;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;

            label.color = Color.Lerp(startColor, newColor, t);
            SetImageAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        label.color = newColor;
        SetImageAlpha(targetAlpha);
    }

    private void SetImageAlpha(float alpha)
    {
        Color c = underlineImage.color;
        c.a = alpha;
        underlineImage.color = c;
    }
}
