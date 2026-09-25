using System.Collections.Generic;
using UnityEngine;

public class GameResultManager : MonoBehaviour
{
    // =========================================================
    // 判定結果
    // =========================================================

    private readonly List<ApplicantResultData> results =
        new List<ApplicantResultData>();


    // =========================================================
    // 外部参照
    // =========================================================

    public IReadOnlyList<ApplicantResultData> Results
    {
        get
        {
            return results;
        }
    }


    public int TotalCount
    {
        get
        {
            return results.Count;
        }
    }


    public int CorrectCount
    {
        get
        {
            int count = 0;

            foreach (ApplicantResultData result in results)
            {
                if (result != null &&
                    result.isCorrect)
                {
                    count++;
                }
            }

            return count;
        }
    }


    // =========================================================
    // 判定記録
    // =========================================================

    public void RecordResult(
        ApplicantData applicant,
        StampMenuUI.StampType decision
    )
    {
        if (applicant == null)
        {
            Debug.LogError(
                "GameResultManager: Applicantがnullです。"
            );

            return;
        }

        if (decision == StampMenuUI.StampType.None)
        {
            Debug.LogWarning(
                "GameResultManager: Stampが未選択です。"
            );

            return;
        }


        // 同じApplicantを二重登録しない
        foreach (ApplicantResultData existing in results)
        {
            if (existing != null &&
                existing.applicant == applicant)
            {
                Debug.LogWarning(
                    "GameResultManager: このApplicantはすでに記録されています。"
                );

                return;
            }
        }


        ApplicantResultData result =
            new ApplicantResultData(
                applicant,
                decision
            );

        results.Add(result);


        Debug.Log(
            "判定結果を記録: " +
            applicant.applicantName +
            " / " +
            decision +
            " / Correct = " +
            result.isCorrect
        );
    }


    // =========================================================
    // Score
    // =========================================================

    public int GetScore()
    {
        if (TotalCount == 0)
        {
            return 0;
        }

        // 100点満点
        return Mathf.RoundToInt(
            (float)CorrectCount /
            TotalCount *
            100f
        );
    }


    // =========================================================
    // 称号
    // =========================================================

    public string GetTitle()
    {
        int score = GetScore();

        if (score >= 100)
        {
            return "MASTER RECRUITER";
        }

        if (score >= 80)
        {
            return "ACCURATE RECRUITER";
        }

        if (score >= 60)
        {
            return "CAREFUL RECRUITER";
        }

        if (score >= 40)
        {
            return "ROOKIE RECRUITER";
        }

        return "NEEDS MORE TRAINING";
    }


    // =========================================================
    // Reset
    // =========================================================

    public void ResetResults()
    {
        results.Clear();

        Debug.Log(
            "GameResultManager: 判定結果をリセットしました。"
        );
    }
}