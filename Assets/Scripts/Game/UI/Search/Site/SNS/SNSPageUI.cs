using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SNSPageUI : MonoBehaviour
{
    // =========================================================
    // 画像
    // =========================================================

    [Header("画像")]
    [SerializeField]
    private Image coverImage;

    [SerializeField]
    private Image profileImage;


    // =========================================================
    // プロフィール
    // =========================================================

    [Header("プロフィール")]
    [SerializeField]
    private TextMeshProUGUI displayNameText;

    [SerializeField]
    private TextMeshProUGUI userIdText;

    [SerializeField]
    private TextMeshProUGUI bioText;

    [SerializeField]
    private TextMeshProUGUI followersText;

    [SerializeField]
    private TextMeshProUGUI followingText;


    // =========================================================
    // 個人情報
    // =========================================================

    [Header("個人情報")]
    [SerializeField]
    private TextMeshProUGUI locationText;

    [SerializeField]
    private TextMeshProUGUI careerText;

    [SerializeField]
    private TextMeshProUGUI educationText;


    // =========================================================
    // 投稿
    // =========================================================

    [Header("投稿")]
    [SerializeField]
    private Transform postContent;

    [SerializeField]
    private SNSPostItem postItemPrefab;


    // =========================================================
    // SNSページ表示
    // =========================================================

    public void Setup(SNSPageData data)
    {
        if (data == null)
        {
            Debug.LogWarning(
                "SNSPageDataが設定されていません。"
            );

            return;
        }


        // -------------------------
        // 画像
        // -------------------------

        if (coverImage != null)
        {
            coverImage.sprite =
                data.coverImage;
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

        if (userIdText != null)
        {
            userIdText.text =
                data.userId;
        }

        if (bioText != null)
        {
            bioText.text =
                data.bio;
        }

        if (followersText != null)
        {
            followersText.text =
                data.followers
                + " Followers";
        }

        if (followingText != null)
        {
            followingText.text =
                data.following
                + " Following";
        }


        // -------------------------
        // 個人情報
        // -------------------------

        if (locationText != null)
        {
            locationText.text =
                data.location;
        }

        if (careerText != null)
        {
            careerText.text =
                data.career;
        }

        if (educationText != null)
        {
            educationText.text =
                data.education;
        }


        // -------------------------
        // 投稿
        // -------------------------

        ShowPosts(data);
    }


    // =========================================================
    // 投稿生成
    // =========================================================

    private void ShowPosts(SNSPageData data)
    {
        ClearPosts();

        if (data.posts == null)
            return;

        if (postItemPrefab == null)
        {
            Debug.LogError(
                "SNSPostItem Prefabが設定されていません。"
            );

            return;
        }

        if (postContent == null)
        {
            Debug.LogError(
                "PostContentが設定されていません。"
            );

            return;
        }

        foreach (SNSPostData post in data.posts)
        {
            SNSPostItem item =
                Instantiate(
                    postItemPrefab,
                    postContent
                );

            item.Setup(
                post,
                data
            );
        }
    }


    // =========================================================
    // 前回の投稿を削除
    // =========================================================

    private void ClearPosts()
    {
        if (postContent == null)
            return;

        foreach (
            Transform child
            in postContent
        )
        {
            Destroy(
                child.gameObject
            );
        }
    }
}