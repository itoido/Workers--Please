using TMPro;
using UnityEngine;

public class HowToPlayUI : MonoBehaviour
{
    [Header("Popup")]
    [Tooltip("遊び方画面全体のPopupRoot")]
    [SerializeField] private GameObject popupRoot;

    [Header("Pages")]
    [Tooltip("Page01, Page02...の順番で登録")]
    [SerializeField] private GameObject[] pages;

    [Header("Arrow Buttons")]
    [Tooltip("左矢印ボタン内のTMP")]
    [SerializeField] private TMP_Text leftArrowText;

    [Tooltip("右矢印ボタン内のTMP")]
    [SerializeField] private TMP_Text rightArrowText;

    [Header("Page Number")]
    [Tooltip("現在のページ番号を表示するTMP")]
    [SerializeField] private TMP_Text pageNumberText;

    [Header("Arrow Characters")]
    [SerializeField] private string normalLeftArrow = "3";
    [SerializeField] private string normalRightArrow = "4";
    [SerializeField] private string closeArrow = "r";

    private int currentPageIndex = 0;

    private void Awake()
    {
        if (popupRoot != null)
            popupRoot.SetActive(false);
    }

    /// <summary>
    /// How to Playボタンから呼び出す
    /// </summary>
    public void Open()
    {
        if (popupRoot == null || pages == null || pages.Length == 0)
            return;

        currentPageIndex = 0;

        popupRoot.SetActive(true);

        ShowCurrentPage();
    }

    /// <summary>
    /// Popupを閉じる
    /// </summary>
    public void Close()
    {
        if (popupRoot != null)
            popupRoot.SetActive(false);
    }

    /// <summary>
    /// 左矢印ボタンから呼び出す
    /// </summary>
    public void PreviousPage()
    {
        if (pages == null || pages.Length == 0)
            return;

        // 最初のページなら閉じる
        if (currentPageIndex <= 0)
        {
            Close();
            return;
        }

        currentPageIndex--;

        ShowCurrentPage();
    }

    /// <summary>
    /// 右矢印ボタンから呼び出す
    /// </summary>
    public void NextPage()
    {
        if (pages == null || pages.Length == 0)
            return;

        // 最後のページなら閉じる
        if (currentPageIndex >= pages.Length - 1)
        {
            Close();
            return;
        }

        currentPageIndex++;

        ShowCurrentPage();
    }

    /// <summary>
    /// 現在のページだけを表示する
    /// </summary>
    private void ShowCurrentPage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(i == currentPageIndex);
        }

        UpdatePageNumber();
        UpdateArrowText();
    }

    /// <summary>
    /// ページ番号を更新する
    /// </summary>
    private void UpdatePageNumber()
    {
        if (pageNumberText == null)
            return;

        pageNumberText.text =
            (currentPageIndex + 1) + " / " + pages.Length;
    }

    /// <summary>
    /// 左右矢印の文字を更新する
    /// </summary>
    private void UpdateArrowText()
    {
        if (leftArrowText != null)
        {
            if (currentPageIndex == 0)
                leftArrowText.text = closeArrow;
            else
                leftArrowText.text = normalLeftArrow;
        }

        if (rightArrowText != null)
        {
            if (currentPageIndex == pages.Length - 1)
                rightArrowText.text = closeArrow;
            else
                rightArrowText.text = normalRightArrow;
        }
    }
}