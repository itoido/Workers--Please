using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ConfigMenuUI : MonoBehaviour
{
    // =========================================================
    // Config全体
    // =========================================================

    [Header("Config全体")]

    [Tooltip("このConfigPopup自身を設定")]
    [SerializeField]
    private GameObject configPopupRoot;


    // =========================================================
    // Header
    // =========================================================

    [Header("Header")]

    [SerializeField]
    private GameObject backButton;

    [SerializeField]
    private TextMeshProUGUI headerText;


    // =========================================================
    // Page
    // =========================================================

    [Header("Page")]

    [SerializeField]
    private GameObject mainPage;

    [SerializeField]
    private GameObject bgmPage;

    [SerializeField]
    private GameObject themePage;


    // =========================================================
    // BGM Page
    // =========================================================

    [Header("BGM Page")]

    [SerializeField]
    private TextMeshProUGUI bgmNameText;

    [SerializeField]
    private TextMeshProUGUI playPauseText;

    [SerializeField]
    private ScrollingTextUI scrollingTextUI;


    // =========================================================
    // Volume
    // =========================================================

    [Header("Volume")]

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private TextMeshProUGUI volumeValueText;


    // =========================================================
    // BGM Manager
    // =========================================================

    [Header("BGM Manager")]

    [SerializeField]
    private BGMManager bgmManager;


    // =========================================================
    // Unity
    // =========================================================

    private void Start()
    {
        // -----------------------------------------------------
        // BGMManagerのイベント登録
        // -----------------------------------------------------

        if (bgmManager != null)
        {
            bgmManager.OnBGMStateChanged +=
                UpdateBGMDisplay;
        }


        // -----------------------------------------------------
        // Volume Slider
        // -----------------------------------------------------

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;

            volumeSlider.onValueChanged.AddListener(
                OnVolumeSliderChanged
            );
        }


        // -----------------------------------------------------
        // 初期ページ
        // -----------------------------------------------------

        ShowMainPage();

        UpdateBGMDisplay();


        // ★ここではSetActive(false)にしない
        //
        // Popupの初期表示状態はHierarchy / Inspector側、
        // またはStart画面管理側で決定する。
    }


    // =========================================================
    // Popupが有効になったとき
    // =========================================================

    private void OnEnable()
    {
        // -----------------------------------------------------
        // 一度Startが実行された後に再び開かれた場合、
        // MainPageから表示する
        // -----------------------------------------------------

        ShowMainPage();

        UpdateBGMDisplay();
    }


    // =========================================================
    // Destroy
    // =========================================================

    private void OnDestroy()
    {
        if (bgmManager != null)
        {
            bgmManager.OnBGMStateChanged -=
                UpdateBGMDisplay;
        }


        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(
                OnVolumeSliderChanged
            );
        }
    }


    // =========================================================
    // Configを開く
    // =========================================================

    public void OpenConfigMenu()
    {
        if (configPopupRoot == null)
        {
            return;
        }


        configPopupRoot.SetActive(true);

        configPopupRoot.transform
            .SetAsLastSibling();


        ShowMainPage();

        UpdateBGMDisplay();
    }


    // =========================================================
    // Config開閉
    // =========================================================

    public void ToggleConfigMenu()
    {
        if (configPopupRoot == null)
        {
            return;
        }


        bool nextState =
            !configPopupRoot.activeSelf;


        configPopupRoot.SetActive(
            nextState
        );


        if (nextState)
        {
            configPopupRoot.transform
                .SetAsLastSibling();

            ShowMainPage();

            UpdateBGMDisplay();
        }
    }


    // =========================================================
    // Configを閉じる
    // =========================================================

    public void CloseConfigMenu()
    {
        if (configPopupRoot != null)
        {
            configPopupRoot.SetActive(false);
        }
    }


    // =========================================================
    // Setting
    // =========================================================

    public void ShowMainPage()
    {
        if (mainPage != null)
        {
            mainPage.SetActive(true);
        }


        if (bgmPage != null)
        {
            bgmPage.SetActive(false);
        }


        if (themePage != null)
        {
            themePage.SetActive(false);
        }


        if (backButton != null)
        {
            backButton.SetActive(false);
        }


        if (headerText != null)
        {
            headerText.text = "Setting";
        }
    }


    // =========================================================
    // BGM
    // =========================================================

    public void ShowBGMPage()
    {
        if (mainPage != null)
        {
            mainPage.SetActive(false);
        }


        if (bgmPage != null)
        {
            bgmPage.SetActive(true);
        }


        if (themePage != null)
        {
            themePage.SetActive(false);
        }


        if (backButton != null)
        {
            backButton.SetActive(true);
        }


        if (headerText != null)
        {
            headerText.text = "BGM";
        }


        UpdateBGMDisplay();
    }


    // =========================================================
    // Theme
    // =========================================================

    public void ShowThemePage()
    {
        if (mainPage != null)
        {
            mainPage.SetActive(false);
        }


        if (bgmPage != null)
        {
            bgmPage.SetActive(false);
        }


        if (themePage != null)
        {
            themePage.SetActive(true);
        }


        if (backButton != null)
        {
            backButton.SetActive(true);
        }


        if (headerText != null)
        {
            headerText.text = "Theme";
        }
    }


    // =========================================================
    // 戻る
    // =========================================================

    public void BackToMainPage()
    {
        ShowMainPage();
    }


    // =========================================================
    // 前の曲
    // =========================================================

    public void PreviousBGM()
    {
        if (bgmManager != null)
        {
            bgmManager.PreviousTrack();
        }
    }


    // =========================================================
    // 再生 / 一時停止
    // =========================================================

    public void ToggleBGMPlayPause()
    {
        if (bgmManager != null)
        {
            bgmManager.TogglePlayPause();
        }
    }


    // =========================================================
    // 次の曲
    // =========================================================

    public void NextBGM()
    {
        if (bgmManager != null)
        {
            bgmManager.NextTrack();
        }
    }


    // =========================================================
    // Sliderから音量変更
    // =========================================================

    private void OnVolumeSliderChanged(
        float value
    )
    {
        if (bgmManager != null)
        {
            bgmManager.SetVolume(
                value
            );
        }
    }


    // =========================================================
    // BGM画面更新
    // =========================================================

    private void UpdateBGMDisplay()
    {
        if (bgmManager == null)
        {
            return;
        }


        // -----------------------------------------------------
        // 曲名
        // -----------------------------------------------------

        if (bgmNameText != null)
        {
            bgmNameText.text =
                bgmManager.CurrentTrackName;


            if (scrollingTextUI != null)
            {
                scrollingTextUI.Refresh();
            }
        }


        // -----------------------------------------------------
        // 再生 / 一時停止
        // -----------------------------------------------------

        if (playPauseText != null)
        {
            if (bgmManager.IsPlaying)
            {
                playPauseText.text = ";";
            }
            else
            {
                playPauseText.text = "4";
            }
        }


        // -----------------------------------------------------
        // Volume Slider
        // -----------------------------------------------------

        if (volumeSlider != null)
        {
            volumeSlider.SetValueWithoutNotify(
                bgmManager.CurrentVolume
            );
        }


        // -----------------------------------------------------
        // Volume %
        // -----------------------------------------------------

        if (volumeValueText != null)
        {
            int percent =
                Mathf.RoundToInt(
                    bgmManager.CurrentVolume *
                    100f
                );


            volumeValueText.text =
                percent + "%";
        }
    }
}