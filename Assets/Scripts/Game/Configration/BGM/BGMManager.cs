using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class BGMTrack
{
    [Header("画面に表示する曲名")]
    public string displayName;

    [Header("実際に再生する音源")]
    public AudioClip audioClip;
}


public class BGMManager : MonoBehaviour
{
    // =========================================================
    // BGM再生用
    // =========================================================

    [Header("BGM再生用")]

    [SerializeField]
    private AudioSource audioSource;


    // =========================================================
    // BGM一覧
    // =========================================================

    [Header("BGM一覧")]

    [SerializeField]
    private BGMTrack[] bgmTracks;


    // =========================================================
    // 初期Track
    // =========================================================

    [Header("ゲーム開始時に選択する曲")]

    [SerializeField]
    private int startTrackIndex = 0;


    // =========================================================
    // 初期音量
    // =========================================================

    [Header("ゲーム開始時の音量")]

    [Range(0f, 1f)]
    [SerializeField]
    private float startVolume = 0.5f;


    // =========================================================
    // 再生開始Delay
    // =========================================================

    [Header("BGM開始Delay")]

    [Tooltip("ゲーム起動後、最初のBGMを流し始めるまでの時間")]
    [SerializeField]
    private float startDelay = 1.0f;


    // =========================================================
    // 現在選択中Track
    // =========================================================

    private int currentTrackIndex = 0;


    // =========================================================
    // Event
    // =========================================================

    public event Action OnBGMStateChanged;


    // =========================================================
    // 現在のTrack番号
    // =========================================================

    public int CurrentTrackIndex
    {
        get
        {
            return currentTrackIndex;
        }
    }


    // =========================================================
    // Track数
    // =========================================================

    public int TrackCount
    {
        get
        {
            if (bgmTracks == null)
            {
                return 0;
            }

            return bgmTracks.Length;
        }
    }


    // =========================================================
    // 現在再生中か
    // =========================================================

    public bool IsPlaying
    {
        get
        {
            return audioSource != null &&
                   audioSource.isPlaying;
        }
    }


    // =========================================================
    // 現在の曲名
    // =========================================================

    public string CurrentTrackName
    {
        get
        {
            if (bgmTracks == null ||
                bgmTracks.Length == 0)
            {
                return "No BGM";
            }


            if (currentTrackIndex < 0 ||
                currentTrackIndex >= bgmTracks.Length)
            {
                return "No BGM";
            }


            BGMTrack track =
                bgmTracks[currentTrackIndex];


            if (track == null)
            {
                return "No BGM";
            }


            return track.displayName;
        }
    }


    // =========================================================
    // 現在の音量
    // =========================================================

    public float CurrentVolume
    {
        get
        {
            if (audioSource == null)
            {
                return 0f;
            }


            return audioSource.volume;
        }
    }


    // =========================================================
    // Unity Start
    // =========================================================

    private void Start()
    {
        // =====================================================
        // AudioSource確認
        // =====================================================

        if (audioSource == null)
        {
            Debug.LogError(
                "BGMManager: AudioSourceが設定されていません。"
            );

            return;
        }


        // =====================================================
        // BGM確認
        // =====================================================

        if (bgmTracks == null ||
            bgmTracks.Length == 0)
        {
            Debug.LogWarning(
                "BGMManager: BGMが1曲も設定されていません。"
            );

            return;
        }


        // =====================================================
        // 初期Track番号
        // =====================================================

        currentTrackIndex =
            Mathf.Clamp(
                startTrackIndex,
                0,
                bgmTracks.Length - 1
            );


        // =====================================================
        // 初期音量
        // =====================================================

        audioSource.volume =
            Mathf.Clamp01(
                startVolume
            );


        // =====================================================
        // Loop
        // =====================================================

        audioSource.loop = true;


        // =====================================================
        // AudioSource側のPlay On Awakeは使用しない
        // =====================================================

        audioSource.Stop();


        // =====================================================
        // UIへ初期状態通知
        // =====================================================

        OnBGMStateChanged?.Invoke();


        // =====================================================
        // 約1秒後に再生
        // =====================================================

        StartCoroutine(
            StartBGMWithDelay()
        );
    }


    // =========================================================
    // 最初のBGM再生
    // =========================================================

    private IEnumerator StartBGMWithDelay()
    {
        if (startDelay > 0f)
        {
            yield return new WaitForSecondsRealtime(
                startDelay
            );
        }


        PlayCurrentTrack();
    }


    // =========================================================
    // Track番号を直接指定
    // =========================================================

    public void SelectTrack(
        int index
    )
    {
        if (bgmTracks == null ||
            bgmTracks.Length == 0)
        {
            return;
        }


        int newIndex =
            Mathf.Clamp(
                index,
                0,
                bgmTracks.Length - 1
            );


        currentTrackIndex =
            newIndex;


        PlayCurrentTrack();
    }


    // =========================================================
    // 現在の曲を再生
    // =========================================================

    private void PlayCurrentTrack()
    {
        if (audioSource == null)
        {
            return;
        }


        if (bgmTracks == null ||
            bgmTracks.Length == 0)
        {
            return;
        }


        if (currentTrackIndex < 0 ||
            currentTrackIndex >= bgmTracks.Length)
        {
            return;
        }


        BGMTrack track =
            bgmTracks[currentTrackIndex];


        if (track == null ||
            track.audioClip == null)
        {
            Debug.LogWarning(
                "BGMが設定されていません。Index: "
                + currentTrackIndex
            );

            return;
        }


        audioSource.Stop();


        audioSource.clip =
            track.audioClip;


        audioSource.loop =
            true;


        audioSource.Play();


        OnBGMStateChanged?.Invoke();
    }


    // =========================================================
    // 前の曲
    // =========================================================

    public void PreviousTrack()
    {
        if (bgmTracks == null ||
            bgmTracks.Length == 0)
        {
            return;
        }


        currentTrackIndex--;


        if (currentTrackIndex < 0)
        {
            currentTrackIndex =
                bgmTracks.Length - 1;
        }


        PlayCurrentTrack();
    }


    // =========================================================
    // 次の曲
    // =========================================================

    public void NextTrack()
    {
        if (bgmTracks == null ||
            bgmTracks.Length == 0)
        {
            return;
        }


        currentTrackIndex++;


        if (currentTrackIndex >=
            bgmTracks.Length)
        {
            currentTrackIndex = 0;
        }


        PlayCurrentTrack();
    }


    // =========================================================
    // 一時停止 / 再生
    // =========================================================

    public void TogglePlayPause()
    {
        if (audioSource == null ||
            audioSource.clip == null)
        {
            return;
        }


        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }


        OnBGMStateChanged?.Invoke();
    }


    // =========================================================
    // 音量変更
    // =========================================================

    public void SetVolume(
        float volume
    )
    {
        if (audioSource == null)
        {
            return;
        }


        audioSource.volume =
            Mathf.Clamp01(
                volume
            );


        OnBGMStateChanged?.Invoke();
    }
}