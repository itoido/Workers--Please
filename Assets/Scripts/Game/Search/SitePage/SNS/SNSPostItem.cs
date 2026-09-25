using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SNSPostItem : MonoBehaviour
{
    [Header("投稿者")]
    [SerializeField]
    private Image profileImage;

    [SerializeField]
    private TextMeshProUGUI userNameText;

    [Header("投稿内容")]
    [SerializeField]
    private TextMeshProUGUI dateText;

    [SerializeField]
    private TextMeshProUGUI postBodyText;

    [SerializeField]
    private Image postImage;


    public void Setup(
        SNSPostData postData,
        SNSPageData pageData
    )
    {
        if (postData == null)
            return;


        // =================================
        // 投稿者情報
        // SNSPageDataから取得する
        // =================================

        if (pageData != null)
        {
            if (profileImage != null)
            {
                profileImage.sprite =
                    pageData.profileImage;
            }

            if (userNameText != null)
            {
                userNameText.text =
                    pageData.displayName;
            }
        }


        // =================================
        // 投稿日
        // =================================

        if (dateText != null)
        {
            dateText.text =
                postData.date;
        }


        // =================================
        // 投稿本文
        // =================================

        if (postBodyText != null)
        {
            postBodyText.text =
                postData.content;
        }


        // =================================
        // 投稿画像
        // =================================

        if (postImage != null)
        {
            if (postData.image != null)
            {
                postImage.gameObject.SetActive(true);
                postImage.sprite = postData.image;
            }
            else
            {
                postImage.gameObject.SetActive(false);
            }
        }
    }
}