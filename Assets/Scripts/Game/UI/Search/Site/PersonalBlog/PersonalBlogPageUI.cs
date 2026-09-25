using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersonalBlogPageUI : MonoBehaviour
{
    // =========================================================
    // 画像
    // =========================================================

    [Header("画像")]
    [SerializeField]
    private Image bannerImage;

    [SerializeField]
    private Image profileImage;


    // =========================================================
    // プロフィール
    // =========================================================

    [Header("プロフィール")]
    [SerializeField]
    private TextMeshProUGUI displayNameText;

    [SerializeField]
    private TextMeshProUGUI bioText;

    [SerializeField]
    private TextMeshProUGUI followInfoText;

    [SerializeField]
    private TextMeshProUGUI snsUrlText;


    // =========================================================
    // 記事
    // =========================================================

    [Header("記事")]
    [SerializeField]
    private Transform articleContent;

    [SerializeField]
    private BlogArticleItem articleItemPrefab;


    // =========================================================
    // ページ表示
    // =========================================================

    public void Setup(PersonalBlogPageData data)
    {
        if (data == null)
        {
            Debug.LogWarning(
                "PersonalBlogPageDataが設定されていません。"
            );

            return;
        }

        // -------------------------
        // 画像
        // -------------------------

        if (bannerImage != null)
        {
            bannerImage.sprite =
                data.bannerImage;
        }

        if (profileImage != null)
        {
            profileImage.sprite =
                data.profileImage;
        }


        // -------------------------
        // プロフィール
        // -------------------------

        if (displayNameText != null)
        {
            displayNameText.text =
                data.displayName;
        }

        if (bioText != null)
        {
            bioText.text =
                data.bio;
        }

        if (followInfoText != null)
        {
            followInfoText.text =
                data.following
                + " Following   "
                + data.followers
                + " Followers";
        }

        if (snsUrlText != null)
        {
            snsUrlText.text =
                data.snsUrl;
        }


        // -------------------------
        // 記事
        // -------------------------

        ShowArticles(data);
    }


    // =========================================================
    // 記事生成
    // =========================================================

    private void ShowArticles(
        PersonalBlogPageData data
    )
    {
        ClearArticles();

        if (data.articles == null)
            return;

        if (articleContent == null)
        {
            Debug.LogError(
                "ArticleContentが設定されていません。"
            );

            return;
        }

        if (articleItemPrefab == null)
        {
            Debug.LogError(
                "BlogArticleItem Prefabが設定されていません。"
            );

            return;
        }

        foreach (
            BlogArticleData article
            in data.articles
        )
        {
            BlogArticleItem item =
                Instantiate(
                    articleItemPrefab,
                    articleContent
                );

            item.Setup(
                article
            );
        }
    }


    // =========================================================
    // 前回の記事を削除
    // =========================================================

    private void ClearArticles()
    {
        if (articleContent == null)
            return;

        foreach (
            Transform child
            in articleContent
        )
        {
            Destroy(
                child.gameObject
            );
        }
    }
}