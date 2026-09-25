using TMPro;
using UnityEngine;

public class ApplicantResult : MonoBehaviour
{
    // =========================================================
    // Header
    // =========================================================

    [Header("Header")]

    [SerializeField]
    private TMP_Text applicantNameText;

    [SerializeField]
    private TMP_Text judgementResultIcon;

    [SerializeField]
    private TMP_Text judgmentResultText;


    // =========================================================
    // Decision
    // =========================================================

    [Header("Decision")]

    [SerializeField]
    private TMP_Text yourDecisionText;

    [SerializeField]
    private TMP_Text actualStatusText;


    // =========================================================
    // Explanation
    // =========================================================

    [Header("Explanation")]

    [SerializeField]
    private GameObject explanationArea;

    [SerializeField]
    private TMP_Text explanationText;


    // =========================================================
    // Setup
    // =========================================================

    public void Setup(
        ApplicantResultData result
    )
    {
        if (result == null ||
            result.applicant == null)
        {
            return;
        }


        ApplicantData applicant =
            result.applicant;


        // =====================================================
        // 名前
        // =====================================================

        if (applicantNameText != null)
        {
            applicantNameText.text =
                applicant.applicantName;
        }


        // =====================================================
        // 正誤アイコン
        // =====================================================

        if (judgementResultIcon != null)
        {
            judgementResultIcon.text =
                result.isCorrect
                    ? "a"
                    : "r";
        }


        // =====================================================
        // 正誤テキスト
        // =====================================================

        if (judgmentResultText != null)
        {
            judgmentResultText.text =
                result.isCorrect
                    ? "CORRECT"
                    : "INCORRECT";
        }


        // =====================================================
        // プレイヤーの判断
        // =====================================================

        if (yourDecisionText != null)
        {
            yourDecisionText.text =
                "あなたの判断：" +
                GetDecisionText(
                    result.playerDecision
                );
        }


        // =====================================================
        // 実際の属性
        // =====================================================

        if (actualStatusText != null)
        {
            actualStatusText.text =
                "応募者の属性：" +
                (
                    applicant.isFraud
                        ? "不正応募者"
                        : "正規応募者"
                );
        }


        // =====================================================
        // 不正理由
        // =====================================================

        if (explanationArea != null)
        {
            explanationArea.SetActive(
                applicant.isFraud
            );
        }


        if (applicant.isFraud &&
            explanationText != null)
        {
            explanationText.text =
                string.IsNullOrWhiteSpace(
                    applicant.fraudExplanation
                )
                    ? "判断材料は設定されていません。"
                    : applicant.fraudExplanation;
        }
    }


    // =========================================================
    // 判定表示
    // =========================================================

    private string GetDecisionText(
        StampMenuUI.StampType decision
    )
    {
        switch (decision)
        {
            case StampMenuUI.StampType.Approve:
                return "採用";

            case StampMenuUI.StampType.Reject:
                return "不採用";

            default:
                return "未判断";
        }
    }
}