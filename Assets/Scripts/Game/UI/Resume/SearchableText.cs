using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SearchableText : MonoBehaviour, IPointerClickHandler
{
    // =========================================================
    // 検索対象
    // =========================================================

    [Header("検索対象の種類")]
    [SerializeField]
    private SearchTargetType targetType;


    // =========================================================
    // テキスト
    // =========================================================

    [Header("テキスト検索用")]
    [Tooltip("Name / Email / Locationなどの場合に設定")]
    [SerializeField]
    private TextMeshProUGUI targetText;


    // =========================================================
    // 画像
    // =========================================================

    [Header("画像検索用")]
    [Tooltip("FaceImageの場合に設定")]
    [SerializeField]
    private Image targetImage;


    // =========================================================
    // 通常検索
    // =========================================================

    [Header("通常検索")]
    [SerializeField]
    private SearchWindowUI searchWindowUI;


    // =========================================================
    // AI画像判定
    // =========================================================

    [Header("AI画像判定")]
    [SerializeField]
    private AIImageDetectorUI aiImageDetectorUI;


    // =========================================================
    // クリック
    // =========================================================

    public void OnPointerClick(
        PointerEventData eventData
    )
    {
        // -----------------------------------------
        // FaceImage
        // -----------------------------------------

        if (targetType == SearchTargetType.FaceImage)
        {
            SendFaceImage();

            return;
        }


        // -----------------------------------------
        // 通常のテキスト検索
        // -----------------------------------------

        SendText();
    }


    // =========================================================
    // 通常テキストをSearchへ送る
    // =========================================================

    private void SendText()
    {
        if (targetText == null)
        {
            Debug.LogWarning(
                "SearchableText: Target Textが設定されていません。"
            );

            return;
        }


        if (searchWindowUI == null)
        {
            Debug.LogWarning(
                "SearchableText: SearchWindowUIが設定されていません。"
            );

            return;
        }


        searchWindowUI.SetSearchTarget(
            targetText.text,
            targetType
        );
    }


    // =========================================================
    // 顔画像をAI Detectorへ送る
    // =========================================================

    private void SendFaceImage()
    {
        if (targetImage == null)
        {
            Debug.LogWarning(
                "SearchableText: FaceImage用のTarget Imageが設定されていません。"
            );

            return;
        }


        if (targetImage.sprite == null)
        {
            Debug.LogWarning(
                "SearchableText: Target ImageにSpriteがありません。"
            );

            return;
        }


        if (aiImageDetectorUI == null)
        {
            Debug.LogWarning(
                "SearchableText: AIImageDetectorUIが設定されていません。"
            );

            return;
        }


        aiImageDetectorUI.SetImage(
            targetImage.sprite,
            SearchTargetType.FaceImage
        );
    }
}