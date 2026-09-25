using UnityEngine;

public class ApplicantManager : MonoBehaviour
{
    // =========================================================
    // 応募者
    // =========================================================

    [Header("ゲームで使用する応募者")]
    [SerializeField]
    private ApplicantData[] applicants;


    // =========================================================
    // 各Window
    // =========================================================

    [Header("各ウィンドウ")]

    [SerializeField]
    private ResumeWindowUI resumeWindowUI;

    [SerializeField]
    private InterviewWindowUI interviewWindowUI;

    [SerializeField]
    private IPLogWindowUI ipLogWindowUI;


    // =========================================================
    // 内部状態
    // =========================================================

    private int currentApplicantIndex = 0;


    // =========================================================
    // 現在の応募者
    // =========================================================

    public ApplicantData CurrentApplicant
    {
        get;
        private set;
    }


    // =========================================================
    // 最後の応募者か
    // =========================================================

    public bool IsLastApplicant
    {
        get
        {
            if (applicants == null ||
                applicants.Length == 0)
            {
                return false;
            }

            return currentApplicantIndex >=
                   applicants.Length - 1;
        }
    }


    // =========================================================
    // 初期化
    // =========================================================

    private void Start()
    {
        StartGame();
    }


    // =========================================================
    // ゲーム開始
    // =========================================================

    public void StartGame()
    {
        if (applicants == null ||
            applicants.Length == 0)
        {
            Debug.LogError(
                "応募者データが設定されていません。"
            );

            return;
        }


        currentApplicantIndex = 0;

        SelectApplicant(
            currentApplicantIndex
        );
    }


    // =========================================================
    // Applicant選択
    // =========================================================

    private void SelectApplicant(
        int index
    )
    {
        if (index < 0 ||
            index >= applicants.Length)
        {
            return;
        }


        CurrentApplicant =
            applicants[index];


        Debug.Log(
            "現在の応募者: " +
            CurrentApplicant.applicantName
        );


        UpdateAllWindows();
    }


    // =========================================================
    // Window更新
    // =========================================================

    private void UpdateAllWindows()
    {
        if (CurrentApplicant == null)
        {
            return;
        }


        // Resume
        if (resumeWindowUI != null)
        {
            resumeWindowUI.ShowApplicant(
                CurrentApplicant
            );
        }


        // Interview
        if (interviewWindowUI != null)
        {
            interviewWindowUI.ShowInterview(
                CurrentApplicant.interviewData
            );
        }


        // IP Log
        if (ipLogWindowUI != null)
        {
            ipLogWindowUI.ShowLog(
                CurrentApplicant.ipLogFile
            );
        }
    }


    // =========================================================
    // 次の応募者
    // =========================================================

    public void NextApplicant()
    {
        // =====================================================
        // すでに最後なら進めない
        // =====================================================

        if (IsLastApplicant)
        {
            Debug.LogWarning(
                "ApplicantManager: すでに最後の応募者です。"
            );

            return;
        }


        currentApplicantIndex++;


        SelectApplicant(
            currentApplicantIndex
        );
    }
}