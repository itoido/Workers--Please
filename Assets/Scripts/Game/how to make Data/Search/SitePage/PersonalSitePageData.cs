using UnityEngine;

[CreateAssetMenu(
    fileName = "PersonalBlogPageData",
    menuName = "Search/Personal Blog Page Data"
)]
public class PersonalBlogPageData : SitePageData
{
    // =========================================================
    // バナー
    // =========================================================

    [Header("バナー画像")]
    public Sprite bannerImage;


    // =========================================================
    // プロフィール
    // =========================================================

    [Header("プロフィール画像")]
    public Sprite profileImage;

    [Header("表示名")]
    public string displayName;

    [Header("プロフィール文")]
    [TextArea(3, 8)]
    public string bio;


    // =========================================================
    // フォロー情報
    // =========================================================

    [Header("フォロー情報")]
    public int followers;
    public int following;


    // =========================================================
    // 外部SNS
    // =========================================================

    [Header("SNS URL")]
    public string snsUrl;


    // =========================================================
    // 投稿記事
    // =========================================================

    [Header("投稿記事")]
    public BlogArticleData[] articles;
}


// =============================================================
// 記事1件分のデータ
// =============================================================

[System.Serializable]
public class BlogArticleData
{
    [Header("記事タイトル")]
    public string title;

    [Header("記事概要")]
    [TextArea(2, 6)]
    public string summary;

    [Header("投稿時期")]
    public string ageText;

    [Header("サムネイル")]
    public Sprite thumbnail;
}