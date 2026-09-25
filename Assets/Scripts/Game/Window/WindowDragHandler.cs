using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDragHandler :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler
{
    // =========================================================
    // 設定
    // =========================================================

    [Header("ウィンドウを収める領域")]
    [SerializeField]
    private RectTransform windowArea;


    // =========================================================
    // 内部参照
    // =========================================================

    private RectTransform windowRect;
    private Canvas canvas;


    // =========================================================
    // 初期化
    // =========================================================

    private void Awake()
    {
        // TitleBarの親をWindow本体として取得
        windowRect =
            transform.parent as RectTransform;

        canvas =
            GetComponentInParent<Canvas>();
    }


    // =========================================================
    // ドラッグ開始
    // =========================================================

    public void OnBeginDrag(
        PointerEventData eventData
    )
    {
        if (windowRect == null)
            return;

        // 選択したWindowを最前面へ
        windowRect.SetAsLastSibling();
    }


    // =========================================================
    // ドラッグ中
    // =========================================================

    public void OnDrag(
        PointerEventData eventData
    )
    {
        if (windowRect == null ||
            windowArea == null ||
            canvas == null)
        {
            return;
        }


        // ---------------------------------
        // マウス移動量をCanvas基準へ変換
        // ---------------------------------

        Vector2 delta =
            eventData.delta / canvas.scaleFactor;


        // ---------------------------------
        // Windowを移動
        // ---------------------------------

        windowRect.anchoredPosition +=
            delta;


        // ---------------------------------
        // WindowAreaから出た分を補正
        // ---------------------------------

        KeepInsideWindowArea();
    }


    // =========================================================
    // WindowArea内へ収める
    // =========================================================

    private void KeepInsideWindowArea()
    {
        if (windowRect == null ||
            windowArea == null)
        {
            return;
        }


        Vector3[] windowCorners =
            new Vector3[4];

        Vector3[] areaCorners =
            new Vector3[4];


        windowRect.GetWorldCorners(
            windowCorners
        );

        windowArea.GetWorldCorners(
            areaCorners
        );


        // GetWorldCorners
        //
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


        // 補正不要
        if (Mathf.Approximately(moveX, 0f) &&
            Mathf.Approximately(moveY, 0f))
        {
            return;
        }


        // =====================================================
        // World座標の補正量を、
        // Windowの親座標系へ変換する
        // =====================================================

        RectTransform parentRect =
            windowRect.parent
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


        windowRect.anchoredPosition +=
            new Vector2(
                localCorrection.x,
                localCorrection.y
            );
    }
}