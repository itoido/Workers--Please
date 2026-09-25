using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResumeWindowUI : MonoBehaviour
{
    // =========================================================
    // 基本情報
    // =========================================================

    [Header("基本情報")]

    [SerializeField]
    private Image facialImage;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI locationText;

    [SerializeField]
    private TextMeshProUGUI emailText;


    // =========================================================
    // 履歴書情報
    // =========================================================

    [Header("履歴書情報")]

    [SerializeField]
    private TextMeshProUGUI BackGroundText;

    [SerializeField]
    private TextMeshProUGUI supplementText;


    // =========================================================
    // 応募者情報を表示
    // =========================================================

    public void ShowApplicant(ApplicantData applicant)
    {
        if (applicant == null)
        {
            Debug.LogWarning(
                "ResumeWindowUIにApplicantDataが渡されていません。"
            );

            return;
        }


        // -------------------------
        // 顔画像
        // -------------------------

        if (facialImage != null)
        {
            if (applicant.FacialImage != null)
            {
                facialImage.gameObject.SetActive(true);

                facialImage.sprite =
                    applicant.FacialImage;
            }
            else
            {
                // 応募者に顔画像が設定されていない場合
                facialImage.gameObject.SetActive(false);
            }
        }


        // -------------------------
        // 氏名
        // -------------------------

        if (nameText != null)
        {
            nameText.text =
                applicant.applicantName;
        }


        // -------------------------
        // 在住地
        // -------------------------

        if (locationText != null)
        {
            locationText.text =
                applicant.location;
        }


        // -------------------------
        // Email
        // -------------------------

        if (emailText != null)
        {
            emailText.text =
                applicant.email;
        }


        // -------------------------
        // 学歴
        // -------------------------

        if (BackGroundText != null)
        {
            BackGroundText.text =
                applicant.Background;
        }


        // -------------------------
        // 職歴
        // -------------------------

        if (supplementText != null)
        {
            supplementText.text =
                applicant.supplement;
        }
    }
}