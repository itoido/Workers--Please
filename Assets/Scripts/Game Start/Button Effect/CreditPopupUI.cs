using UnityEngine;


public class CreditPopupUI : MonoBehaviour
{
    // =========================================================
    // Credit Popup
    // =========================================================

    [Header("Credit Popup")]

    [Tooltip("CreditPopupRoot自身を設定")]
    [SerializeField]
    private GameObject creditPopupRoot;


    // =========================================================
    // Open
    // =========================================================

    public void OpenCredit()
    {
        if (creditPopupRoot == null)
        {
            Debug.LogError(
                "CreditPopupUI: CreditPopupRootが設定されていません。"
            );

            return;
        }


        creditPopupRoot.SetActive(true);


        // 他のStart画面UIより前面へ
        creditPopupRoot.transform.SetAsLastSibling();
    }


    // =========================================================
    // Close
    // =========================================================

    public void CloseCredit()
    {
        if (creditPopupRoot == null)
        {
            return;
        }


        creditPopupRoot.SetActive(false);
    }
}