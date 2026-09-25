using UnityEngine;
using UnityEngine.EventSystems;

public class WindowResizeHandler :
    MonoBehaviour,
    IDragHandler
{
    // =========================================================
    // Window
    // =========================================================

    [Header("サイズ変更対象")]
    [SerializeField]
    private RectTransform targetWindow;


    // =========================================================
    // WindowArea
    // =========================================================

    [Header("ウィンドウを収める領域")]
    [SerializeField]
    private RectTransform windowArea;


    // =========================================================
    // 最小サイズ
    // =========================================================

    [Header("最小サイズ")]

    [SerializeField]
    private float minWidth = 400f;

    [SerializeField]
    private float minHeight = 300f;


    // =========================================================
    // 内部参照
    // =========================================================

    private Canvas canvas;


    // =========================================================
    // 初期化
    // =========================================================

    private void Awake()
    {
        canvas =
            GetComponentInParent<Canvas>();


        // 未設定ならResizeHandleの親をWindowとする
        if (targetWindow == null)
        {
            targetWindow =
                transform.parent
                as RectTransform;
        }
    }


    // =========================================================
    // リサイズ
    // =========================================================

    public void OnDrag(
        PointerEventData eventData
    )
    {
        if (targetWindow == null ||
            windowArea == null ||
            canvas == null)
        {
            return;
        }


        // ---------------------------------
        // マウス移動量
        // ---------------------------------

        Vector2 delta =
            eventData.delta /
            canvas.scaleFactor;


        // ---------------------------------
        // 希望サイズ
        // ---------------------------------

        float newWidth =
            targetWindow.rect.width
            + delta.x;

        float newHeight =
            targetWindow.rect.height
            - delta.y;


        // ---------------------------------
        // 最小サイズ
        // ---------------------------------

        newWidth =
            Mathf.Max(
                newWidth,
                minWidth
            );

        newHeight =
            Mathf.Max(
                newHeight,
                minHeight
            );


        // ---------------------------------
        // WindowAreaを超えるサイズにはしない
        // ---------------------------------

        float maxWidth =
            windowArea.rect.width;

        float maxHeight =
            windowArea.rect.height;


        newWidth =
            Mathf.Clamp(
                newWidth,
                minWidth,
                maxWidth
            );

        newHeight =
            Mathf.Clamp(
                newHeight,
                minHeight,
                maxHeight
            );


        // ---------------------------------
        // サイズ変更
        // ---------------------------------

        targetWindow.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            newWidth
        );

        targetWindow.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            newHeight
        );


        // ---------------------------------
        // リサイズ後に位置補正
        // ---------------------------------

        KeepInsideWindowArea();
    }


    // =========================================================
    // WindowArea内へ収める
    // =========================================================

    private void KeepInsideWindowArea()
    {
        if (targetWindow == null ||
            windowArea == null)
        {
            return;
        }


        Vector3[] windowCorners =
            new Vector3[4];

        Vector3[] areaCorners =
            new Vector3[4];


        targetWindow.GetWorldCorners(
            windowCorners
        );

        windowArea.GetWorldCorners(
            areaCorners
        );


        // 0 = 左下
        // 1 = 左上
        // 2 = 右上
        // 3 = 右下


        float moveX = 0f;
        float moveY = 0f;


        // ---------------------------------
        // 左端
        // ---------------------------------

        if (windowCorners[0].x <
            areaCorners[0].x)
        {
            moveX =
                areaCorners[0].x
                - windowCorners[0].x;
        }


        // ---------------------------------
        // 右端
        // ---------------------------------

        if (windowCorners[2].x >
            areaCorners[2].x)
        {
            moveX =
                areaCorners[2].x
                - windowCorners[2].x;
        }


        // ---------------------------------
        // 下端
        // ---------------------------------

        if (windowCorners[0].y <
            areaCorners[0].y)
        {
            moveY =
                areaCorners[0].y
                - windowCorners[0].y;
        }


        // ---------------------------------
        // 上端
        // ---------------------------------

        if (windowCorners[2].y >
            areaCorners[2].y)
        {
            moveY =
                areaCorners[2].y
                - windowCorners[2].y;
        }


        if (Mathf.Approximately(moveX, 0f) &&
            Mathf.Approximately(moveY, 0f))
        {
            return;
        }


        // =====================================================
        // World → 親RectTransformのLocalへ変換
        // =====================================================

        RectTransform parentRect =
            targetWindow.parent
            as RectTransform;


        if (parentRect == null)
            return;


        Vector3 worldCorrection =
            new Vector3(
                moveX,
                moveY,
                0f
            );


        Vector3 localCorrection =
            parentRect.InverseTransformVector(
                worldCorrection
            );


        targetWindow.anchoredPosition +=
            new Vector2(
                localCorrection.x,
                localCorrection.y
            );
    }
}