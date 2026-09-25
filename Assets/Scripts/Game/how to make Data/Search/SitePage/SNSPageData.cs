using UnityEngine;

[CreateAssetMenu(
    fileName = "SNSPageData",
    menuName = "Search/SNS Page Data"
)]
public class SNSPageData : SitePageData
{
    [Header("画像")]
    public Sprite profileImage;
    public Sprite coverImage;

    [Header("プロフィール")]
    public string displayName;
    public string userId;

    [TextArea(2, 5)]
    public string bio;

    public int followers;
    public int following;

    [Header("個人情報")]
    [TextArea(2, 5)]
    public string location;

    [TextArea(2, 8)]
    public string career;

    [TextArea(2, 8)]
    public string education;

    [Header("投稿")]
    public SNSPostData[] posts;
}


[System.Serializable]
public class SNSPostData
{
    [Header("投稿日")]
    public string date;

    [Header("投稿本文")]
    [TextArea(3, 10)]
    public string content;

    [Header("投稿画像")]
    public Sprite image;
}