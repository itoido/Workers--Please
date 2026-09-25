using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndResultUI : MonoBehaviour
{
    // =========================================================
    // Result
    // =========================================================

    [Header("Result")]

    [SerializeField]
    private GameResultManager gameResultManager;


    // =========================================================
    // Summary
    // =========================================================

    [Header("Summary")]

    [SerializeField]
    private TMP_Text correctDecisionText;


    // =========================================================
    // Applicant List
    // =========================================================

    [Header("Applicant List")]

    [SerializeField]
    private Transform applicantList;

    [SerializeField]
    private ApplicantResult applicantResultPrefab;


    // =========================================================
    // Evaluation
    // =========================================================

    [Header("Evaluation")]

    [SerializeField]
    private TMP_Text titleScoreText;

    [SerializeField]
    private TMP_Text numberScoreText;


    // =========================================================
    // 内部
    // =========================================================

    private readonly List<ApplicantResult> spawnedResults =
        new List<ApplicantResult>();


    // =========================================================
    // 結果表示
    // =========================================================

    public void ShowResults()
    {
        ClearApplicantResults();


        if (gameResultManager == null)
        {
            Debug.LogError(
                "EndResultUI: GameResultManagerが設定されていません。"
            );

            return;
        }


        // =====================================================
        // Summary
        // =====================================================

        if (correctDecisionText != null)
        {
            correctDecisionText.text =
                gameResultManager.CorrectCount +
                " / " +
                gameResultManager.TotalCount;
        }


        // =====================================================
        // Applicant
        // =====================================================

        foreach (
            ApplicantResultData result
            in gameResultManager.Results
        )
        {
            CreateApplicantResult(
                result
            );
        }


        // =====================================================
        // Title
        // =====================================================

        if (titleScoreText != null)
        {
            titleScoreText.text =
                gameResultManager.GetTitle();
        }


        // =====================================================
        // Score
        // =====================================================

        if (numberScoreText != null)
        {
            numberScoreText.text =
                gameResultManager
                    .GetScore()
                    .ToString();
        }


        Debug.Log(
            "EndResultUI: 結果画面を生成しました。"
        );
    }


    // =========================================================
    // Applicant生成
    // =========================================================

    private void CreateApplicantResult(
        ApplicantResultData result
    )
    {
        if (applicantList == null)
        {
            Debug.LogError(
                "EndResultUI: ApplicantListが設定されていません。"
            );

            return;
        }


        if (applicantResultPrefab == null)
        {
            Debug.LogError(
                "EndResultUI: ApplicantResultPrefabが設定されていません。"
            );

            return;
        }


        ApplicantResult item =
            Instantiate(
                applicantResultPrefab,
                applicantList
            );


        item.Setup(
            result
        );


        spawnedResults.Add(
            item
        );
    }


    // =========================================================
    // Clear
    // =========================================================

    public void ClearApplicantResults()
    {
        foreach (
            ApplicantResult item
            in spawnedResults
        )
        {
            if (item != null)
            {
                Destroy(
                    item.gameObject
                );
            }
        }


        spawnedResults.Clear();
    }
}