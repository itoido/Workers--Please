using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class StartMenuItemEffect :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    // =========================================================
    // Brackets
    // =========================================================

    [Header("Brackets")]

    [Tooltip("左側の [")]
    [SerializeField]
    private RectTransform leftBracket;

    [Tooltip("右側の ]")]
    [SerializeField]
    private RectTransform rightBracket;


    // =========================================================
    // Selection Animation
    // =========================================================

    [Header("Selection Animation")]

    [Tooltip("選択時に括弧を外側へ広げる距離")]
    [SerializeField]
    private float selectedExpandDistance = 30f;

    [Tooltip("選択・解除時に括弧が移動する時間")]
    [SerializeField]
    private float selectMoveDuration = 0.15f;


    // =========================================================
    // Pulse Animation
    // =========================================================

    [Header("Pulse Animation")]

    [Tooltip("何秒ごとに括弧が収縮・拡大するか")]
    [SerializeField]
    private float pulseInterval = 2.5f;

    [Tooltip("選択位置からさらに外側へ動く距離")]
    [SerializeField]
    private float pulseDistance = 5f;

    [Tooltip("1回の細かい移動にかける時間")]
    [SerializeField]
    private float pulseStepDuration = 0.05f;


    // =========================================================
    // Runtime
    // =========================================================

    private Vector2 defaultLeftPosition;
    private Vector2 defaultRightPosition;

    private Vector2 selectedLeftPosition;
    private Vector2 selectedRightPosition;

    private Coroutine selectCoroutine;
    private Coroutine pulseCoroutine;

    private bool isSelected = false;


    // =========================================================
    // Awake
    // =========================================================

    private void Awake()
    {
        if (leftBracket != null)
        {
            defaultLeftPosition =
                leftBracket.anchoredPosition;
        }


        if (rightBracket != null)
        {
            defaultRightPosition =
                rightBracket.anchoredPosition;
        }


        CalculateSelectedPositions();
    }


    // =========================================================
    // Selected Position
    // =========================================================

    private void CalculateSelectedPositions()
    {
        selectedLeftPosition =
            defaultLeftPosition +
            Vector2.left *
            selectedExpandDistance;


        selectedRightPosition =
            defaultRightPosition +
            Vector2.right *
            selectedExpandDistance;
    }


    // =========================================================
    // Pointer Enter
    // =========================================================

    public void OnPointerEnter(
        PointerEventData eventData
    )
    {
        Select();
    }


    // =========================================================
    // Pointer Exit
    // =========================================================

    public void OnPointerExit(
        PointerEventData eventData
    )
    {
        Deselect();
    }


    // =========================================================
    // Select
    // =========================================================

    public void Select()
    {
        if (isSelected)
        {
            return;
        }


        isSelected = true;


        CalculateSelectedPositions();


        // -----------------------------------------------------
        // 既存アニメーション停止
        // -----------------------------------------------------

        if (selectCoroutine != null)
        {
            StopCoroutine(
                selectCoroutine
            );
        }


        if (pulseCoroutine != null)
        {
            StopCoroutine(
                pulseCoroutine
            );

            pulseCoroutine = null;
        }


        // -----------------------------------------------------
        // 通常位置 → 選択位置
        // -----------------------------------------------------

        selectCoroutine =
            StartCoroutine(
                SelectAnimation()
            );
    }


    // =========================================================
    // Select Animation
    // =========================================================

    private IEnumerator SelectAnimation()
    {
        yield return
            MoveBrackets(
                selectedLeftPosition,
                selectedRightPosition,
                selectMoveDuration
            );


        selectCoroutine = null;


        // -----------------------------------------------------
        // 選択されたままならPulse開始
        // -----------------------------------------------------

        if (isSelected)
        {
            pulseCoroutine =
                StartCoroutine(
                    PulseLoop()
                );
        }
    }


    // =========================================================
    // Deselect
    // =========================================================

    public void Deselect()
    {
        if (!isSelected)
        {
            return;
        }


        isSelected = false;


        // -----------------------------------------------------
        // Pulse停止
        // -----------------------------------------------------

        if (pulseCoroutine != null)
        {
            StopCoroutine(
                pulseCoroutine
            );

            pulseCoroutine = null;
        }


        // -----------------------------------------------------
        // 選択アニメーション停止
        // -----------------------------------------------------

        if (selectCoroutine != null)
        {
            StopCoroutine(
                selectCoroutine
            );

            selectCoroutine = null;
        }


        // -----------------------------------------------------
        // 現在位置 → 通常位置
        // -----------------------------------------------------

        selectCoroutine =
            StartCoroutine(
                DeselectAnimation()
            );
    }


    // =========================================================
    // Deselect Animation
    // =========================================================

    private IEnumerator DeselectAnimation()
    {
        yield return
            MoveBrackets(
                defaultLeftPosition,
                defaultRightPosition,
                selectMoveDuration
            );


        selectCoroutine = null;
    }


    // =========================================================
    // Pulse Loop
    // =========================================================

    private IEnumerator PulseLoop()
    {
        while (isSelected)
        {
            yield return
                new WaitForSecondsRealtime(
                    pulseInterval
                );


            if (!isSelected)
            {
                break;
            }


            yield return
                PulseOnce();
        }


        pulseCoroutine = null;
    }


    // =========================================================
    // Pulse Once
    // =========================================================

    private IEnumerator PulseOnce()
    {
        // -----------------------------------------------------
        // 外側位置
        //
        // 左 [
        //   → 左へ
        //
        // 右 ]
        //   → 右へ
        // -----------------------------------------------------

        Vector2 outerLeft =
            selectedLeftPosition +
            Vector2.left *
            pulseDistance;


        Vector2 outerRight =
            selectedRightPosition +
            Vector2.right *
            pulseDistance;


        // -----------------------------------------------------
        // 内側位置
        //
        // 選択位置より少し中央側
        // -----------------------------------------------------

        Vector2 innerLeft =
            selectedLeftPosition +
            Vector2.right *
            pulseDistance;


        Vector2 innerRight =
            selectedRightPosition +
            Vector2.left *
            pulseDistance;


        // =====================================================
        // 1. 最初は外側へ
        //
        // ← [             ] →
        // =====================================================

        yield return
            MoveBrackets(
                outerLeft,
                outerRight,
                pulseStepDuration
            );


        if (!isSelected)
        {
            yield break;
        }


        // =====================================================
        // 2. 一気に内側へ
        //
        // → [           ] ←
        // =====================================================

        yield return
            MoveBrackets(
                innerLeft,
                innerRight,
                pulseStepDuration * 2f
            );


        if (!isSelected)
        {
            yield break;
        }


        // =====================================================
        // 3. もう一度外側へ
        //
        // ← [             ] →
        // =====================================================

        yield return
            MoveBrackets(
                outerLeft,
                outerRight,
                pulseStepDuration * 2f
            );


        if (!isSelected)
        {
            yield break;
        }


        // =====================================================
        // 4. 選択位置へ戻る
        // =====================================================

        yield return
            MoveBrackets(
                selectedLeftPosition,
                selectedRightPosition,
                pulseStepDuration
            );
    }


    // =========================================================
    // Move Brackets
    // =========================================================

    private IEnumerator MoveBrackets(
        Vector2 targetLeft,
        Vector2 targetRight,
        float duration
    )
    {
        if (
            leftBracket == null ||
            rightBracket == null
        )
        {
            yield break;
        }


        Vector2 startLeft =
            leftBracket.anchoredPosition;

        Vector2 startRight =
            rightBracket.anchoredPosition;


        if (duration <= 0f)
        {
            leftBracket.anchoredPosition =
                targetLeft;

            rightBracket.anchoredPosition =
                targetRight;

            yield break;
        }


        float elapsed = 0f;


        while (
            elapsed <
            duration
        )
        {
            elapsed +=
                Time.unscaledDeltaTime;


            float t =
                Mathf.Clamp01(
                    elapsed /
                    duration
                );


            leftBracket.anchoredPosition =
                Vector2.Lerp(
                    startLeft,
                    targetLeft,
                    t
                );


            rightBracket.anchoredPosition =
                Vector2.Lerp(
                    startRight,
                    targetRight,
                    t
                );


            yield return null;
        }


        leftBracket.anchoredPosition =
            targetLeft;

        rightBracket.anchoredPosition =
            targetRight;
    }


    // =========================================================
    // Disable
    // =========================================================

    private void OnDisable()
    {
        if (selectCoroutine != null)
        {
            StopCoroutine(
                selectCoroutine
            );

            selectCoroutine = null;
        }


        if (pulseCoroutine != null)
        {
            StopCoroutine(
                pulseCoroutine
            );

            pulseCoroutine = null;
        }


        isSelected = false;


        if (leftBracket != null)
        {
            leftBracket.anchoredPosition =
                defaultLeftPosition;
        }


        if (rightBracket != null)
        {
            rightBracket.anchoredPosition =
                defaultRightPosition;
        }
    }
}