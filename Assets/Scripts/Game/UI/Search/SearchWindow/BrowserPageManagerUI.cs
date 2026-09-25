using UnityEngine;
using TMPro;

public class BrowserPageManager : MonoBehaviour
{
    public enum BrowserPage
    {
        SearchHome,
        SearchLoading,
        SearchResult,

        AIImageHome,

        SNS,
        GitHub,
        PersonalBlog
    }


    // =========================================================
    // URL
    // =========================================================

    [Header("URL欄")]
    [SerializeField]
    private TextMeshProUGUI addressText;


    // =========================================================
    // メインページ
    // =========================================================

    [Header("メインページ")]
    [SerializeField]
    private GameObject searchHomePage;

    [SerializeField]
    private GameObject searchLoadingPage;

    [SerializeField]
    private GameObject searchResultPage;

    [SerializeField]
    private GameObject sites;


    // =========================================================
    // AI画像判定
    // =========================================================

    [Header("AI画像判定")]
    [SerializeField]
    private GameObject aiImageHomePage;


    // =========================================================
    // SNS Prefab
    // =========================================================

    [Header("SNS Prefab")]

    [Tooltip("生成するSNSページのPrefab")]
    [SerializeField]
    private SNSPageUI snsPagePrefab;

    [Tooltip("SNSページを生成する親。通常はSiteを指定")]
    [SerializeField]
    private Transform snsPageParent;

    private SNSPageUI currentSNSPage;


    // =========================================================
    // GitHub Prefab
    // =========================================================

    [Header("GitHub Prefab")]

    [Tooltip("生成するGitHubページのPrefab")]
    [SerializeField]
    private GitHubPageUI githubPagePrefab;

    [Tooltip("GitHubページを生成する親。通常はSiteを指定")]
    [SerializeField]
    private Transform githubPageParent;

    private GitHubPageUI currentGitHubPage;

    // =========================================================
    // PersonalBlog Prefab
    // =========================================================

    [Header("PersonalBlog Prefab")]

    [Tooltip("生成するPersonalBlogページのPrefab")]
    [SerializeField]
    private PersonalBlogPageUI personalBlogPagePrefab;

    [Tooltip("PersonalBlogページを生成する親。通常はSiteを指定")]
    [SerializeField]
    private Transform personalBlogPageParent;

    private PersonalBlogPageUI currentPersonalBlogPage;


    // =========================================================
    // 現在ページ
    // =========================================================

    public BrowserPage CurrentPage { get; private set; }
        = BrowserPage.SearchHome;


    // =========================================================
    // Search Home
    // =========================================================

    public void ShowSearchHome()
    {
        HideAll();

        CurrentPage =
            BrowserPage.SearchHome;

        if (searchHomePage != null)
        {
            searchHomePage.SetActive(true);
        }

        SetAddress(
            "https://search.local/"
        );
    }


    // =========================================================
    // AI Image Detector Home
    // =========================================================

    public void ShowAIImageHome()
    {
        HideAll();

        CurrentPage =
            BrowserPage.AIImageHome;

        if (aiImageHomePage != null)
        {
            aiImageHomePage.SetActive(true);
        }

        SetAddress(
            "https://ai-detector.local/"
        );
    }


    // =========================================================
    // Search Loading
    // =========================================================

    public void ShowSearchLoading(
        string keyword
    )
    {
        HideAll();

        CurrentPage =
            BrowserPage.SearchLoading;

        if (searchLoadingPage != null)
        {
            searchLoadingPage.SetActive(true);
        }

        SetSearchAddress(
            keyword
        );
    }


    // =========================================================
    // Search Result
    // =========================================================

    public void ShowSearchResult(
        string keyword
    )
    {
        HideAll();

        CurrentPage =
            BrowserPage.SearchResult;

        if (searchResultPage != null)
        {
            searchResultPage.SetActive(true);
        }

        SetSearchAddress(
            keyword
        );
    }


    // =========================================================
    // SNS
    // =========================================================

    public void ShowSNS(
        string url,
        SNSPageData data
    )
    {
        HideAll();

        CurrentPage =
            BrowserPage.SNS;


        if (sites != null)
        {
            sites.SetActive(true);
        }


        if (snsPagePrefab == null)
        {
            Debug.LogError(
                "BrowserPageManager: SNSPagePrefabが設定されていません。"
            );

            return;
        }


        if (snsPageParent == null)
        {
            Debug.LogError(
                "BrowserPageManager: SNSPageParentが設定されていません。"
            );

            return;
        }


        currentSNSPage =
            Instantiate(
                snsPagePrefab,
                snsPageParent
            );


        currentSNSPage.Setup(
            data
        );


        SetAddress(
            url
        );
    }


    // =========================================================
    // GitHub
    // =========================================================

    public void ShowGitHub(
        string url,
        GitHubPageData data
    )
    {
        HideAll();

        CurrentPage =
            BrowserPage.GitHub;


        if (sites != null)
        {
            sites.SetActive(true);
        }


        if (githubPagePrefab == null)
        {
            Debug.LogError(
                "BrowserPageManager: GitHubPagePrefabが設定されていません。"
            );

            return;
        }


        if (githubPageParent == null)
        {
            Debug.LogError(
                "BrowserPageManager: GitHubPageParentが設定されていません。"
            );

            return;
        }


        currentGitHubPage =
            Instantiate(
                githubPagePrefab,
                githubPageParent
            );


        currentGitHubPage.Setup(
            data
        );


        SetAddress(
            url
        );
    }


    // =========================================================
    // Personal Blog
    // =========================================================

    public void ShowPersonalBlog(
        string url,
        PersonalBlogPageData data
    )
    {
        HideAll();

        CurrentPage =
            BrowserPage.PersonalBlog;


        if (sites != null)
        {
            sites.SetActive(true);
        }


        if (personalBlogPagePrefab == null)
        {
            Debug.LogError(
                "BrowserPageManager: PersonalBlogPagePrefabが設定されていません。"
            );

            return;
        }


        if (personalBlogPageParent == null)
        {
            Debug.LogError(
                "BrowserPageManager: PersonalBlogPageParentが設定されていません。"
            );

            return;
        }


        currentPersonalBlogPage =
            Instantiate(
                personalBlogPagePrefab,
                personalBlogPageParent
            );


        currentPersonalBlogPage.Setup(
            data
        );


        SetAddress(
            url
        );
    }

    // =========================================================
    // SNSPage削除
    // =========================================================

    private void DestroyCurrentSNSPage()
    {
        if (currentSNSPage == null)
        {
            return;
        }

        Destroy(
            currentSNSPage.gameObject
        );

        currentSNSPage = null;
    }


    // =========================================================
    // GitHubPage削除
    // =========================================================

    private void DestroyCurrentGitHubPage()
    {
        if (currentGitHubPage == null)
        {
            return;
        }

        Destroy(
            currentGitHubPage.gameObject
        );

        currentGitHubPage = null;
    }

    // =========================================================
    // PersonalBlogPage削除
    // =========================================================

    private void DestroyCurrentPersonalBlogPage()
    {
        if (currentPersonalBlogPage == null)
        {
            return;
        }

        Destroy(
            currentPersonalBlogPage.gameObject
        );

        currentPersonalBlogPage = null;
    }


    // =========================================================
    // 全ページを非表示
    // =========================================================

    private void HideAll()
    {
        if (searchHomePage != null)
        {
            searchHomePage.SetActive(false);
        }

        if (searchLoadingPage != null)
        {
            searchLoadingPage.SetActive(false);
        }

        if (searchResultPage != null)
        {
            searchResultPage.SetActive(false);
        }

        if (aiImageHomePage != null)
        {
            aiImageHomePage.SetActive(false);
        }

        if (sites != null)
        {
            sites.SetActive(false);
        }

        // 動的生成したサイトページを削除
        DestroyCurrentSNSPage();
        DestroyCurrentGitHubPage();
        DestroyCurrentPersonalBlogPage();
    }


    // =========================================================
    // Search URL
    // =========================================================

    private void SetSearchAddress(
        string keyword
    )
    {
        string safeKeyword =
            string.IsNullOrEmpty(keyword)
            ? ""
            : keyword.Replace(
                " ",
                "+"
            );

        SetAddress(
            "https://search.local/?q="
            + safeKeyword
        );
    }


    // =========================================================
    // URL設定
    // =========================================================

    private void SetAddress(
        string address
    )
    {
        if (addressText != null)
        {
            addressText.text =
                address;
        }
    }
}