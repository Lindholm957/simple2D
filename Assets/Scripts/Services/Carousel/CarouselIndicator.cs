using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarouselIndicator : MonoBehaviour
{
    [SerializeField] private CanvasGroup indicatorActiveCG;
    [SerializeField] private Button button;

    private Coroutine _alphaChangeCoroutine;

    private UnityAction _onClickAction;

    public void Initialize(UnityAction onClickAction)
    {
        _onClickAction = onClickAction;
        button.onClick.AddListener(_onClickAction);
    }

    public void Deactivate(float duration)
    {
        if (_alphaChangeCoroutine != null)
            StopCoroutine(_alphaChangeCoroutine);

        _alphaChangeCoroutine = StartCoroutine(ChangeAlpha(1, duration));
    }

    public void Activate(float duration)
    {
        if (_alphaChangeCoroutine != null)
            StopCoroutine(_alphaChangeCoroutine);

        _alphaChangeCoroutine = StartCoroutine(ChangeAlpha(0, duration));
    }

    private IEnumerator ChangeAlpha(float targetAlpha, float duration)
    {
        float startAlpha = indicatorActiveCG.alpha;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float lerpValue = time / duration;
            float newAlpha = indicatorActiveCG.alpha;
            newAlpha = Mathf.Lerp(startAlpha, targetAlpha, lerpValue);
            indicatorActiveCG.alpha = newAlpha;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(_onClickAction);
    }
}