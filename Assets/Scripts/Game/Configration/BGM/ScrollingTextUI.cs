using UnityEngine;
using TMPro;
using System.Collections;

public class ScrollingTextUI : MonoBehaviour
{
    [Header("スクロール対象")]
    [SerializeField]
    private TextMeshProUGUI targetText;

    [Header("表示領域")]
    [SerializeField]
    private RectTransform viewport;

    [Header("スクロール速度")]
    [SerializeField]
    private float scrollSpeed = 50f;

    [Header("端で待つ時間")]
    [SerializeField]
    private float waitTime = 1.0f;

    private RectTransform textRect;
    private Coroutine scrollCoroutine;

    private Vector2 initialPosition;


    private void Awake()
    {
        if (targetText != null)
        {
            textRect =
                targetText.rectTransform;

            initialPosition =
                textRect.anchoredPosition;
        }
    }


    private void OnEnable()
    {
        Refresh();
    }


    // =========================================================
    // 曲名が変更されたときに呼ぶ
    // =========================================================

    public void Refresh()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
            scrollCoroutine = null;
        }

        if (targetText == null ||
            viewport == null ||
            textRect == null)
        {
            return;
        }

        textRect.anchoredPosition =
            initialPosition;

        // TMPのレイアウト情報を更新
        targetText.ForceMeshUpdate();

        Canvas.ForceUpdateCanvases();

        float textWidth =
            targetText.preferredWidth;

        float viewportWidth =
            viewport.rect.width;

        // -----------------------------------------
        // 表示領域に収まっている
        // -----------------------------------------

        if (textWidth <= viewportWidth)
        {
            textRect.anchoredPosition =
                initialPosition;

            return;
        }

        // -----------------------------------------
        // 長い場合だけスクロール開始
        // -----------------------------------------

        scrollCoroutine =
            StartCoroutine(
                ScrollRoutine(
                    textWidth,
                    viewportWidth
                )
            );
    }


    // =========================================================
    // スクロール
    // =========================================================

    private IEnumerator ScrollRoutine(
        float textWidth,
        float viewportWidth
    )
    {
        float scrollDistance =
            textWidth - viewportWidth;

        while (true)
        {
            // 最初は少し待つ
            yield return new WaitForSecondsRealtime(
                waitTime
            );


            // -----------------------------------------
            // 左へスクロール
            // -----------------------------------------

            Vector2 startPosition =
                initialPosition;

            Vector2 endPosition =
                initialPosition +
                Vector2.left *
                scrollDistance;

            while (
                Vector2.Distance(
                    textRect.anchoredPosition,
                    endPosition
                ) > 0.1f
            )
            {
                textRect.anchoredPosition =
                    Vector2.MoveTowards(
                        textRect.anchoredPosition,
                        endPosition,
                        scrollSpeed *
                        Time.unscaledDeltaTime
                    );

                yield return null;
            }


            textRect.anchoredPosition =
                endPosition;


            // -----------------------------------------
            // 右端まで見せきったら少し待つ
            // -----------------------------------------

            yield return new WaitForSecondsRealtime(
                waitTime
            );


            // -----------------------------------------
            // 左端へ瞬時に戻す
            // -----------------------------------------

            textRect.anchoredPosition =
                initialPosition;
        }
    }


    private void OnDisable()
    {
        if (scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
            scrollCoroutine = null;
        }

        if (textRect != null)
        {
            textRect.anchoredPosition =
                initialPosition;
        }
    }
}