using UnityEngine;

public class GameWindow : MonoBehaviour
{
    // =========================================================
    // ウィンドウ内容
    // =========================================================

    [Header("ウィンドウ内容")]
    [SerializeField]
    private GameObject content;


    // =========================================================
    // 最大化設定
    // =========================================================

    [Header("最大化設定")]
    [SerializeField]
    private RectTransform windowArea;

    [SerializeField]
    private GameObject resizeHandle;


    // =========================================================
    // アニメーション
    // =========================================================

    [Header("ウィンドウアニメーション")]
    [SerializeField]
    private WindowAnimation windowAnimation;


    // =========================================================
    // 内部参照
    // =========================================================

    private RectTransform windowRect;


    // =========================================================
    // 状態
    // =========================================================

    private bool isMinimized = false;
    private bool isMaximized = false;


    // =========================================================
    // 最大化前の状態
    // =========================================================

    private Vector2 normalPosition;
    private Vector2 normalSize;
    private Vector2 normalAnchorMin;
    private Vector2 normalAnchorMax;
    private Vector2 normalPivot;


    // =========================================================
    // 初期化
    // =========================================================

    private void Awake()
    {
        windowRect =
            GetComponent<RectTransform>();


        if (windowAnimation == null)
        {
            windowAnimation =
                GetComponent<WindowAnimation>();
        }
    }


    // =========================================================
    // TaskBarからWindowの表示状態を切り替える
    // =========================================================

    public void ToggleWindow()
    {
        if (windowAnimation != null &&
            windowAnimation.IsAnimating)
        {
            return;
        }


        // =====================================================
        // 最小化されている
        // → 復元
        // =====================================================

        if (isMinimized)
        {
            OpenWindow();

            return;
        }


        // =====================================================
        // 非表示
        // → 通常Open
        // =====================================================

        if (!gameObject.activeSelf)
        {
            OpenWindow();

            return;
        }


        // =====================================================
        // 表示中
        // → 最小化
        // =====================================================

        MinimizeWindow();
    }


    // =========================================================
    // Windowを開く
    // =========================================================

    public void OpenWindow()
    {
        if (windowAnimation != null &&
            windowAnimation.IsAnimating)
        {
            return;
        }


        // =====================================================
        // 最前面
        // =====================================================

        transform.SetAsLastSibling();


        // =====================================================
        // 最小化状態から復元
        // =====================================================

        if (isMinimized)
        {
            gameObject.SetActive(true);


            if (content != null)
            {
                content.SetActive(true);
            }


            // =================================================
            // TaskBarから復元アニメーション
            // =================================================

            if (windowAnimation != null)
            {
                windowAnimation.PlayRestore(
                    () =>
                    {
                        isMinimized = false;
                    }
                );

                return;
            }


            isMinimized = false;

            return;
        }


        // =====================================================
        // 通常Open
        // =====================================================

        gameObject.SetActive(true);


        if (content != null)
        {
            content.SetActive(true);
        }


        isMinimized = false;
    }


    // =========================================================
    // Windowを閉じる
    // =========================================================

    public void CloseWindow()
    {
        if (windowAnimation != null)
        {
            windowAnimation
                .KillCurrentAnimation();
        }


        // CloseはMinimizeではない
        isMinimized = false;


        gameObject.SetActive(false);
    }


    // =========================================================
    // Windowを最小化
    // =========================================================

    public void MinimizeWindow()
    {
        if (isMinimized)
        {
            return;
        }


        if (windowAnimation != null &&
            windowAnimation.IsAnimating)
        {
            return;
        }


        // =====================================================
        // WindowAnimationあり
        // =====================================================

        if (windowAnimation != null)
        {
            isMinimized = true;


            windowAnimation.PlayMinimize(
                () =>
                {
                    gameObject.SetActive(false);
                }
            );

            return;
        }


        // =====================================================
        // Animationなし
        // =====================================================

        isMinimized = true;

        gameObject.SetActive(false);
    }


    // =========================================================
    // システムによる初期非表示状態
    //
    // WindowLayoutSystemから使用する。
    //
    // 現在位置を復元先として保存し、
    // TaskBarから復元できる状態にして非表示にする。
    // =========================================================

    public void PrepareHiddenState()
    {
        // =====================================================
        // 実行中Animation停止
        // =====================================================

        if (windowAnimation != null)
        {
            windowAnimation
                .KillCurrentAnimation();
        }


        // =====================================================
        // 現在位置を復元先として保存
        //
        // WindowLayoutSystem側でDefault位置・サイズを
        // 適用した「後」に呼ばれる。
        // =====================================================

        if (windowAnimation != null)
        {
            windowAnimation
                .SaveRestoreState();
        }


        // =====================================================
        // 最大化状態を解除
        // =====================================================

        isMaximized = false;


        if (resizeHandle != null)
        {
            resizeHandle.SetActive(true);
        }


        // =====================================================
        // TaskBarから復元可能な状態として扱う
        // =====================================================

        isMinimized = true;


        // =====================================================
        // 非表示
        // =====================================================

        gameObject.SetActive(false);
    }


    // =========================================================
    // システムによる通常表示状態
    //
    // WindowLayoutSystemが自動PopupするWindowに使用する。
    // =========================================================

    public void PrepareVisibleState()
    {
        if (windowAnimation != null)
        {
            windowAnimation
                .KillCurrentAnimation();
        }


        isMinimized = false;
        isMaximized = false;


        if (resizeHandle != null)
        {
            resizeHandle.SetActive(true);
        }
    }


    // =========================================================
    // 最大化切り替え
    // =========================================================

    public void ToggleMaximize()
    {
        if (windowAnimation != null &&
            windowAnimation.IsAnimating)
        {
            return;
        }


        if (!isMaximized)
        {
            MaximizeWindow();
        }
        else
        {
            RestoreWindow();
        }
    }


    // =========================================================
    // 最大化
    // =========================================================

    private void MaximizeWindow()
    {
        if (windowRect == null ||
            windowArea == null)
        {
            return;
        }


        // =====================================================
        // 現在状態を保存
        // =====================================================

        normalPosition =
            windowRect.anchoredPosition;

        normalSize =
            windowRect.rect.size;

        normalAnchorMin =
            windowRect.anchorMin;

        normalAnchorMax =
            windowRect.anchorMax;

        normalPivot =
            windowRect.pivot;


        // =====================================================
        // 最大化
        // =====================================================

        windowRect.anchorMin =
            Vector2.zero;

        windowRect.anchorMax =
            Vector2.one;

        windowRect.offsetMin =
            Vector2.zero;

        windowRect.offsetMax =
            Vector2.zero;


        isMaximized = true;


        transform.SetAsLastSibling();


        if (resizeHandle != null)
        {
            resizeHandle.SetActive(false);
        }
    }


    // =========================================================
    // 最大化解除
    // =========================================================

    private void RestoreWindow()
    {
        if (windowRect == null)
        {
            return;
        }


        windowRect.anchorMin =
            normalAnchorMin;

        windowRect.anchorMax =
            normalAnchorMax;

        windowRect.pivot =
            normalPivot;


        windowRect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            normalSize.x
        );

        windowRect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            normalSize.y
        );


        windowRect.anchoredPosition =
            normalPosition;


        isMaximized = false;


        if (resizeHandle != null)
        {
            resizeHandle.SetActive(true);
        }
    }
}