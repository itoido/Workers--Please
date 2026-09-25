using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlogArticleItem : MonoBehaviour
{
    [Header("サムネイル")]
    [SerializeField]
    private Image thumbnailImage;

    [Header("記事タイトル")]
    [SerializeField]
    private TextMeshProUGUI articleTitleText;

    [Header("記事概要")]
    [SerializeField]
    private TextMeshProUGUI articleSummaryText;

    [Header("投稿時期")]
    [SerializeField]
    private TextMeshProUGUI articleAgeText;


    public void Setup(BlogArticleData article)
    {
        if (article == null)
            return;

        // タイトル
        if (articleTitleText != null)
        {
            articleTitleText.text =
                article.title;
        }

        // 概要
        if (articleSummaryText != null)
        {
            articleSummaryText.text =
                article.summary;
        }

        // 投稿時期
        if (articleAgeText != null)
        {
            articleAgeText.text =
                article.ageText;
        }

        // サムネイル
        if (thumbnailImage != null)
        {
            if (article.thumbnail != null)
            {
                thumbnailImage.gameObject.SetActive(true);
                thumbnailImage.sprite =
                    article.thumbnail;
            }
            else
            {
                thumbnailImage.gameObject.SetActive(false);
            }
        }
    }
}