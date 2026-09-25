using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class StampMenuUI : MonoBehaviour
{
    // =========================================================
    // スタンプ種類
    // =========================================================

    public enum StampType
    {
        None,
        Approve,
        Reject
    }


    // =========================================================
    // Popup
    // =========================================================

    [Header("スタンプ選択Popup")]

    [SerializeField]
    private GameObject stampPopup;


    // =========================================================
    // TaskBar Stampボタン
    // =========================================================

    [Header("Stampボタン")]

    [SerializeField]
    private Button stampButton;

    [SerializeField]
    private TextMeshProUGUI stampButtonText;

    [SerializeField]
    private Image stampButtonBackground;


    // =========================================================
    // 表示文字
    // =========================================================

    [Header("表示文字")]

    [SerializeField]
    private string noneText = "Stamp";

    [SerializeField]
    private string approveText = "採用";

    [SerializeField]
    private string rejectText = "非採用";


    // =========================================================
    // 背景色
    // =========================================================

    [Header("Stampボタンの背景色")]

    [SerializeField]
    private Color noneColor = Color.white;

    [SerializeField]
    private Color approveColor =
        new Color(0.65f, 0.9f, 0.65f, 1f);

    [SerializeField]
    private Color rejectColor =
        new Color(1f, 0.65f, 0.65f, 1f);


    // =========================================================
    // Popup内ボタン
    // =========================================================

    [Header("スタンプ選択ボタン")]

    [SerializeField]
    private Button approveButton;

    [SerializeField]
    private Button rejectButton;


    // =========================================================
    // StampArea
    // =========================================================

    [Header("StampArea")]

    [Tooltip("履歴書内のクリック可能なJudgeValueを設定")]
    [SerializeField]
    private RectTransform stampClickArea;


    // =========================================================
    // 内部状態
    // =========================================================

    private bool isOpen = false;

    private StampType selectedStamp =
        StampType.None;

    // 採用/非採用ボタンを押した直後のクリックだけ無視する
    private bool ignoreNextOutsideClick = false;


    // =========================================================
    // 現在選択中
    // =========================================================

    public StampType SelectedStamp
    {
        get
        {
            return selectedStamp;
        }
    }


    // =========================================================
    // 初期化
    // =========================================================

    private void Awake()
    {
        if (stampPopup != null)
        {
            stampPopup.SetActive(false);
        }

        isOpen = false;

        selectedStamp =
            StampType.None;


        if (approveButton != null)
        {
            approveButton.onClick.AddListener(
                SelectApproveStamp
            );
        }


        if (rejectButton != null)
        {
            rejectButton.onClick.AddListener(
                SelectRejectStamp
            );
        }


        UpdateStampButtonDisplay();
    }


    // =========================================================
    // クリック監視
    // =========================================================

    private void Update()
    {
        if (selectedStamp ==
            StampType.None)
        {
            return;
        }


        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }


        // ---------------------------------
        // 採用/非採用を選んだクリック自身は無視
        // ---------------------------------

        if (ignoreNextOutsideClick)
        {
            ignoreNextOutsideClick = false;

            return;
        }


        // ---------------------------------
        // StampArea上なら解除しない
        // ---------------------------------

        if (IsPointerOverStampArea())
        {
            return;
        }


        // ---------------------------------
        // それ以外をクリックした
        // ---------------------------------

        ClearStampSelection();
    }


    // =========================================================
    // 現在のクリック位置がStampAreaか
    // =========================================================

    private bool IsPointerOverStampArea()
    {
        if (stampClickArea == null)
        {
            return false;
        }


        if (EventSystem.current == null)
        {
            return false;
        }


        PointerEventData pointerData =
            new PointerEventData(
                EventSystem.current
            );


        pointerData.position =
            Input.mousePosition;


        List<RaycastResult> results =
            new List<RaycastResult>();


        EventSystem.current.RaycastAll(
            pointerData,
            results
        );


        foreach (RaycastResult result in results)
        {
            Transform hit =
                result.gameObject.transform;


            // JudgeValue自身またはその子ならStampArea扱い
            if (hit == stampClickArea ||
                hit.IsChildOf(stampClickArea))
            {
                return true;
            }
        }


        return false;
    }


    // =========================================================
    // Stampメニュー開閉
    // =========================================================

    public void ToggleStampMenu()
    {
        if (stampPopup == null)
        {
            Debug.LogError(
                "StampPopupが設定されていません。"
            );

            return;
        }


        isOpen = !isOpen;

        stampPopup.SetActive(isOpen);


        if (isOpen)
        {
            stampPopup.transform.SetAsLastSibling();
        }
    }


    // =========================================================
    // Stampメニューを閉じる
    // =========================================================

    public void CloseStampMenu()
    {
        if (stampPopup == null)
            return;


        isOpen = false;

        stampPopup.SetActive(false);
    }


    // =========================================================
    // 採用選択
    // =========================================================

    public void SelectApproveStamp()
    {
        selectedStamp =
            StampType.Approve;


        // このクリックでは選択解除しない
        ignoreNextOutsideClick =
            true;


        UpdateStampButtonDisplay();

        CloseStampMenu();


        Debug.Log(
            "採用スタンプを選択しました。"
        );
    }


    // =========================================================
    // 非採用選択
    // =========================================================

    public void SelectRejectStamp()
    {
        selectedStamp =
            StampType.Reject;


        ignoreNextOutsideClick =
            true;


        UpdateStampButtonDisplay();

        CloseStampMenu();


        Debug.Log(
            "非採用スタンプを選択しました。"
        );
    }


    // =========================================================
    // 選択解除
    // =========================================================

    public void ClearStampSelection()
    {
        selectedStamp =
            StampType.None;


        ignoreNextOutsideClick =
            false;


        UpdateStampButtonDisplay();


        Debug.Log(
            "スタンプ選択を解除しました。"
        );
    }


    // =========================================================
    // 選択されているか
    // =========================================================

    public bool HasSelectedStamp()
    {
        return selectedStamp !=
               StampType.None;
    }


    // =========================================================
    // ボタン表示更新
    // =========================================================

    private void UpdateStampButtonDisplay()
    {
        switch (selectedStamp)
        {
            case StampType.Approve:

                if (stampButtonText != null)
                {
                    stampButtonText.text =
                        approveText;
                }

                if (stampButtonBackground != null)
                {
                    stampButtonBackground.color =
                        approveColor;
                }

                break;


            case StampType.Reject:

                if (stampButtonText != null)
                {
                    stampButtonText.text =
                        rejectText;
                }

                if (stampButtonBackground != null)
                {
                    stampButtonBackground.color =
                        rejectColor;
                }

                break;


            default:

                if (stampButtonText != null)
                {
                    stampButtonText.text =
                        noneText;
                }

                if (stampButtonBackground != null)
                {
                    stampButtonBackground.color =
                        noneColor;
                }

                break;
        }
    }


    // =========================================================
    // Destroy
    // =========================================================

    private void OnDestroy()
    {
        if (approveButton != null)
        {
            approveButton.onClick.RemoveListener(
                SelectApproveStamp
            );
        }


        if (rejectButton != null)
        {
            rejectButton.onClick.RemoveListener(
                SelectRejectStamp
            );
        }
    }
}