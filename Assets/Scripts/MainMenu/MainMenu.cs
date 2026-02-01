using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private PictureElement pictureElementPrefab;

    [Header("Categories")]
    [SerializeField] private List<Category> categories;

    [Header("Popups")]
    [SerializeField] private Transform popupsParent;
    [SerializeField] private PreviewPopup previewPopupPrefab;
    [SerializeField] private PremiumPopup premiumPopupPrefab;


    private readonly List<PictureElement> _pictureElements = new();
    private readonly List<PictureElement> _elementsBuffer = new();

    private CategoryType _currentCategory = CategoryType.All;

    private ServiceLocator _locator => ServiceLocator.Instance;
    private LinksStorage _linksStorage => _locator.Get<LinksStorage>();

    private void Awake()
    {
        grid.constraintCount = DeviceInformation.IsTablet() ? 3 : 2;

        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            var element = scrollRect.content.GetChild(i).GetComponent<PictureElement>();
            _elementsBuffer.Add(element);
            _pictureElements.Add(element);
        }

        foreach (var category in categories)
            category.Selected += OnCategorySelected;
    }

    private void OnDestroy()
    {
        foreach (var category in categories)
            category.Selected -= OnCategorySelected;
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => _linksStorage.Urls.Count > 0);

        for (int i = 0; i < _elementsBuffer.Count && i < _linksStorage.Urls.Count; i++)
            InitializeElement(_elementsBuffer[i], i);

        for (int i = _elementsBuffer.Count; i < _linksStorage.Urls.Count; i++)
            InstantiateElement(i);

        ApplyCategory();
    }

    private void OnCategorySelected(CategoryType category)
    {
        _currentCategory = category;
        ApplyCategory();
    }

    private void InstantiateElement(int index)
    {
        var element = Instantiate(pictureElementPrefab, scrollRect.content);
        InitializeElement(element, index);
        _pictureElements.Add(element);
    }

    private void InitializeElement(PictureElement element, int index)
    {
        element.Initialize(
            _linksStorage.Urls[index],
            scrollRect.viewport
        );

        element.Clicked += OnElementClicked;
        element.SetPremiumTagState((index + 1) % 4 == 0);
    }

    private void OnElementClicked(PictureElement element)
    {
        if (element.IsPremium)
        {
            var premiumPopUp = Instantiate(
                premiumPopupPrefab,
                popupsParent
            );
            return;
        }

        if (!element.IsLoaded)
        {
            Debug.Log("Image not loaded yet");
            return;
        }

        var previewPopUp = Instantiate(
            previewPopupPrefab,
            popupsParent
        );
        previewPopUp.Show(element.Sprite);
    }

    private void ApplyCategory()
    {
        for (int i = 0; i < _pictureElements.Count; i++)
        {
            bool visible = IsVisibleByCategory(i, _currentCategory);
            _pictureElements[i].gameObject.SetActive(visible);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            scrollRect.content as RectTransform
        );

        scrollRect.verticalNormalizedPosition = 1f;
    }

    private bool IsVisibleByCategory(int dataIndex, CategoryType category)
    {
        int uiIndex = dataIndex + 1;

        return category switch
        {
            CategoryType.All => true,
            CategoryType.Even => uiIndex % 2 == 0,
            CategoryType.Odd => uiIndex % 2 != 0,
            _ => true
        };
    }

}
