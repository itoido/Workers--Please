using UnityEngine;
using TMPro;
using DG.Tweening;

public class SearchLoadingUI : MonoBehaviour
{
    [Header("検索中表示")]
    [SerializeField] private RectTransform loadingIcon;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private string searchingText = "Searching...";

    [Header("アニメーション")]
    [SerializeField] private float rotationDuration = 0.8f;

    [SerializeField] private float minDisplayTime = 0.6f;
    [SerializeField] private float maxDisplayTime = 1.2f;

    private Tween loadingTween;

    public void StartLoading()
    {
        if (loadingText != null)
            loadingText.text = searchingText;

        StopLoading();

        if (loadingIcon == null)
            return;

        loadingIcon.localRotation = Quaternion.identity;

        loadingTween = loadingIcon
            .DORotate(
                new Vector3(0f, 0f, -360f),
                rotationDuration,
                RotateMode.FastBeyond360
            )
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    public void StopLoading()
    {
        if (loadingTween != null)
        {
            loadingTween.Kill();
            loadingTween = null;
        }

        if (loadingIcon != null)
        {
            loadingIcon.DOKill();
            loadingIcon.localRotation = Quaternion.identity;
        }
    }
    public float GetRandomDisplayTime()
    {
        return Random.Range(
            Mathf.Min(minDisplayTime, maxDisplayTime),
            Mathf.Max(minDisplayTime, maxDisplayTime)
        );
    }

    private void OnDisable()
    {
        StopLoading();
    }

    private void OnDestroy()
    {
        StopLoading();
    }
}
