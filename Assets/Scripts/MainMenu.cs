using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private PictureElement pictureElementPrefab;

    private ServiceLocator _locator => ServiceLocator.Instance;
    private LinksStorage _linksStorage => _locator.Get<LinksStorage>();

    private List<PictureElement> _pictureElements = new();
    private List<PictureElement> _elementsBuffer = new();

    private void Awake()
    {
        grid.constraintCount = DeviceInformation.IsTablet() ? 3 : 2;

        for (int i = 0; i < scrollRect.content.childCount; i++)
            _elementsBuffer.Add(scrollRect.content.GetChild(i).GetComponent<PictureElement>());
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => _linksStorage.Urls.Count != 0);

        for (int i = 0; i < _elementsBuffer.Count; i++)
            InitializeElement(_elementsBuffer[i], i);

        int existingElementsCount = _elementsBuffer.Count;
        for (int i = existingElementsCount; i < _linksStorage.Urls.Count; i++)
            InstantiateElement(i);

        _elementsBuffer.Clear();
    }

    private void InstantiateElement(int index)
    {
        var pictureElement = Instantiate(pictureElementPrefab, scrollRect.content);
        InitializeElement(pictureElement, index);
    }

    private void InitializeElement(PictureElement element, int index)
    {
        element.Initialize(
            () => Debug.Log($"Clicked on picture with URL: {_linksStorage.Urls[index]}"),
            _linksStorage.Urls[index],
            scrollRect.viewport
        );
        element.SetPremiumTagState((index + 1) % 4 == 0);
        _pictureElements.Add(element);
    }
}
