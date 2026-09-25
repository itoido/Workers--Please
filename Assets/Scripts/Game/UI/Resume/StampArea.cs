using UnityEngine;
using DG.Tweening;

public class StampArea : MonoBehaviour
{
    // =========================================================
    // Stampシステム
    // =========================================================

    [Header("Stampシステム")]

    [SerializeField]
    private StampMenuUI stampMenuUI;


    // =========================================================
    // Applicant
    // =========================================================

    [Header("Applicant")]

    [Tooltip("現在の応募者と最後の応募者判定に使用")]
    [SerializeField]
    private ApplicantManager applicantManager;


    // =========================================================
    // Result
    // =========================================================

    [Header("Result")]

    [Tooltip("応募者ごとの判定結果を記録")]
    [SerializeField]
    private GameResultManager gameResultManager;


    // =========================================================
    // スタンプ画像
    // =========================================================

    [Header("スタンプ画像")]

    [SerializeField]
    private RectTransform approvedStamp;

    [SerializeField]
    private RectTransform rejectedStamp;


    // =========================================================
    // スタンプSE
    // =========================================================

    [Header("スタンプSE")]

    [Tooltip("スタンプSEを再生するAudioSource")]
    [SerializeField]
    private AudioSource stampAudioSource;

    [Tooltip("スタンプを押した瞬間に再生するSE")]
    [SerializeField]
    private AudioClip stampSound;


    // =========================================================
    // EyeCatch
    // =========================================================

    [Header("EyeCatch")]

    [SerializeField]
    private EyeCatch eyeCatch;


    // =========================================================
    // 演出設定
    // =========================================================

    [Header("スタンプ演出")]

    [Tooltip("表示開始時の大きさ")]
    [SerializeField]
    private float startScale = 1.5f;

    [Tooltip("押し込んだ瞬間の大きさ")]
    [SerializeField]
    private float impactScale = 0.9f;

    [Tooltip("最終サイズ")]
    [SerializeField]
    private float finalScale = 1.0f;

    [Tooltip("押し込む時間")]
    [SerializeField]
    private float stampDownDuration = 0.12f;

    [Tooltip("戻る時間")]
    [SerializeField]
    private float settleDuration = 0.10f;

    [Tooltip("押印後、EyeCatch開始まで待つ時間")]
    [SerializeField]
    private float waitBeforeEyeCatch = 0.6f;

    [Tooltip("EyeCatch開始後、Stampを消すまでの時間")]
    [SerializeField]
    private float resetDelayAfterEyeCatch = 0.7f;


    // =========================================================
    // 内部状態
    // =========================================================

    private bool isProcessing = false;

    private bool hasStamped = false;

    private Sequence stampSequence;


    // =========================================================
    // 初期化
    // =========================================================

    private void Awake()
    {
        ResetStampArea();
    }


    // =========================================================
    // StampAreaクリック
    // =========================================================

    public void OnStampAreaClicked()
    {
        Debug.Log(
            "StampAreaがクリックされました"
        );


        if (isProcessing)
        {
            Debug.Log(
                "現在スタンプ処理中です"
            );

            return;
        }


        if (hasStamped)
        {
            Debug.Log(
                "すでに押印済みです"
            );

            return;
        }


        if (stampMenuUI == null)
        {
            Debug.LogError(
                "StampArea: StampMenuUIが設定されていません"
            );

            return;
        }


        if (applicantManager == null)
        {
            Debug.LogError(
                "StampArea: ApplicantManagerが設定されていません"
            );

            return;
        }


        StampMenuUI.StampType selectedStamp =
            stampMenuUI.SelectedStamp;


        Debug.Log(
            "現在選択中のStamp = " +
            selectedStamp
        );


        if (selectedStamp ==
            StampMenuUI.StampType.None)
        {
            Debug.LogWarning(
                "StampArea: スタンプが選択されていません"
            );

            return;
        }


        isProcessing = true;

        hasStamped = true;


        // =====================================================
        // 判定結果を保存
        // =====================================================

        if (gameResultManager != null)
        {
            gameResultManager.RecordResult(
                applicantManager.CurrentApplicant,
                selectedStamp
            );
        }
        else
        {
            Debug.LogWarning(
                "StampArea: GameResultManagerが設定されていません"
            );
        }


        // =====================================================
        // Stamp選択解除
        // =====================================================

        stampMenuUI.ClearStampSelection();


        // =====================================================
        // 採用
        // =====================================================

        if (selectedStamp ==
            StampMenuUI.StampType.Approve)
        {
            PlayStampAnimation(
                approvedStamp
            );

            return;
        }


        // =====================================================
        // 非採用
        // =====================================================

        if (selectedStamp ==
            StampMenuUI.StampType.Reject)
        {
            PlayStampAnimation(
                rejectedStamp
            );

            return;
        }


        isProcessing = false;

        hasStamped = false;
    }


    // =========================================================
    // Stamp Animation
    // =========================================================

    private void PlayStampAnimation(
        RectTransform stamp
    )
    {
        if (stamp == null)
        {
            Debug.LogError(
                "StampArea: 表示するStamp画像が設定されていません"
            );

            isProcessing = false;

            hasStamped = false;

            return;
        }


        if (stampSequence != null &&
            stampSequence.IsActive())
        {
            stampSequence.Kill();
        }


        stamp.DOKill();


        if (approvedStamp != null &&
            stamp != approvedStamp)
        {
            approvedStamp
                .gameObject
                .SetActive(false);
        }


        if (rejectedStamp != null &&
            stamp != rejectedStamp)
        {
            rejectedStamp
                .gameObject
                .SetActive(false);
        }


        stamp.gameObject.SetActive(
            true
        );


        stamp.SetAsLastSibling();


        stamp.localScale =
            Vector3.one *
            startScale;


        stampSequence =
            DOTween.Sequence();


        stampSequence.Append(
            stamp
                .DOScale(
                    impactScale,
                    stampDownDuration
                )
                .SetEase(
                    Ease.InQuad
                )
        );


        stampSequence.AppendCallback(
            () =>
            {
                PlayStampSound();
            }
        );


        stampSequence.Append(
            stamp
                .DOScale(
                    finalScale,
                    settleDuration
                )
                .SetEase(
                    Ease.OutBack
                )
        );


        stampSequence.AppendInterval(
            waitBeforeEyeCatch
        );


        stampSequence.AppendCallback(
            () =>
            {
                if (eyeCatch == null)
                {
                    Debug.LogWarning(
                        "StampArea: EyeCatchが設定されていません"
                    );

                    return;
                }


                if (applicantManager.IsLastApplicant)
                {
                    eyeCatch
                        .PlayGameEndTransition();
                }
                else
                {
                    eyeCatch
                        .PlayNextApplicantTransition();
                }
            }
        );


        stampSequence.AppendInterval(
            resetDelayAfterEyeCatch
        );


        stampSequence.AppendCallback(
            () =>
            {
                ResetStampArea();
            }
        );


        stampSequence.OnComplete(
            () =>
            {
                isProcessing = false;
            }
        );
    }


    // =========================================================
    // Stamp SE
    // =========================================================

    private void PlayStampSound()
    {
        if (stampAudioSource == null)
        {
            Debug.LogWarning(
                "StampArea: Stamp Audio Sourceが設定されていません"
            );

            return;
        }


        if (stampSound == null)
        {
            Debug.LogWarning(
                "StampArea: Stamp Soundが設定されていません"
            );

            return;
        }


        stampAudioSource.PlayOneShot(
            stampSound
        );
    }


    // =========================================================
    // StampArea初期化
    // =========================================================

    public void ResetStampArea()
    {
        hasStamped = false;


        if (approvedStamp != null)
        {
            approvedStamp.DOKill();

            approvedStamp.localScale =
                Vector3.one;

            approvedStamp
                .gameObject
                .SetActive(false);
        }


        if (rejectedStamp != null)
        {
            rejectedStamp.DOKill();

            rejectedStamp.localScale =
                Vector3.one;

            rejectedStamp
                .gameObject
                .SetActive(false);
        }
    }


    // =========================================================
    // Destroy
    // =========================================================

    private void OnDestroy()
    {
        if (stampSequence != null)
        {
            stampSequence.Kill();
        }


        if (approvedStamp != null)
        {
            approvedStamp.DOKill();
        }


        if (rejectedStamp != null)
        {
            rejectedStamp.DOKill();
        }
    }
}