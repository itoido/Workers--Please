using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class WindowAnimation : MonoBehaviour
{
    // =========================================================
    // TaskBar
    // =========================================================

    [Header("TaskBar")]

    [Tooltip("このWindowに対応するTaskBar上のButton")]
    [SerializeField]
    private RectTransform taskBarButton;


    // =========================================================
    // アニメーション時間
    // =========================================================

    [Header("アニメーション時間")]

    [Tooltip("最小化アニメーションの時間")]
    [SerializeField]
    private float minimizeDuration = 0.22f;

    [Tooltip("復元アニメーションの時間")]
    [SerializeField]
    private float restoreDuration = 0.25f;


    // =========================================================
    // 最小化時の見た目
    // =========================================================

    [Header("最小化設定")]

    [Tooltip("TaskBarへ到達したときのScale")]
    [SerializeField]
    [Range(0.01f, 1f)]
    private float minimizedScale = 0.12f;

    [Tooltip("最小化時に透明化するか")]
    [SerializeField]
    private bool fadeWhenMinimized = true;


    // =========================================================
    // Ease
    // =========================================================

    [Header("Ease")]

    [Tooltip("最小化時のEase")]
    [SerializeField]
    private Ease minimizeEase = Ease.InCubic;

    [Tooltip("復元時のEase")]
    [SerializeField]
    private Ease restoreEase = Ease.OutCubic;


    // =========================================================
    // 内部参照
    // =========================================================

    private RectTransform windowRect;

    private RectTransform parentRect;

    private CanvasGroup canvasGroup;


    // =========================================================
    // 復元先
    // =========================================================

    private Vector2 restorePosition;

    private Vector3 restoreScale = Vector3.one;

    private float restoreAlpha = 1f;


    // =========================================================
    // Tween
    // =========================================================

    private Sequence currentSequence;


    // =========================================================
    // 状態
    // =========================================================

    private bool isAnimating = false;

    private bool hasRestoreState = false;


    // =========================================================
    // Public
    // =========================================================

    public bool IsAnimating
    {
        get
        {
            return isAnimating;
        }
    }


    // =========================================================
    // 初期化
    // =========================================================

    private void Awake()
    {
        windowRect =
            GetComponent<RectTransform>();

        canvasGroup =
            GetComponent<CanvasGroup>();

        parentRect =
            windowRect.parent as RectTransform;


        // ---------------------------------
        // 初期状態
        // ---------------------------------

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }


    // =========================================================
    // 最小化
    // =========================================================

    public void PlayMinimize(
        Action onComplete = null
    )
    {
        if (isAnimating)
        {
            return;
        }


        if (windowRect == null)
        {
            Debug.LogError(
                "WindowAnimation: RectTransformが取得できません。"
            );

            return;
        }


        if (taskBarButton == null)
        {
            Debug.LogError(
                "WindowAnimation: TaskBar Buttonが設定されていません。"
            );

            return;
        }


        if (parentRect == null)
        {
            Debug.LogError(
                "WindowAnimation: Windowの親RectTransformが取得できません。"
            );

            return;
        }


        // =====================================================
        // 現在状態を復元先として保存
        // =====================================================

        SaveRestoreState();


        // =====================================================
        // TaskBarの位置をWindow親座標へ変換
        // =====================================================

        Vector2 taskBarPosition =
            GetTaskBarPositionInWindowParent();


        // =====================================================
        // 古いTween停止
        // =====================================================

        KillCurrentAnimation();


        isAnimating = true;


        // 操作を無効化
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }


        // =====================================================
        // Sequence
        // =====================================================

        currentSequence =
            DOTween.Sequence();


        // ---------------------------------
        // TaskBarへ移動
        // ---------------------------------

        currentSequence.Join(
            windowRect
                .DOAnchorPos(
                    taskBarPosition,
                    minimizeDuration
                )
                .SetEase(minimizeEase)
        );


        // ---------------------------------
        // 縮小
        // ---------------------------------

        currentSequence.Join(
            windowRect
                .DOScale(
                    minimizedScale,
                    minimizeDuration
                )
                .SetEase(minimizeEase)
        );


        // ---------------------------------
        // Fade Out
        // ---------------------------------

        if (fadeWhenMinimized &&
            canvasGroup != null)
        {
            currentSequence.Join(
                canvasGroup
                    .DOFade(
                        0f,
                        minimizeDuration
                    )
                    .SetEase(minimizeEase)
            );
        }


        // =====================================================
        // 完了
        // =====================================================

        currentSequence.OnComplete(
            () =>
            {
                isAnimating = false;

                onComplete?.Invoke();
            }
        );
    }


    // =========================================================
    // 復元
    // =========================================================

    public void PlayRestore(
        Action onComplete = null
    )
    {
        if (isAnimating)
        {
            return;
        }


        if (windowRect == null)
        {
            Debug.LogError(
                "WindowAnimation: RectTransformが取得できません。"
            );

            return;
        }


        if (taskBarButton == null)
        {
            Debug.LogError(
                "WindowAnimation: TaskBar Buttonが設定されていません。"
            );

            return;
        }


        if (parentRect == null)
        {
            Debug.LogError(
                "WindowAnimation: Windowの親RectTransformが取得できません。"
            );

            return;
        }


        if (!hasRestoreState)
        {
            Debug.LogWarning(
                "WindowAnimation: 復元先が保存されていません。"
            );

            return;
        }


        // =====================================================
        // 古いTween停止
        // =====================================================

        KillCurrentAnimation();


        isAnimating = true;


        // =====================================================
        // TaskBar位置から開始
        // =====================================================

        Vector2 taskBarPosition =
            GetTaskBarPositionInWindowParent();


        windowRect.anchoredPosition =
            taskBarPosition;


        windowRect.localScale =
            Vector3.one *
            minimizedScale;


        if (canvasGroup != null)
        {
            canvasGroup.alpha =
                fadeWhenMinimized
                    ? 0f
                    : restoreAlpha;

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }


        // =====================================================
        // Sequence
        // =====================================================

        currentSequence =
            DOTween.Sequence();


        // ---------------------------------
        // 元の位置へ移動
        // ---------------------------------

        currentSequence.Join(
            windowRect
                .DOAnchorPos(
                    restorePosition,
                    restoreDuration
                )
                .SetEase(restoreEase)
        );


        // ---------------------------------
        // 元のScaleへ
        // ---------------------------------

        currentSequence.Join(
            windowRect
                .DOScale(
                    restoreScale,
                    restoreDuration
                )
                .SetEase(restoreEase)
        );


        // ---------------------------------
        // Fade In
        // ---------------------------------

        if (canvasGroup != null)
        {
            currentSequence.Join(
                canvasGroup
                    .DOFade(
                        restoreAlpha,
                        restoreDuration
                    )
                    .SetEase(restoreEase)
            );
        }


        // =====================================================
        // 完了
        // =====================================================

        currentSequence.OnComplete(
            () =>
            {
                isAnimating = false;


                if (canvasGroup != null)
                {
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }


                onComplete?.Invoke();
            }
        );
    }


    // =========================================================
    // 現在状態を復元先として保存
    // =========================================================

    public void SaveRestoreState()
    {
        if (windowRect == null)
        {
            return;
        }


        restorePosition =
            windowRect.anchoredPosition;


        restoreScale =
            windowRect.localScale;


        if (canvasGroup != null)
        {
            restoreAlpha =
                canvasGroup.alpha;
        }
        else
        {
            restoreAlpha = 1f;
        }


        hasRestoreState = true;
    }


    // =========================================================
    // TaskBar位置取得
    // =========================================================

    private Vector2 GetTaskBarPositionInWindowParent()
    {
        if (taskBarButton == null ||
            parentRect == null)
        {
            return Vector2.zero;
        }


        // =====================================================
        // TaskBar Buttonの中心をWorld座標で取得
        // =====================================================

        Vector3 taskBarWorldPosition =
            taskBarButton.TransformPoint(
                taskBarButton.rect.center
            );


        // =====================================================
        // World → Window親のLocal座標
        // =====================================================

        Vector3 localPosition =
            parentRect.InverseTransformPoint(
                taskBarWorldPosition
            );


        return new Vector2(
            localPosition.x,
            localPosition.y
        );
    }


    // =========================================================
    // Tween停止
    // =========================================================

    public void KillCurrentAnimation()
    {
        if (currentSequence != null &&
            currentSequence.IsActive())
        {
            currentSequence.Kill();
        }


        if (windowRect != null)
        {
            windowRect.DOKill();
        }


        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
        }


        currentSequence = null;

        isAnimating = false;
    }


    // =========================================================
    // 強制的に通常表示状態へ戻す
    // =========================================================

    public void ForceRestoreImmediately()
    {
        KillCurrentAnimation();


        if (!hasRestoreState)
        {
            return;
        }


        windowRect.anchoredPosition =
            restorePosition;


        windowRect.localScale =
            restoreScale;


        if (canvasGroup != null)
        {
            canvasGroup.alpha =
                restoreAlpha;

            canvasGroup.interactable =
                true;

            canvasGroup.blocksRaycasts =
                true;
        }
    }


    // =========================================================
    // Destroy
    // =========================================================

    private void OnDestroy()
    {
        KillCurrentAnimation();
    }
}