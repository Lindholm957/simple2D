using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Carousel : MonoBehaviour, IEndDragHandler
{
    [Header("Parts Setup")]
    [SerializeField] private List<ElementData> elementsData = new List<ElementData>();
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform contentBoxHorizontal;
    [SerializeField] private CarouselElement carouselElementPrefab;
    private List<CarouselElement> _elements = new List<CarouselElement>();

    [Header("Indicators")]
    [SerializeField] private Transform indicatorParent;
    [SerializeField] private CarouselIndicator indicatorPrefab;
    private List<CarouselIndicator> _indicators = new List<CarouselIndicator>();

    [Header("Animation Setup")]
    [SerializeField, Range(0.25f, 1f)] private float duration = 0.5f;
    [SerializeField] private AnimationCurve easeCurve;

    [Header("Auto Scroll Setup")]
    [SerializeField] private bool autoScroll = false;
    [SerializeField] private float autoScrollInterval = 5f;
    private float _autoScrollTimer;

    private int _currentIndex = 0;
    private Coroutine _scrollCoroutine;

    private void Reset()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
    }

    private void Start()
    {
        foreach (var elementData in elementsData)
        {
            var carouselEntry = Instantiate(carouselElementPrefab, contentBoxHorizontal);
            carouselEntry.Initialize(elementData.Preview, elementData.URL);
            _elements.Add(carouselEntry);

            CarouselIndicator indicator = Instantiate(indicatorPrefab, indicatorParent);
            indicator.Initialize(() =>
            {
                ClearCurrentIndex();
                ScrollTo(_indicators.IndexOf(indicator));
            });
            _indicators.Add(indicator);           
        }

        if (_indicators.Count > 0)
            _indicators[0].Activate(0.1f);

        _autoScrollTimer = autoScrollInterval;
    }

    private void ClearCurrentIndex()
    {
        if (_indicators.Count > 0)
            _indicators[_currentIndex].Deactivate(duration);
    }

    private void ScrollTo(int index)
    {
        _currentIndex = index;
        _autoScrollTimer = autoScrollInterval;

        float targetHorizontalPosition = (float)_currentIndex / (_elements.Count - 1);

        if (_scrollCoroutine != null)
            StopCoroutine(_scrollCoroutine);

        _scrollCoroutine = StartCoroutine(LerpToPos(targetHorizontalPosition));

        if (_indicators.Count > 0)
            _indicators[_currentIndex].Activate(duration);
    }

    public void ScrollToNext()
    {
        ClearCurrentIndex();
        _currentIndex = (_currentIndex + 1) % _elements.Count;
        ScrollTo(_currentIndex);
    }

    public void ScrollToPrevious()
    {
        ClearCurrentIndex();
        _currentIndex = (_currentIndex - 1 + _elements.Count) % _elements.Count;
        ScrollTo(_currentIndex);
    }

    private IEnumerator LerpToPos(float targetHorizontalPosition)
    {
        float elapsedTime = 0f;
        float initialPos = scrollRect.horizontalNormalizedPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float eased = easeCurve.Evaluate(elapsedTime / duration);
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(initialPos, targetHorizontalPosition, eased);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetHorizontalPosition;
    }

    private void Update()
    {
        if (!autoScroll)
            return;

        _autoScrollTimer -= Time.deltaTime;
        if (_autoScrollTimer <= 0)
        {
            ScrollToNext();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.delta.x > 0)
        {
            ScrollToPrevious();
        }
        else if (eventData.delta.x < 0)
        {
            ScrollToNext();
        }
        else
        {
            ScrollTo(_currentIndex);
        }
    }
}
