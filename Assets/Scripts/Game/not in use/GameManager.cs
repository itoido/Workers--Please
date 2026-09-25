using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("応募者")]
    public ApplicantData[] applicants;

    private int currentApplicantIndex = 0;
    private int correctAnswers = 0;

    private void Start()
    {
        ShowCurrentApplicant();
    }

    public void ShowCurrentApplicant()
    {
        if (currentApplicantIndex >= applicants.Length)
        {
            FinishGame();
            return;
        }

        ApplicantData applicant = applicants[currentApplicantIndex];

        Debug.Log("現在の応募者: " + applicant.applicantName);
        Debug.Log("Email: " + applicant.email);
        Debug.Log("Location: " + applicant.location);
        Debug.Log("EducationBackground: " + applicant.Background);
        Debug.Log("WorkHistory: " + applicant.supplement);
    }

    public void MakeDecision(bool reject)
    {
        ApplicantData applicant = applicants[currentApplicantIndex];

        bool playerSaysFraud = reject;

        if (playerSaysFraud == applicant.isFraud)
        {
            correctAnswers++;

            Debug.Log("正解！");
        }
        else
        {
            Debug.Log("不正解...");
        }

        currentApplicantIndex++;

        ShowCurrentApplicant();
    }

    private void FinishGame()
    {
        Debug.Log("ゲーム終了！");
        Debug.Log("正解数: " + correctAnswers + "/" + applicants.Length);
    }
}