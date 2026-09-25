using UnityEngine;

[CreateAssetMenu(
    fileName = "GitHubPageData",
    menuName = "Search/GitHub Page Data"
)]
public class GitHubPageData : SitePageData
{
    [Header("プロフィール画像")]
    public Sprite profileImage;

    [Header("基本情報")]
    public string displayName;
    public string userName;

    [TextArea(2, 5)]
    public string bio;

    [Header("フォロー情報")]
    public int followers;
    public int following;

    [Header("連絡先")]
    public string location;
    public string email;

    [Header("経歴")]
    [TextArea(3, 10)]
    public string career;

    [Header("スキル")]
    [TextArea(3, 10)]
    public string skills;

    [Header("興味・関心")]
    [TextArea(3, 10)]
    public string interests;

    [Header("その他リンク")]
    [TextArea(2, 8)]
    public string links;
}