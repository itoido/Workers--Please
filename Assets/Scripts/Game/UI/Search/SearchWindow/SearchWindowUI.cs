using UnityEngine;
using TMPro;
using System.Collections;


public class SearchWindowUI : MonoBehaviour
{
    // =========================================================
    // 検索入力
    // =========================================================

    [Header("検索入力")]
    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private TMP_InputField resultSearchInputField;


    // =========================================================
    // 検索結果
    // =========================================================

    [Header("検索結果")]
    [SerializeField] private Transform searchResultContent;
    [SerializeField] private SearchResultItem searchResultItemPrefab;
    [SerializeField] private GameObject noResultsText;

    // =========================================================
    // 管理
    // =========================================================

    [Header("管理")]
    [SerializeField] private BrowserPageManager pageManager;
    [SerializeField] private SearchLoadingUI searchLoadingUI;
    [SerializeField] private SearchSiteIconDatabase iconDatabase;
    [SerializeField] private ApplicantManager applicantManager;
    [SerializeField] private AIImageDetectorUI aiImageDetectorUI;

    // =========================================================
    // Runtime
    // =========================================================

    private bool isSearchInputFocused;
    private bool isSearching;

    private SearchTargetType currentTargetType;

    private TMP_InputField activeSearchInputField;

    private string lastSearchKeyword = "";

    private Coroutine searchCoroutine;


    // =========================================================
    // Start
    // =========================================================

    private void Start()
    {
        if (searchInputField != null)
        {
            searchInputField.onSelect.AddListener(
                OnHomeSearchInputSelected
            );
        }


        if (resultSearchInputField != null)
        {
            resultSearchInputField.onSelect.AddListener(
                OnResultSearchInputSelected
            );
        }


        ShowSearchHome();
    }


    // =========================================================
    // Destroy
    // =========================================================

    private void OnDestroy()
    {
        if (searchInputField != null)
        {
            searchInputField.onSelect.RemoveListener(
                OnHomeSearchInputSelected
            );
        }


        if (resultSearchInputField != null)
        {
            resultSearchInputField.onSelect.RemoveListener(
                OnResultSearchInputSelected
            );
        }


        StopSearchLoading();
    }


    // =========================================================
    // InputField選択
    // =========================================================

    private void OnHomeSearchInputSelected(
        string text
    )
    {
        isSearchInputFocused = true;
        activeSearchInputField = searchInputField;
    }


    private void OnResultSearchInputSelected(
        string text
    )
    {
        isSearchInputFocused = true;
        activeSearchInputField = resultSearchInputField;
    }


    // =========================================================
    // 検索対象設定
    // =========================================================

    public void SetSearchTarget(
        string text,
        SearchTargetType targetType
    )
    {
        if (
            !isSearchInputFocused ||
            activeSearchInputField == null
        )
        {
            Debug.Log(
                "検索InputFieldを先に選択してください。"
            );

            return;
        }


        // 実際に画面へ表示する検索文字列
        activeSearchInputField.text = text;


        // 実際の検索結果判定に使用するTargetType
        currentTargetType = targetType;
    }


    // =========================================================
    // 検索実行
    // =========================================================

    public void ExecuteSearch()
    {
        if (isSearching)
        {
            return;
        }


        if (applicantManager == null)
        {
            Debug.LogError(
                "ApplicantManagerが設定されていません。"
            );

            return;
        }


        ApplicantData applicant =
            applicantManager.CurrentApplicant;


        if (
            applicant == null ||
            applicant.searchData == null
        )
        {
            Debug.LogWarning(
                "現在の応募者、またはSearchDataが設定されていません。"
            );

            return;
        }


        if (activeSearchInputField == null)
        {
            Debug.LogWarning(
                "検索InputFieldが選択されていません。"
            );

            return;
        }


        // -----------------------------------------------------
        // keywordは検索結果の判定には使用しない。
        //
        // SearchWindow上への表示、
        // Loading画面、
        // 検索結果画面への表示のためだけに保持する。
        // -----------------------------------------------------

        string keyword =
            activeSearchInputField.text.Trim();


        if (string.IsNullOrEmpty(keyword))
        {
            Debug.Log(
                "検索キーワードを入力してください。"
            );

            return;
        }


        lastSearchKeyword = keyword;


        searchCoroutine =
            StartCoroutine(
                SearchRoutine(
                    applicant,
                    keyword,
                    currentTargetType
                )
            );
    }


    // =========================================================
    // Search Routine
    // =========================================================

    private IEnumerator SearchRoutine(
        ApplicantData applicant,
        string keyword,
        SearchTargetType targetType
    )
    {
        isSearching = true;


        if (pageManager != null)
        {
            pageManager.ShowSearchLoading(
                keyword
            );
        }


        if (searchLoadingUI != null)
        {
            searchLoadingUI.StartLoading();
        }


        if (searchLoadingUI != null)
        {
            yield return
                new WaitForSecondsRealtime(
                    searchLoadingUI.GetRandomDisplayTime()
                );
        }


        if (searchLoadingUI != null)
        {
            searchLoadingUI.StopLoading();
        }


        PerformSearch(
            applicant,
            keyword,
            targetType
        );


        isSearching = false;
        searchCoroutine = null;
    }


    // =========================================================
    // 実際の検索処理
    // =========================================================

    private void PerformSearch(
        ApplicantData applicant,
        string keyword,
        SearchTargetType targetType
    )
    {
        if (pageManager != null)
        {
            pageManager.ShowSearchResult(
                keyword
            );
        }


        if (resultSearchInputField != null)
        {
            resultSearchInputField.text =
                keyword;
        }


        activeSearchInputField =
            resultSearchInputField;


        ClearSearchResults();


        if (noResultsText != null)
        {
            noResultsText.SetActive(false);
        }


        // -----------------------------------------------------
        // ★変更点
        //
        // Keywordは検索条件として使用しない。
        // 現在の応募者のSearchDataから
        // TargetTypeだけで検索結果を決定する。
        // -----------------------------------------------------

        SearchEntryData matchedEntry =
            FindSearchEntry(
                applicant.searchData,
                targetType
            );


        if (
            matchedEntry == null ||
            matchedEntry.results == null ||
            matchedEntry.results.Length == 0
        )
        {
            if (noResultsText != null)
            {
                noResultsText.SetActive(true);
            }


            isSearchInputFocused = false;

            return;
        }


        ShowSearchResults(
            matchedEntry.results
        );


        isSearchInputFocused = false;
    }


    // =========================================================
    // 検索結果を開く
    // =========================================================

    public void OpenSearchResult(
        SearchResultData result
    )
    {
        if (result == null)
        {
            return;
        }


        switch (result.siteType)
        {
            // -------------------------------------------------
            // Facebook
            // -------------------------------------------------

            case SearchSiteType.Facebook:
            {
                SNSPageData data =
                    result.pageData as SNSPageData;


                if (data == null)
                {
                    Debug.LogError(
                        "Facebook検索結果にSNSPageDataが設定されていません。"
                    );

                    return;
                }


                if (pageManager != null)
                {
                    pageManager.ShowSNS(
                        result.displayUrl,
                        data
                    );
                }

                break;
            }


            // -------------------------------------------------
            // GitHub
            // -------------------------------------------------

            case SearchSiteType.GitHub:
            {
                GitHubPageData data =
                    result.pageData as GitHubPageData;


                if (data == null)
                {
                    Debug.LogError(
                        "GitHub検索結果にGitHubPageDataが設定されていません。"
                    );

                    return;
                }


                if (pageManager != null)
                {
                    pageManager.ShowGitHub(
                        result.displayUrl,
                        data
                    );
                }

                break;
            }


            // -------------------------------------------------
            // Personal Blog
            // -------------------------------------------------

            case SearchSiteType.PersonalBlog:
            {
                PersonalBlogPageData data =
                    result.pageData
                    as PersonalBlogPageData;


                if (data == null)
                {
                    Debug.LogError(
                        "PersonalBlog検索結果にPersonalBlogPageDataが設定されていません。"
                    );

                    return;
                }


                if (pageManager != null)
                {
                    pageManager.ShowPersonalBlog(
                        result.displayUrl,
                        data
                    );
                }


                break;
            }

            // -------------------------------------------------
            // 未対応
            // -------------------------------------------------

            default:
            {
                Debug.LogWarning(
                    "未対応のサイト種類です: " +
                    result.siteType
                );

                break;
            }
        }
    }


    // =========================================================
    // Back
    // =========================================================

    public void GoBack()
    {
        if (pageManager == null)
        {
            return;
        }


        switch (pageManager.CurrentPage)
        {
            case BrowserPageManager.BrowserPage.SearchHome:
                break;


            case BrowserPageManager.BrowserPage.SearchLoading:

                StopSearchLoading();
                ShowSearchHome();

                break;


            case BrowserPageManager.BrowserPage.SearchResult:

                ShowSearchHome();

                break;


            case BrowserPageManager.BrowserPage.SNS:
            case BrowserPageManager.BrowserPage.GitHub:
            case BrowserPageManager.BrowserPage.PersonalBlog:

                RestoreSearchResults();

                break;
        }
    }


    // =========================================================
    // Search Home
    // =========================================================

    public void ShowSearchHome()
    {
        StopSearchLoading();


        if (pageManager != null)
        {
            pageManager.ShowSearchHome();
        }


        if (searchInputField != null)
        {
            searchInputField.text = "";
        }


        activeSearchInputField =
            searchInputField;


        isSearchInputFocused =
            false;


        if (noResultsText != null)
        {
            noResultsText.SetActive(false);
        }


        ClearSearchResults();
    }

    // =========================================================
    // 検索windowをリセット
    // =========================================================
    public void ResetSearchWindow()
    {
        // 実行中の検索を停止
        StopSearchLoading();

        // 検索状態を初期化
        isSearchInputFocused = false;
        isSearching = false;
        currentTargetType = default;
        activeSearchInputField = searchInputField;
        lastSearchKeyword = "";

        // Home側検索欄を空にする
        if (searchInputField != null)
        {
            searchInputField.text = "";
        }

        // Result側検索欄も空にする
        if (resultSearchInputField != null)
        {
            resultSearchInputField.text = "";
        }

        // 生成済み検索結果を削除
        ClearSearchResults();

        if (noResultsText != null)
        {
            noResultsText.SetActive(false);
        }

        // AI画像判定を初期化
        if (aiImageDetectorUI != null)
        {
            aiImageDetectorUI.ResetDetector();
        }

        // 最後に検索Homeへ戻す
        if (pageManager != null)
        {
            pageManager.ShowSearchHome();
        }
    }

    // =========================================================
    // 検索結果へ戻る
    // =========================================================

    private void RestoreSearchResults()
    {
        StopSearchLoading();


        if (pageManager != null)
        {
            pageManager.ShowSearchResult(
                lastSearchKeyword
            );
        }


        if (resultSearchInputField != null)
        {
            resultSearchInputField.text =
                lastSearchKeyword;
        }


        activeSearchInputField =
            resultSearchInputField;


        isSearchInputFocused =
            false;
    }


    // =========================================================
    // Loading停止
    // =========================================================

    private void StopSearchLoading()
    {
        if (searchCoroutine != null)
        {
            StopCoroutine(
                searchCoroutine
            );

            searchCoroutine = null;
        }


        if (searchLoadingUI != null)
        {
            searchLoadingUI.StopLoading();
        }


        isSearching = false;
    }


    // =========================================================
    // SearchEntry検索
    // =========================================================

    private SearchEntryData FindSearchEntry(
        ApplicantSearchData searchData,
        SearchTargetType targetType
    )
    {
        if (
            searchData == null ||
            searchData.searchEntries == null
        )
        {
            return null;
        }


        foreach (
            SearchEntryData entry
            in searchData.searchEntries
        )
        {
            if (entry == null)
            {
                continue;
            }


            // -------------------------------------------------
            // ★TargetTypeだけで判定
            // -------------------------------------------------

            if (entry.targetType == targetType)
            {
                return entry;
            }
        }


        return null;
    }


    // =========================================================
    // 検索結果表示
    // =========================================================

    private void ShowSearchResults(
        SearchResultData[] results
    )
    {
        if (
            results == null ||
            searchResultContent == null ||
            searchResultItemPrefab == null
        )
        {
            return;
        }


        foreach (
            SearchResultData result
            in results
        )
        {
            SearchResultItem item =
                Instantiate(
                    searchResultItemPrefab,
                    searchResultContent
                );


            item.Setup(
                result,
                iconDatabase,
                this
            );
        }
    }


    // =========================================================
    // 検索結果削除
    // =========================================================

    private void ClearSearchResults()
    {
        if (searchResultContent == null)
        {
            return;
        }


        foreach (
            Transform child
            in searchResultContent
        )
        {
            if (
                noResultsText != null &&
                child.gameObject ==
                noResultsText
            )
            {
                continue;
            }


            Destroy(
                child.gameObject
            );
        }
    }
}