using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AIImageDetectorUI : MonoBehaviour
{
    // =========================================================
    // 管理
    // =========================================================

    [Header("管理")]
    [SerializeField]
    private ApplicantManager applicantManager;


    // =========================================================
    // 画像入力
    // =========================================================

    [Header("画像入力")]
    [Tooltip("入力された顔画像を表示するImage")]
    [SerializeField]
    private Image previewImage;

    [Tooltip("画像入力前に表示する入力枠")]
    [SerializeField]
    private GameObject placeholderObject;


    // =========================================================
    // Analyze
    // =========================================================

    [Header("Analyze")]
    [Tooltip("ANALYZEボタン")]
    [SerializeField]
    private Button analyzeButton;

    [Tooltip("ANALYZEボタン内のText")]
    [SerializeField]
    private TextMeshProUGUI analyzeButtonText;

    [Tooltip("判定にかかる時間")]
    [SerializeField]
    private float analyzeDuration = 2.0f;


    // =========================================================
    // 判定結果
    // =========================================================

    [Header("判定結果")]
    [SerializeField]
    private GameObject resultArea;

    [SerializeField]
    private TextMeshProUGUI probabilityText;

    [SerializeField]
    private TextMeshProUGUI resultText;


    // =========================================================
    // Runtime
    // =========================================================

    private bool isInputSelected;
    private bool hasImage;
    private bool isAnalyzing;

    private Sprite selectedImage;

    private Coroutine analyzeCoroutine;


    // =========================================================
    // Start
    // =========================================================

    private void Start()
    {
        ResetDetector();
    }


    // =========================================================
    // 画像入力欄をクリック
    // =========================================================

    public void SelectImageInput()
    {
        if (isAnalyzing)
        {
            return;
        }

        isInputSelected = true;

        Debug.Log(
            "AI画像判定: 画像入力待ち"
        );
    }


    // =========================================================
    // 履歴書から画像を受け取る
    // =========================================================

    public void SetImage(
        Sprite imageSprite,
        SearchTargetType targetType
    )
    {
        // 画像入力欄を選択していない場合
        if (!isInputSelected)
        {
            Debug.Log(
                "AI画像判定の画像入力欄を先に選択してください。"
            );

            return;
        }


        // FaceImage以外は受け付けない
        if (targetType != SearchTargetType.FaceImage)
        {
            Debug.Log(
                "AI画像判定にはFaceImageのみ入力できます。"
            );

            return;
        }


        if (imageSprite == null)
        {
            Debug.LogWarning(
                "AI画像判定: Spriteがありません。"
            );

            return;
        }


        // -----------------------------------------
        // 画像を保存
        // -----------------------------------------

        selectedImage = imageSprite;

        hasImage = true;

        isInputSelected = false;


        // -----------------------------------------
        // 入力枠を消す
        // -----------------------------------------

        if (placeholderObject != null)
        {
            placeholderObject.SetActive(false);
        }


        // -----------------------------------------
        // 入力画像だけを表示
        // -----------------------------------------

        if (previewImage != null)
        {
            previewImage.sprite = selectedImage;
            previewImage.preserveAspect = true;

            // PreviewImageのGameObject自体を表示
            previewImage.gameObject.SetActive(true);
        }


        // -----------------------------------------
        // ANALYZEボタンを通常状態に戻す
        // -----------------------------------------

        if (analyzeButton != null)
        {
            analyzeButton.gameObject.SetActive(true);
            analyzeButton.interactable = true;
        }


        if (analyzeButtonText != null)
        {
            analyzeButtonText.text = "ANALYZE";
        }


        // -----------------------------------------
        // 前回の結果を消す
        // -----------------------------------------

        HideResult();


        Debug.Log(
            "AI画像判定: 顔画像を受け取りました。"
        );
    }


    // =========================================================
    // 判定開始
    // =========================================================

    public void AnalyzeImage()
    {
        if (isAnalyzing)
        {
            return;
        }


        if (!hasImage)
        {
            Debug.Log(
                "判定する画像が選択されていません。"
            );

            return;
        }


        if (applicantManager == null)
        {
            Debug.LogError(
                "AIImageDetectorUI: ApplicantManagerが設定されていません。"
            );

            return;
        }


        ApplicantData applicant =
            applicantManager.CurrentApplicant;


        if (applicant == null)
        {
            Debug.LogWarning(
                "AIImageDetectorUI: 現在の応募者が存在しません。"
            );

            return;
        }


        if (analyzeCoroutine != null)
        {
            StopCoroutine(analyzeCoroutine);
        }


        analyzeCoroutine =
            StartCoroutine(
                AnalyzeRoutine(applicant)
            );
    }


    // =========================================================
    // 分析中
    // =========================================================

    private IEnumerator AnalyzeRoutine(
        ApplicantData applicant
    )
    {
        isAnalyzing = true;


        // -----------------------------------------
        // 前回の結果を消す
        // -----------------------------------------

        HideResult();


        // -----------------------------------------
        // ボタンを操作不能にする
        // -----------------------------------------

        if (analyzeButton != null)
        {
            analyzeButton.interactable = false;
        }


        // -----------------------------------------
        // ANALYZE → Analyzing...
        // -----------------------------------------

        if (analyzeButtonText != null)
        {
            analyzeButtonText.text = "Analyzing...";
        }


        // -----------------------------------------
        // 指定秒数待つ
        // -----------------------------------------

        yield return new WaitForSecondsRealtime(
            analyzeDuration
        );


        // -----------------------------------------
        // 分析完了
        // ボタンそのものを消す
        // -----------------------------------------

        if (analyzeButton != null)
        {
            analyzeButton.gameObject.SetActive(false);
        }


        // -----------------------------------------
        // 結果表示
        // -----------------------------------------

        ShowResult(applicant);


        isAnalyzing = false;

        analyzeCoroutine = null;
    }


    // =========================================================
    // 結果表示
    // =========================================================

    private void ShowResult(
        ApplicantData applicant
    )
    {
        if (applicant.aiImageDetectionData == null)
        {
            Debug.LogWarning(
                "この応募者にはAI画像判定データが設定されていません。"
            );

            return;
        }


        AIImageDetectionData data =
            applicant.aiImageDetectionData;


        if (probabilityText != null)
        {
            probabilityText.text =
                "AI-generated probability: "
                + data.aiProbability.ToString("0")
                + "%";
        }


        if (resultText != null)
        {
            resultText.text =
                data.resultMessage;
        }


        if (resultArea != null)
        {
            resultArea.SetActive(true);
        }
    }


    // =========================================================
    // Result非表示
    // =========================================================

    private void HideResult()
    {
        if (resultArea != null)
        {
            resultArea.SetActive(false);
        }
    }


    // =========================================================
    // 初期状態へ戻す
    // =========================================================

    public void ResetDetector()
    {
        if (analyzeCoroutine != null)
        {
            StopCoroutine(analyzeCoroutine);

            analyzeCoroutine = null;
        }


        isInputSelected = false;

        hasImage = false;

        isAnalyzing = false;

        selectedImage = null;


        // -----------------------------------------
        // 入力枠を表示
        // -----------------------------------------

        if (placeholderObject != null)
        {
            placeholderObject.SetActive(true);
        }


        // -----------------------------------------
        // Previewを消す
        // -----------------------------------------

        if (previewImage != null)
        {
            previewImage.sprite = null;

            // PreviewImageのGameObject自体を非表示
            previewImage.gameObject.SetActive(false);
        }

        // -----------------------------------------
        // ANALYZEボタンを初期状態へ
        // -----------------------------------------

        if (analyzeButton != null)
        {
            analyzeButton.gameObject.SetActive(true);
            analyzeButton.interactable = true;
        }


        if (analyzeButtonText != null)
        {
            analyzeButtonText.text = "ANALYZE";
        }


        // -----------------------------------------
        // Resultを消す
        // -----------------------------------------

        HideResult();
    }


    // =========================================================
    // Destroy
    // =========================================================

    private void OnDestroy()
    {
        if (analyzeCoroutine != null)
        {
            StopCoroutine(analyzeCoroutine);

            analyzeCoroutine = null;
        }
    }
}