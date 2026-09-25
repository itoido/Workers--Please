using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchResultItem : MonoBehaviour
{
    [SerializeField]
    private Image siteIcon;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI urlText;

    [SerializeField]
    private TextMeshProUGUI summaryText;

    [SerializeField]
    private Button titleButton;

    private SearchResultData resultData;
    private SearchWindowUI searchWindowUI;


    public void Setup(
        SearchResultData data,
        SearchSiteIconDatabase iconDatabase,
        SearchWindowUI windowUI
    )
    {
        resultData = data;
        searchWindowUI = windowUI;

        if (titleText != null)
            titleText.text = data.title;

        if (urlText != null)
            urlText.text = data.displayUrl;

        if (summaryText != null)
            summaryText.text = data.summary;

        if (siteIcon != null && iconDatabase != null)
        {
            siteIcon.sprite =
                iconDatabase.GetIcon(data.siteType);
        }

        if (titleButton != null)
        {
            titleButton.onClick.RemoveListener(
                OnTitleClicked
            );

            titleButton.onClick.AddListener(
                OnTitleClicked
            );
        }
    }


    private void OnTitleClicked()
    {
        if (resultData == null)
        {
            Debug.LogWarning(
                "SearchResultDataがありません。"
            );

            return;
        }

        if (searchWindowUI == null)
        {
            Debug.LogWarning(
                "SearchWindowUIがありません。"
            );

            return;
        }

        searchWindowUI.OpenSearchResult(
            resultData
        );
    }
}