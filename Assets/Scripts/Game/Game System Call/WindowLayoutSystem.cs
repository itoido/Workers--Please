using System.Collections;
using UnityEngine;
using DG.Tweening;

public class WindowLayoutSystem : MonoBehaviour
{
    // =========================================================
    // Window設定
    // =========================================================

    [System.Serializable]
    public class WindowLayoutData
    {
        [Header("対象Window")]

        [Tooltip("配置・表示するWindow")]
        public GameWindow gameWindow;


        [Header("デフォルト配置")]

        [Tooltip("Windowのデフォルト位置")]
        public Vector2 defaultPosition;

        [Tooltip("Windowのデフォルトサイズ")]
        public Vector2 defaultSize =
            new Vector2(
                600f,
                500f
            );


        // =====================================================
        // 自動表示設定
        // =====================================================

        [Header("自動表示")]

        [Tooltip(
            "ONの場合、EyeCatch終了後の順番表示に含めます。" +
            "OFFの場合、位置とサイズだけ初期化して非表示のままにします。"
        )]
        public bool showOnSequence = true;


        // =====================================================
        // 登場アニメーション
        // =====================================================

        [Header("登場アニメーション")]

        [Tooltip("このWindowが表示されるまでの追加待機時間")]
        [Min(0f)]
        public float additionalDelay = 0f;
    }


    // =========================================================
    // Window一覧
    // =========================================================

    [Header("Window一覧")]

    [Tooltip(
        "上から順番にWindowを表示します。" +
        "Show On SequenceがOFFのWindowは表示されません。"
    )]
    [SerializeField]
    private WindowLayoutData[] windows;


    // =========================================================
    // 表示タイミング
    // =========================================================

    [Header("表示タイミング")]

    [Tooltip("Window同士の基本表示間隔")]
    [SerializeField]
    private float intervalBetweenWindows = 0.10f;


    // =========================================================
    // Popupアニメーション
    // =========================================================

    [Header("Popupアニメーション")]

    [Tooltip("Popupにかける時間")]
    [SerializeField]
    private float popupDuration = 0.22f;

    [Tooltip("Popup開始時のScale")]
    [SerializeField]
    [Range(0.01f, 1f)]
    private float popupStartScale = 0.85f;

    [Tooltip("PopupアニメーションのEase")]
    [SerializeField]
    private Ease popupEase = Ease.OutBack;


    // =========================================================
    // 内部状態
    // =========================================================

    private Coroutine showWindowsCoroutine;

    private bool isShowingWindows = false;


    // =========================================================
    // Public
    // =========================================================

    public bool IsShowingWindows
    {
        get
        {
            return isShowingWindows;
        }
    }


    // =========================================================
    // 全Windowをデフォルト状態へ戻す
    // =========================================================

    public void ResetAllWindowsToDefault()
    {
        StopShowSequence();


        if (windows == null)
        {
            return;
        }


        foreach (WindowLayoutData data in windows)
        {
            if (data == null ||
                data.gameWindow == null)
            {
                continue;
            }


            ResetWindowToDefault(
                data,
                false
            );
        }
    }


    // =========================================================
    // EyeCatch中の準備
    //
    // 全Windowを
    // ・Default位置
    // ・Defaultサイズ
    // ・非表示
    //
    // にする
    // =========================================================

    public void PrepareWindowsForPopup()
    {
        StopShowSequence();


        if (windows == null)
        {
            return;
        }


        foreach (WindowLayoutData data in windows)
        {
            if (data == null ||
                data.gameWindow == null)
            {
                continue;
            }


            ResetWindowToDefault(
                data,
                true
            );
        }
    }


    // =========================================================
    // Windowを順番に表示
    // =========================================================

    public void ShowWindowsInOrder()
    {
        StopShowSequence();


        showWindowsCoroutine =
            StartCoroutine(
                ShowWindowsRoutine()
            );
    }


    // =========================================================
    // 準備 → 順番表示
    // =========================================================

    public void ResetAndShowWindows()
    {
        PrepareWindowsForPopup();

        ShowWindowsInOrder();
    }


    // =========================================================
    // Window表示Coroutine
    // =========================================================

    private IEnumerator ShowWindowsRoutine()
    {
        isShowingWindows = true;


        if (windows == null ||
            windows.Length == 0)
        {
            isShowingWindows = false;

            showWindowsCoroutine = null;

            yield break;
        }


        foreach (WindowLayoutData data in windows)
        {
            if (data == null ||
                data.gameWindow == null)
            {
                continue;
            }


            // =================================================
            // 自動表示しないWindow
            //
            // PrepareHiddenState()によって
            // 非表示＋TaskBar復元可能状態になっている。
            // =================================================

            if (!data.showOnSequence)
            {
                continue;
            }


            // =================================================
            // 個別Delay
            // =================================================

            if (data.additionalDelay > 0f)
            {
                yield return
                    new WaitForSecondsRealtime(
                        data.additionalDelay
                    );
            }


            // =================================================
            // Popup
            // =================================================

            PlayPopup(
                data
            );


            // =================================================
            // 次のWindowまで待つ
            // =================================================

            if (intervalBetweenWindows > 0f)
            {
                yield return
                    new WaitForSecondsRealtime(
                        intervalBetweenWindows
                    );
            }
        }


        isShowingWindows = false;

        showWindowsCoroutine = null;
    }


    // =========================================================
    // 1つのWindowをPopup
    // =========================================================

    private void PlayPopup(
        WindowLayoutData data
    )
    {
        if (data == null ||
            data.gameWindow == null)
        {
            return;
        }


        GameObject windowObject =
            data.gameWindow.gameObject;


        RectTransform windowRect =
            windowObject.GetComponent<RectTransform>();


        if (windowRect == null)
        {
            Debug.LogWarning(
                "WindowLayoutSystem: " +
                windowObject.name +
                " にRectTransformがありません。"
            );

            return;
        }


        CanvasGroup canvasGroup =
            windowObject.GetComponent<CanvasGroup>();


        // =====================================================
        // GameWindowを通常表示状態へ
        // =====================================================

        data.gameWindow
            .PrepareVisibleState();


        // =====================================================
        // Window表示
        // =====================================================

        windowObject.SetActive(true);


        // =====================================================
        // 最前面
        // =====================================================

        windowObject.transform.SetAsLastSibling();


        // =====================================================
        // Tween停止
        // =====================================================

        windowRect.DOKill();


        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
        }


        // =====================================================
        // Default状態を再設定
        // =====================================================

        ApplyDefaultRect(
            windowRect,
            data
        );


        // =====================================================
        // Popup開始状態
        // =====================================================

        windowRect.localScale =
            Vector3.one *
            popupStartScale;


        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;

            canvasGroup.interactable = false;

            canvasGroup.blocksRaycasts = false;
        }


        // =====================================================
        // Popup Sequence
        // =====================================================

        Sequence sequence =
            DOTween.Sequence();


        sequence.Join(
            windowRect
                .DOScale(
                    1f,
                    popupDuration
                )
                .SetEase(
                    popupEase
                )
        );


        if (canvasGroup != null)
        {
            sequence.Join(
                canvasGroup
                    .DOFade(
                        1f,
                        popupDuration
                    )
                    .SetEase(
                        Ease.OutQuad
                    )
            );
        }


        // =====================================================
        // Popup完了
        // =====================================================

        sequence.OnComplete(
            () =>
            {
                windowRect.localScale =
                    Vector3.one;


                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f;

                    canvasGroup.interactable = true;

                    canvasGroup.blocksRaycasts = true;
                }
            }
        );
    }


    // =========================================================
    // Windowをデフォルトへ戻す
    // =========================================================

    private void ResetWindowToDefault(
        WindowLayoutData data,
        bool hideAfterReset
    )
    {
        if (data == null ||
            data.gameWindow == null)
        {
            return;
        }


        GameObject windowObject =
            data.gameWindow.gameObject;


        // =====================================================
        // RectTransformを操作するため一旦有効化
        // =====================================================

        windowObject.SetActive(true);


        RectTransform windowRect =
            windowObject.GetComponent<RectTransform>();


        if (windowRect == null)
        {
            return;
        }


        CanvasGroup canvasGroup =
            windowObject.GetComponent<CanvasGroup>();


        // =====================================================
        // WindowAnimation停止
        // =====================================================

        WindowAnimation windowAnimation =
            windowObject.GetComponent<WindowAnimation>();


        if (windowAnimation != null)
        {
            windowAnimation
                .KillCurrentAnimation();
        }


        // =====================================================
        // Tween停止
        // =====================================================

        windowRect.DOKill();


        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
        }


        // =====================================================
        // Default配置
        // =====================================================

        ApplyDefaultRect(
            windowRect,
            data
        );


        // =====================================================
        // 見た目を通常状態へ
        // =====================================================

        windowRect.localScale =
            Vector3.one;


        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;

            canvasGroup.interactable = true;

            canvasGroup.blocksRaycasts = true;
        }


        // =====================================================
        // GameWindowの内部状態も同期
        // =====================================================

        if (hideAfterReset)
        {
            // ---------------------------------
            // 現在のDefault位置を復元先として保存
            // ↓
            // 最小化状態
            // ↓
            // 非表示
            // ---------------------------------

            data.gameWindow
                .PrepareHiddenState();
        }
        else
        {
            data.gameWindow
                .PrepareVisibleState();
        }
    }


    // =========================================================
    // Default Rect適用
    // =========================================================

    private void ApplyDefaultRect(
        RectTransform windowRect,
        WindowLayoutData data
    )
    {
        if (windowRect == null ||
            data == null)
        {
            return;
        }


        // =====================================================
        // Anchor
        // =====================================================

        windowRect.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );

        windowRect.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );


        // =====================================================
        // サイズ
        // =====================================================

        windowRect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            data.defaultSize.x
        );


        windowRect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            data.defaultSize.y
        );


        // =====================================================
        // 位置
        // =====================================================

        windowRect.anchoredPosition =
            data.defaultPosition;
    }


    // =========================================================
    // Coroutine停止
    // =========================================================

    private void StopShowSequence()
    {
        if (showWindowsCoroutine != null)
        {
            StopCoroutine(
                showWindowsCoroutine
            );

            showWindowsCoroutine = null;
        }


        isShowingWindows = false;
    }


    // =========================================================
    // Destroy
    // =========================================================

    private void OnDestroy()
    {
        StopShowSequence();


        if (windows == null)
        {
            return;
        }


        foreach (WindowLayoutData data in windows)
        {
            if (data == null ||
                data.gameWindow == null)
            {
                continue;
            }


            RectTransform rect =
                data.gameWindow
                    .GetComponent<RectTransform>();


            if (rect != null)
            {
                rect.DOKill();
            }


            CanvasGroup canvasGroup =
                data.gameWindow
                    .GetComponent<CanvasGroup>();


            if (canvasGroup != null)
            {
                canvasGroup.DOKill();
            }
        }
    }
}