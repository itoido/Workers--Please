using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    // =========================================================
    // 画面
    // =========================================================

    [Header("画面")]

    [Tooltip("スタート画面")]
    [SerializeField]
    private GameObject startPanel;

    [Tooltip("ゲーム本体")]
    [SerializeField]
    private GameObject gamePanel;

    [Tooltip("終了画面")]
    [SerializeField]
    private GameObject endPanel;


    // =========================================================
    // Applicant
    // =========================================================

    [Header("Applicant")]

    [SerializeField]
    private ApplicantManager applicantManager;


    // =========================================================
    // Result
    // =========================================================

    [Header("Result")]

    [SerializeField]
    private GameResultManager gameResultManager;

    [SerializeField]
    private EndResultUI endResultUI;


    // =========================================================
    // Fade
    // =========================================================

    [Header("Fade")]

    [Tooltip("画面全体を覆う黒いFadePanel")]
    [SerializeField]
    private CanvasGroup fadePanel;

    [Tooltip("暗転にかける時間")]
    [SerializeField]
    private float fadeOutDuration = 0.5f;

    [Tooltip("暗転解除にかける時間")]
    [SerializeField]
    private float fadeInDuration = 0.5f;


    // =========================================================
    // Window
    // =========================================================

    [Header("Window")]

    [Tooltip("Windowの初期化・順番表示を担当するシステム")]
    [SerializeField]
    private WindowLayoutSystem windowLayoutSystem;


    // =========================================================
    // 内部状態
    // =========================================================

    private bool isTransitioning = false;


    // =========================================================
    // 初期化
    // =========================================================

    private void Awake()
    {
        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        if (gamePanel != null)
        {
            gamePanel.SetActive(false);
        }

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
            fadePanel.interactable = false;
            fadePanel.blocksRaycasts = false;
            fadePanel.gameObject.SetActive(false);
        }
    }


    // =========================================================
    // STARTボタン
    // =========================================================

    public void StartGame()
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(
            StartGameRoutine()
        );
    }


    // =========================================================
    // Start → Game
    // =========================================================

    private IEnumerator StartGameRoutine()
    {
        isTransitioning = true;

        PrepareFade();

        yield return Fade(
            0f,
            1f,
            fadeOutDuration
        );


        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        if (gamePanel != null)
        {
            gamePanel.SetActive(true);
        }

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }


        // 新しいゲームなので結果をリセット
        if (gameResultManager != null)
        {
            gameResultManager.ResetResults();
        }


        // Applicantを最初に戻す
        if (applicantManager != null)
        {
            applicantManager.StartGame();
        }


        if (windowLayoutSystem != null)
        {
            windowLayoutSystem
                .PrepareWindowsForPopup();
        }


        yield return Fade(
            1f,
            0f,
            fadeInDuration
        );


        FinishFade();


        if (windowLayoutSystem != null)
        {
            windowLayoutSystem
                .ShowWindowsInOrder();
        }


        isTransitioning = false;

        Debug.Log(
            "Game Start → Window表示開始"
        );
    }


    // =========================================================
    // RETRY
    // =========================================================

    public void RetryGame()
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(
            RetryGameRoutine()
        );
    }


    private IEnumerator RetryGameRoutine()
    {
        isTransitioning = true;

        PrepareFade();

        // End画面を暗転
        yield return Fade(
            0f,
            1f,
            fadeOutDuration
        );


        // =====================================================
        // 結果をリセット
        // =====================================================

        if (gameResultManager != null)
        {
            gameResultManager.ResetResults();
        }


        if (endResultUI != null)
        {
            endResultUI.ClearApplicantResults();
        }


        // =====================================================
        // Applicantを最初に戻す
        // =====================================================

        if (applicantManager != null)
        {
            applicantManager.StartGame();
        }


        // =====================================================
        // End → Game
        // =====================================================

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        if (gamePanel != null)
        {
            gamePanel.SetActive(true);
        }


        // =====================================================
        // Window初期化
        // =====================================================

        if (windowLayoutSystem != null)
        {
            windowLayoutSystem
                .PrepareWindowsForPopup();
        }


        // =====================================================
        // 暗転解除
        // =====================================================

        yield return Fade(
            1f,
            0f,
            fadeInDuration
        );


        FinishFade();


        // =====================================================
        // Window表示
        // =====================================================

        if (windowLayoutSystem != null)
        {
            windowLayoutSystem
                .ShowWindowsInOrder();
        }


        isTransitioning = false;

        Debug.Log(
            "Retry → Game再開始"
        );
    }


    // =========================================================
    // HOME
    // =========================================================

    public void ReturnToHome()
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(
            ReturnToHomeRoutine()
        );
    }


    private IEnumerator ReturnToHomeRoutine()
    {
        isTransitioning = true;

        PrepareFade();


        // End画面を暗転
        yield return Fade(
            0f,
            1f,
            fadeOutDuration
        );


        // =====================================================
        // 結果をリセット
        // =====================================================

        if (gameResultManager != null)
        {
            gameResultManager.ResetResults();
        }


        if (endResultUI != null)
        {
            endResultUI.ClearApplicantResults();
        }


        // =====================================================
        // End → Start
        // =====================================================

        if (gamePanel != null)
        {
            gamePanel.SetActive(false);
        }

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }


        // =====================================================
        // 暗転解除
        // =====================================================

        yield return Fade(
            1f,
            0f,
            fadeInDuration
        );


        FinishFade();


        isTransitioning = false;

        Debug.Log(
            "End → Home"
        );
    }


    // =========================================================
    // End画面を裏側に準備
    // =========================================================

    public void PrepareEndScreen()
    {
        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }


        if (gamePanel != null)
        {
            gamePanel.SetActive(false);
        }


        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }
        else
        {
            Debug.LogError(
                "GameFlowManager: EndPanelが設定されていません。"
            );

            return;
        }


        if (endResultUI != null)
        {
            endResultUI.ShowResults();
        }
        else
        {
            Debug.LogWarning(
                "GameFlowManager: EndResultUIが設定されていません。"
            );
        }


        Debug.Log(
            "GameFlowManager → EyeCatchの裏側をEnd画面へ変更"
        );
    }


    // =========================================================
    // Fade準備
    // =========================================================

    private void PrepareFade()
    {
        if (fadePanel == null)
        {
            return;
        }

        fadePanel.gameObject.SetActive(true);

        fadePanel.transform.SetAsLastSibling();

        fadePanel.alpha = 0f;

        fadePanel.interactable = true;

        fadePanel.blocksRaycasts = true;
    }


    // =========================================================
    // Fade終了
    // =========================================================

    private void FinishFade()
    {
        if (fadePanel == null)
        {
            return;
        }

        fadePanel.alpha = 0f;

        fadePanel.interactable = false;

        fadePanel.blocksRaycasts = false;

        fadePanel.gameObject.SetActive(false);
    }


    // =========================================================
    // Fade
    // =========================================================

    private IEnumerator Fade(
        float from,
        float to,
        float duration
    )
    {
        if (fadePanel == null)
        {
            yield break;
        }


        if (duration <= 0f)
        {
            fadePanel.alpha = to;

            yield break;
        }


        float elapsedTime = 0f;

        fadePanel.alpha = from;


        while (elapsedTime < duration)
        {
            elapsedTime +=
                Time.unscaledDeltaTime;


            float t =
                Mathf.Clamp01(
                    elapsedTime / duration
                );


            fadePanel.alpha =
                Mathf.Lerp(
                    from,
                    to,
                    t
                );


            yield return null;
        }


        fadePanel.alpha = to;
    }
}