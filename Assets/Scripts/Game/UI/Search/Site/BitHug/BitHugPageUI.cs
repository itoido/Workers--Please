using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GitHubPageUI : MonoBehaviour
{
    [Header("プロフィール")]
    [SerializeField]
    private Image profileImage;

    [SerializeField]
    private TextMeshProUGUI displayNameText;

    [SerializeField]
    private TextMeshProUGUI userIDText;

    [SerializeField]
    private TextMeshProUGUI bioText;

    [SerializeField]
    private TextMeshProUGUI followersText;


    [Header("連絡先")]
    [SerializeField]
    private TextMeshProUGUI locationText;

    [SerializeField]
    private TextMeshProUGUI emailText;


    [Header("公開情報")]
    [SerializeField]
    private TextMeshProUGUI careerText;

    [SerializeField]
    private TextMeshProUGUI skillsText;

    [SerializeField]
    private TextMeshProUGUI interestsText;

    [SerializeField]
    private TextMeshProUGUI linksText;


    public void Setup(GitHubPageData data)
    {
        if (data == null)
            return;

        if (profileImage != null)
            profileImage.sprite = data.profileImage;

        if (displayNameText != null)
            displayNameText.text = data.displayName;
        
        if (userIDText != null)
            userIDText.text = data.userName;

        if (bioText != null)
            bioText.text = data.bio;

        if (followersText != null)
        {
            followersText.text =
                data.followers
                + " followers · "
                + data.following
                + " following";
        }

        if (locationText != null)
            locationText.text = data.location;

        if (emailText != null)
            emailText.text = data.email;

        if (careerText != null)
            careerText.text = data.career;

        if (skillsText != null)
            skillsText.text = data.skills;

        if (interestsText != null)
            interestsText.text = data.interests;

        if (linksText != null)
            linksText.text = data.links;
    }
}