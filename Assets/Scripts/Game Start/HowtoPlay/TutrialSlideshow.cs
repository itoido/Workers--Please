using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSlideshow : MonoBehaviour
{
    [Header("Image")]
    [Tooltip("画像を表示するUI Image")]
    [SerializeField] private Image displayImage;

    [Header("Slides")]
    [Tooltip("表示する画像を順番に登録")]
    [SerializeField] private Sprite[] slides;

    [Header("Timing")]
    [Tooltip("1枚を表示する時間（秒）")]
    [SerializeField] private float interval = 1.5f;

    private int currentSlideIndex = 0;
    private Coroutine slideshowCoroutine;

    private void OnEnable()
    {
        StartSlideshow();
    }

    private void OnDisable()
    {
        StopSlideshow();
    }

    private void StartSlideshow()
    {
        StopSlideshow();

        if (displayImage == null || slides == null || slides.Length == 0)
            return;

        currentSlideIndex = 0;
        displayImage.sprite = slides[currentSlideIndex];

        // 画像が1枚しかない場合は切り替える必要がない
        if (slides.Length <= 1)
            return;

        slideshowCoroutine = StartCoroutine(SlideshowRoutine());
    }

    private void StopSlideshow()
    {
        if (slideshowCoroutine != null)
        {
            StopCoroutine(slideshowCoroutine);
            slideshowCoroutine = null;
        }
    }

    private IEnumerator SlideshowRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            currentSlideIndex++;

            if (currentSlideIndex >= slides.Length)
                currentSlideIndex = 0;

            displayImage.sprite = slides[currentSlideIndex];
        }
    }
}