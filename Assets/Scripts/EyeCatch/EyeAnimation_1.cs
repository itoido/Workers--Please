using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DefaultEyeCatchAnimation : MonoBehaviour
{
    // =========================================================
    // 表示全体
    // =========================================================

    [Header("EyeCatch表示全体")]

    [Tooltip("BlueMaskや斜線などをまとめている親オブジェクト")]
    [SerializeField]
    private GameObject eyeCatchVisual;


    // =========================================================
    // 水色背景
    // =========================================================

    [Header("水色背景")]

    [SerializeField]
    private RectTransform blueMask;

    [SerializeField]
    private Image blueBackground;


    // =========================================================
    // 先頭の斜線
    // =========================================================

    [Header("先頭の斜線")]

    [SerializeField]
    private RectTransform frontYellow;

    [SerializeField]
    private RectTransform frontGreen;


    // =========================================================
    // 後尾の斜線
    // =========================================================

    [Header("後尾の斜線")]

    [SerializeField]
    private RectTransform backGreen;

    [SerializeField]
    private RectTransform backYellow;


    // =========================================================
    // 中央メッセージ
    // =========================================================

    [Header("中央メッセージ")]

    [SerializeField]
    private TextMeshProUGUI messageText;

    [Tooltip("文字のFade In / Fade Out時間")]
    [SerializeField]
    private float textFadeDuration = 0.15f;

    [Tooltip("メッセージを表示しておく時間")]
    [SerializeField]
    private float textDisplayDuration = 1.0f;

    [Tooltip("表示開始時のScale")]
    [SerializeField]
    private float textStartScale = 0.9f;


    // =========================================================
    // 入場位置
    // =========================================================

    [Header("入場位置")]

    [Tooltip("先頭斜線の開始X")]
    [SerializeField]
    private float frontStartX = -2200f;

    [Tooltip("黄色斜線が止まるX")]
    [SerializeField]
    private float frontYellowEndX = 1850f;

    [Tooltip("緑斜線が止まるX")]
    [SerializeField]
    private float frontGreenEndX = 1650f;


    // =========================================================
    // 後尾位置
    // =========================================================

    [Header("後尾位置")]

    [Tooltip("後尾斜線の開始X")]
    [SerializeField]
    private float backStartX = -2400f;

    [Tooltip("後尾斜線が完全に抜けるX")]
    [SerializeField]
    private float backEndX = 2400f;


    // =========================================================
    // BlueMask
    // =========================================================

    [Header("BlueMask")]

    [Tooltip("水色Maskの最大幅")]
    [SerializeField]
    private float blueMaskFullWidth = 1920f;

    [Tooltip("水色Maskの初期X位置")]
    [SerializeField]
    private float blueMaskStartX = 0f;

    [Tooltip("退場時にBlueMaskが移動するX位置")]
    [SerializeField]
    private float blueMaskExitX = 1920f;


    // =========================================================
    // アニメーション時間
    // =========================================================

    [Header("アニメーション時間")]

    [Tooltip("先頭斜線の移動時間")]
    [SerializeField]
    private float frontMoveDuration = 0.6f;

    [Tooltip("FrontYellowとFrontGreenの時間差")]
    [SerializeField]
    private float frontInterval = 0.08f;

    [Tooltip("水色Maskが広がる時間")]
    [SerializeField]
    private float blueAppearDuration = 0.6f;

    [Tooltip("後尾斜線の移動時間")]
    [SerializeField]
    private float backMoveDuration = 0.6f;

    [Tooltip("BackGreenとBackYellowの時間差")]
    [SerializeField]
    private float backInterval = 0.08f;

    [Tooltip("水色Maskが消える時間")]
    [SerializeField]
    private float blueDisappearDuration = 0.6f;


    // =========================================================
    // 内部状態
    // =========================================================

    private bool isPlaying = false;

    private Sequence currentSequence;


    // =========================================================
    // Public
    // =========================================================

    public bool IsPlaying
    {
        get
        {
            return isPlaying;
        }
    }


    // =========================================================
    // 初期化
    // =========================================================

    private void Awake()
    {
        ResetTransition();


        if (eyeCatchVisual != null)
        {
            eyeCatchVisual.SetActive(false);
        }
    }


    // =========================================================
    // EyeCatch再生
    //
    // message
    //      中央に表示する文字
    //
    // onCovered
    //      BlueMaskが画面全体を覆った瞬間
    //
    // onComplete
    //      EyeCatchが完全に終了した瞬間
    // =========================================================

    public void Play(
        string message,
        Action onCovered = null,
        Action onComplete = null
    )
    {
        // =====================================================
        // 連打防止
        // =====================================================

        if (isPlaying)
        {
            return;
        }


        // =====================================================
        // Inspector確認
        // =====================================================

        if (!CheckReferences())
        {
            return;
        }


        isPlaying = true;


        // =====================================================
        // 最前面へ
        // =====================================================

        transform.SetAsLastSibling();


        // =====================================================
        // 表示
        // =====================================================

        eyeCatchVisual.SetActive(true);


        // =====================================================
        // 初期状態
        // =====================================================

        ResetTransition();


        // =====================================================
        // メッセージ設定
        // =====================================================

        if (messageText != null)
        {
            messageText.text = message;
        }


        // =====================================================
        // 古いSequence停止
        // =====================================================

        if (currentSequence != null &&
            currentSequence.IsActive())
        {
            currentSequence.Kill();
        }


        currentSequence =
            DOTween.Sequence();


        // =====================================================
        // 1. FrontYellow
        // =====================================================

        currentSequence.Append(
            frontYellow
                .DOAnchorPosX(
                    frontYellowEndX,
                    frontMoveDuration
                )
                .SetEase(Ease.OutCubic)
        );


        // =====================================================
        // 2. FrontGreen
        // =====================================================

        currentSequence.Insert(
            frontInterval,
            frontGreen
                .DOAnchorPosX(
                    frontGreenEndX,
                    frontMoveDuration
                )
                .SetEase(Ease.OutCubic)
        );


        // =====================================================
        // 3. BlueMask
        // =====================================================

        currentSequence.Insert(
            frontInterval,
            blueMask
                .DOSizeDelta(
                    new Vector2(
                        blueMaskFullWidth,
                        blueMask.sizeDelta.y
                    ),
                    blueAppearDuration
                )
                .SetEase(Ease.OutCubic)
        );


        // =====================================================
        // 画面が完全に覆われる時間
        // =====================================================

        float frontEndTime =
            Mathf.Max(
                frontMoveDuration,
                frontInterval + frontMoveDuration,
                frontInterval + blueAppearDuration
            );


        // =====================================================
        // 4. 完全に覆われた瞬間
        // =====================================================

        currentSequence.InsertCallback(
            frontEndTime,
            () =>
            {
                ShowMessage();

                onCovered?.Invoke();
            }
        );


        // =====================================================
        // 5. メッセージ終了時間
        // =====================================================

        float messageEndTime =
            frontEndTime
            + textDisplayDuration;


        // =====================================================
        // 6. メッセージFade Out
        // =====================================================

        if (messageText != null)
        {
            currentSequence.Insert(
                messageEndTime,
                messageText
                    .DOFade(
                        0f,
                        textFadeDuration
                    )
                    .SetEase(Ease.Linear)
            );
        }


        // =====================================================
        // 退場開始時間
        // =====================================================

        float backStartTime =
            messageEndTime
            + textFadeDuration;


        // =====================================================
        // 7. BackGreen
        // =====================================================

        currentSequence.Insert(
            backStartTime,
            backGreen
                .DOAnchorPosX(
                    backEndX,
                    backMoveDuration
                )
                .SetEase(Ease.InOutCubic)
        );


        // =====================================================
        // 8. BackYellow
        // =====================================================

        currentSequence.Insert(
            backStartTime + backInterval,
            backYellow
                .DOAnchorPosX(
                    backEndX,
                    backMoveDuration
                )
                .SetEase(Ease.InOutCubic)
        );


        // =====================================================
        // 9. BlueMask退場
        // =====================================================

        currentSequence.Insert(
            backStartTime,
            blueMask
                .DOAnchorPosX(
                    blueMaskExitX,
                    blueDisappearDuration
                )
                .SetEase(Ease.InOutCubic)
        );


        // =====================================================
        // 10. FrontYellow退場
        // =====================================================

        currentSequence.Insert(
            backStartTime,
            frontYellow
                .DOAnchorPosX(
                    backEndX,
                    backMoveDuration
                )
                .SetEase(Ease.InCubic)
        );


        // =====================================================
        // 11. FrontGreen退場
        // =====================================================

        currentSequence.Insert(
            backStartTime + frontInterval,
            frontGreen
                .DOAnchorPosX(
                    backEndX,
                    backMoveDuration
                )
                .SetEase(Ease.InCubic)
        );


        // =====================================================
        // EyeCatch終了
        // =====================================================

        currentSequence.OnComplete(
            () =>
            {
                ResetTransition();


                eyeCatchVisual.SetActive(false);


                isPlaying = false;

                currentSequence = null;


                // =================================================
                // 呼び出し元へ終了通知
                // =================================================

                onComplete?.Invoke();
            }
        );
    }


    // =========================================================
    // メッセージ表示
    // =========================================================

    private void ShowMessage()
    {
        if (messageText == null)
        {
            return;
        }


        messageText.gameObject.SetActive(
            true
        );


        messageText.alpha =
            0f;


        messageText.rectTransform.localScale =
            Vector3.one *
            textStartScale;


        messageText
            .DOFade(
                1f,
                textFadeDuration
            )
            .SetEase(Ease.Linear);


        messageText.rectTransform
            .DOScale(
                1f,
                textFadeDuration
            )
            .SetEase(Ease.OutBack);
    }


    // =========================================================
    // 初期状態
    // =========================================================

    private void ResetTransition()
    {
        KillTweens();


        // =====================================================
        // 前方斜線
        // =====================================================

        SetAnchoredX(
            frontYellow,
            frontStartX
        );

        SetAnchoredX(
            frontGreen,
            frontStartX
        );


        // =====================================================
        // 後方斜線
        // =====================================================

        SetAnchoredX(
            backGreen,
            backStartX
        );

        SetAnchoredX(
            backYellow,
            backStartX
        );


        // =====================================================
        // BlueMask
        // =====================================================

        if (blueMask != null)
        {
            Vector2 position =
                blueMask.anchoredPosition;

            position.x =
                blueMaskStartX;

            blueMask.anchoredPosition =
                position;


            Vector2 size =
                blueMask.sizeDelta;

            size.x =
                0f;

            blueMask.sizeDelta =
                size;
        }


        // =====================================================
        // メッセージ
        // =====================================================

        if (messageText != null)
        {
            messageText.alpha =
                0f;

            messageText.rectTransform.localScale =
                Vector3.one;

            messageText.gameObject.SetActive(
                false
            );
        }
    }


    // =========================================================
    // X座標設定
    // =========================================================

    private void SetAnchoredX(
        RectTransform target,
        float x
    )
    {
        if (target == null)
        {
            return;
        }


        Vector2 position =
            target.anchoredPosition;

        position.x =
            x;

        target.anchoredPosition =
            position;
    }


    // =========================================================
    // Inspector確認
    // =========================================================

    private bool CheckReferences()
    {
        if (eyeCatchVisual == null)
        {
            Debug.LogError(
                "DefaultEyeCatchAnimation: EyeCatchVisualが設定されていません。"
            );

            return false;
        }


        if (blueMask == null ||
            blueBackground == null)
        {
            Debug.LogError(
                "DefaultEyeCatchAnimation: BlueMaskまたはBlueBackgroundが設定されていません。"
            );

            return false;
        }


        if (frontYellow == null ||
            frontGreen == null ||
            backGreen == null ||
            backYellow == null)
        {
            Debug.LogError(
                "DefaultEyeCatchAnimation: Front / Backの斜線が設定されていません。"
            );

            return false;
        }


        if (messageText == null)
        {
            Debug.LogError(
                "DefaultEyeCatchAnimation: MessageTextが設定されていません。"
            );

            return false;
        }


        return true;
    }


    // =========================================================
    // Tween停止
    // =========================================================

    private void KillTweens()
    {
        if (frontYellow != null)
        {
            frontYellow.DOKill();
        }


        if (frontGreen != null)
        {
            frontGreen.DOKill();
        }


        if (backGreen != null)
        {
            backGreen.DOKill();
        }


        if (backYellow != null)
        {
            backYellow.DOKill();
        }


        if (blueMask != null)
        {
            blueMask.DOKill();
        }


        if (messageText != null)
        {
            messageText.DOKill();

            messageText.rectTransform.DOKill();
        }
    }


    // =========================================================
    // 破棄
    // =========================================================

    private void OnDestroy()
    {
        if (currentSequence != null)
        {
            currentSequence.Kill();
        }


        KillTweens();
    }
}