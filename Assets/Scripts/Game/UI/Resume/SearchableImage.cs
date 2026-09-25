using UnityEngine;
using UnityEngine.UI;


public class AIImageSource : MonoBehaviour
{
    [Header("画像")]
    [SerializeField]
    private Image sourceImage;


    [Header("画像種類")]
    [SerializeField]
    private SearchTargetType targetType =
        SearchTargetType.FaceImage;


    [Header("AI画像判定")]
    [SerializeField]
    private AIImageDetectorUI aiImageDetectorUI;


    // =========================================================
    // Awake
    // =========================================================

    private void Awake()
    {
        if (sourceImage == null)
        {
            sourceImage =
                GetComponent<Image>();
        }
    }


    // =========================================================
    // クリック
    // =========================================================

    public void SendImageToDetector()
    {
        if (aiImageDetectorUI == null)
        {
            Debug.LogError(
                "AIImageSource: AIImageDetectorUIが設定されていません。"
            );

            return;
        }


        if (sourceImage == null)
        {
            Debug.LogError(
                "AIImageSource: SourceImageが設定されていません。"
            );

            return;
        }


        if (sourceImage.sprite == null)
        {
            Debug.LogWarning(
                "AIImageSource: 顔画像Spriteがありません。"
            );

            return;
        }


        aiImageDetectorUI.SetImage(
            sourceImage.sprite,
            targetType
        );
    }
}