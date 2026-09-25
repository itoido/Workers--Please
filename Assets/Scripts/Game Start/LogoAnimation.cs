using UnityEngine;
using DG.Tweening;

public class StartLogoAnimation : MonoBehaviour
{
    // =========================================================
    // 基本
    // =========================================================

    [Header("基本設定")]
    [Tooltip("アニメーションさせるロゴ本体。未設定ならこのObject自身を使用します。")]
    [SerializeField] private RectTransform logoRect;


    // =========================================================
    // 登場アニメーション
    // =========================================================

    [Header("登場アニメーション")]
    [SerializeField] private bool useIntroAnimation = true;

    [Tooltip("登場アニメーション全体の時間")]
    [SerializeField] private float introDuration = 0.55f;

    [Tooltip("登場時に一度大きくなる倍率")]
    [SerializeField] private float introOvershootScale = 1.15f;

    [Tooltip("大きくなった後、一度小さくなる倍率")]
    [SerializeField] private float introUndershootScale = 0.95f;


    // =========================================================
    // 呼吸
    // =========================================================

    [Header("Idle - 呼吸")]
    [SerializeField] private bool useBreathing = true;

    [Tooltip("通常サイズからどれだけ大きくするか")]
    [SerializeField] private float breathingScaleAmount = 0.025f;

    [Tooltip("大きくなるまでの時間")]
    [SerializeField] private float breathingHalfDuration = 1.25f;


    // =========================================================
    // 浮遊
    // =========================================================

    [Header("Idle - 上下浮遊")]
    [SerializeField] private bool useFloating = false;

    [Tooltip("上下に移動する距離")]
    [SerializeField] private float floatingDistance = 5f;

    [Tooltip("上から下までの片道時間")]
    [SerializeField] private float floatingHalfDuration = 1.5f;


    // =========================================================
    // 傾き
    // =========================================================

    [Header("Idle - ゆっくり傾く")]
    [SerializeField] private bool useRotation = false;

    [Tooltip("左右へ傾く角度")]
    [SerializeField] private float rotationAngle = 1.5f;

    [Tooltip("片側まで傾く時間")]
    [SerializeField] private float rotationHalfDuration = 1.5f;


    // =========================================================
    // グリッチ
    // =========================================================

    [Header("Accent - グリッチ")]
    [SerializeField] private bool useGlitch = true;

    [Tooltip("次のグリッチまでの最短時間")]
    [SerializeField] private float glitchIntervalMin = 4f;

    [Tooltip("次のグリッチまでの最長時間")]
    [SerializeField] private float glitchIntervalMax = 7f;

    [Tooltip("左右へ瞬間移動する距離")]
    [SerializeField] private float glitchDistance = 5f;

    [Tooltip("グリッチ1回の速さ")]
    [SerializeField] private float glitchStepDuration = 0.035f;

    [Tooltip("グリッチ時に少し傾ける")]
    [SerializeField] private float glitchRotation = 1.5f;


    // =========================================================
    // ピクッ
    // =========================================================

    [Header("Accent - ピクッ")]
    [SerializeField] private bool useTwitch = false;

    [Tooltip("次のピクッまでの最短時間")]
    [SerializeField] private float twitchIntervalMin = 3f;

    [Tooltip("次のピクッまでの最長時間")]
    [SerializeField] private float twitchIntervalMax = 6f;

    [Tooltip("一瞬大きくなる倍率")]
    [SerializeField] private float twitchBigScale = 1.06f;

    [Tooltip("その後一瞬小さくなる倍率")]
    [SerializeField] private float twitchSmallScale = 0.98f;

    [Tooltip("1段階の時間")]
    [SerializeField] private float twitchStepDuration = 0.07f;


    // =========================================================
    // Runtime
    // =========================================================

    private Vector2 basePosition;
    private Vector3 baseScale;
    private Vector3 baseRotation;

    private Tween breathingTween;
    private Tween floatingTween;
    private Tween rotationTween;

    private Sequence introSequence;
    private Sequence glitchSequence;
    private Sequence twitchSequence;

    private Tween glitchTimer;
    private Tween twitchTimer;

    private bool initialized;


    // =========================================================
    // Awake
    // =========================================================

    private void Awake()
    {
        if (logoRect == null)
        {
            logoRect = GetComponent<RectTransform>();
        }

        if (logoRect == null)
        {
            Debug.LogError(
                "StartLogoAnimation: RectTransformが見つかりません。"
            );

            enabled = false;
            return;
        }

        SaveBaseTransform();

        initialized = true;
    }


    // =========================================================
    // Enable
    // =========================================================

    private void OnEnable()
    {
        if (!initialized)
        {
            return;
        }

        StopAllAnimations();

        ResetTransform();

        if (useIntroAnimation)
        {
            PlayIntro();
        }
        else
        {
            StartIdleAnimations();
            StartAccentAnimations();
        }
    }


    // =========================================================
    // Disable
    // =========================================================

    private void OnDisable()
    {
        StopAllAnimations();

        if (initialized)
        {
            ResetTransform();
        }
    }


    // =========================================================
    // 初期状態保存
    // =========================================================

    private void SaveBaseTransform()
    {
        basePosition = logoRect.anchoredPosition;
        baseScale = logoRect.localScale;
        baseRotation = logoRect.localEulerAngles;
    }


    // =========================================================
    // 初期状態へ戻す
    // =========================================================

    private void ResetTransform()
    {
        logoRect.anchoredPosition = basePosition;
        logoRect.localScale = baseScale;
        logoRect.localEulerAngles = baseRotation;
    }


    // =========================================================
    // 登場
    // =========================================================

    private void PlayIntro()
    {
        logoRect.localScale = Vector3.zero;

        float firstDuration = introDuration * 0.55f;
        float secondDuration = introDuration * 0.25f;
        float thirdDuration = introDuration * 0.20f;

        Vector3 overshootScale =
            baseScale * introOvershootScale;

        Vector3 undershootScale =
            baseScale * introUndershootScale;

        introSequence = DOTween.Sequence();

        introSequence
            .Append(
                logoRect.DOScale(
                    overshootScale,
                    firstDuration
                )
                .SetEase(Ease.OutCubic)
            )
            .Append(
                logoRect.DOScale(
                    undershootScale,
                    secondDuration
                )
                .SetEase(Ease.InOutSine)
            )
            .Append(
                logoRect.DOScale(
                    baseScale,
                    thirdDuration
                )
                .SetEase(Ease.OutSine)
            )
            .OnComplete(() =>
            {
                introSequence = null;

                StartIdleAnimations();
                StartAccentAnimations();
            });
    }


    // =========================================================
    // Idle開始
    // =========================================================

    private void StartIdleAnimations()
    {
        if (useBreathing)
        {
            StartBreathing();
        }

        if (useFloating)
        {
            StartFloating();
        }

        if (useRotation)
        {
            StartRotation();
        }
    }


    // =========================================================
    // 呼吸
    // =========================================================

    private void StartBreathing()
    {
        Vector3 targetScale =
            baseScale * (1f + breathingScaleAmount);

        breathingTween =
            logoRect.DOScale(
                targetScale,
                breathingHalfDuration
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(
                -1,
                LoopType.Yoyo
            );
    }


    // =========================================================
    // 上下浮遊
    // =========================================================

    private void StartFloating()
    {
        Vector2 targetPosition =
            basePosition +
            Vector2.up * floatingDistance;

        floatingTween =
            logoRect.DOAnchorPos(
                targetPosition,
                floatingHalfDuration
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(
                -1,
                LoopType.Yoyo
            );
    }


    // =========================================================
    // ゆっくり傾く
    // =========================================================

    private void StartRotation()
    {
        Vector3 targetRotation =
            baseRotation +
            new Vector3(
                0f,
                0f,
                rotationAngle
            );

        logoRect.localEulerAngles =
            baseRotation +
            new Vector3(
                0f,
                0f,
                -rotationAngle
            );

        rotationTween =
            logoRect.DOLocalRotate(
                targetRotation,
                rotationHalfDuration * 2f,
                RotateMode.Fast
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(
                -1,
                LoopType.Yoyo
            );
    }


    // =========================================================
    // Accent開始
    // =========================================================

    private void StartAccentAnimations()
    {
        if (useGlitch)
        {
            ScheduleNextGlitch();
        }

        if (useTwitch)
        {
            ScheduleNextTwitch();
        }
    }


    // =========================================================
    // 次のGlitchを予約
    // =========================================================

    private void ScheduleNextGlitch()
    {
        if (!useGlitch)
        {
            return;
        }

        float waitTime =
            Random.Range(
                glitchIntervalMin,
                glitchIntervalMax
            );

        glitchTimer =
            DOVirtual.DelayedCall(
                waitTime,
                PlayGlitch
            );
    }


    // =========================================================
    // Glitch
    // =========================================================

    private void PlayGlitch()
    {
        glitchTimer = null;

        Vector2 currentPosition =
            logoRect.anchoredPosition;

        Vector3 currentRotation =
            logoRect.localEulerAngles;

        glitchSequence = DOTween.Sequence();

        glitchSequence
            .Append(
                logoRect.DOAnchorPosX(
                    currentPosition.x - glitchDistance,
                    glitchStepDuration
                )
                .SetEase(Ease.Linear)
            )
            .Join(
                logoRect.DOLocalRotate(
                    currentRotation +
                    new Vector3(
                        0f,
                        0f,
                        -glitchRotation
                    ),
                    glitchStepDuration
                )
                .SetEase(Ease.Linear)
            )

            .Append(
                logoRect.DOAnchorPosX(
                    currentPosition.x + glitchDistance,
                    glitchStepDuration
                )
                .SetEase(Ease.Linear)
            )
            .Join(
                logoRect.DOLocalRotate(
                    currentRotation +
                    new Vector3(
                        0f,
                        0f,
                        glitchRotation
                    ),
                    glitchStepDuration
                )
                .SetEase(Ease.Linear)
            )

            .Append(
                logoRect.DOAnchorPosX(
                    currentPosition.x - glitchDistance * 0.5f,
                    glitchStepDuration
                )
                .SetEase(Ease.Linear)
            )

            .Append(
                logoRect.DOAnchorPosX(
                    currentPosition.x,
                    glitchStepDuration
                )
                .SetEase(Ease.Linear)
            )
            .Join(
                logoRect.DOLocalRotate(
                    currentRotation,
                    glitchStepDuration
                )
                .SetEase(Ease.Linear)
            )

            .OnComplete(() =>
            {
                glitchSequence = null;

                ScheduleNextGlitch();
            });
    }


    // =========================================================
    // 次のTwitchを予約
    // =========================================================

    private void ScheduleNextTwitch()
    {
        if (!useTwitch)
        {
            return;
        }

        float waitTime =
            Random.Range(
                twitchIntervalMin,
                twitchIntervalMax
            );

        twitchTimer =
            DOVirtual.DelayedCall(
                waitTime,
                PlayTwitch
            );
    }


    // =========================================================
    // Twitch
    // =========================================================

    private void PlayTwitch()
    {
        twitchTimer = null;

        Vector3 currentScale =
            logoRect.localScale;

        Vector3 bigScale =
            currentScale * twitchBigScale;

        Vector3 smallScale =
            currentScale * twitchSmallScale;

        twitchSequence = DOTween.Sequence();

        twitchSequence
            .Append(
                logoRect.DOScale(
                    bigScale,
                    twitchStepDuration
                )
                .SetEase(Ease.OutQuad)
            )

            .Append(
                logoRect.DOScale(
                    smallScale,
                    twitchStepDuration
                )
                .SetEase(Ease.InOutQuad)
            )

            .Append(
                logoRect.DOScale(
                    currentScale,
                    twitchStepDuration
                )
                .SetEase(Ease.OutQuad)
            )

            .OnComplete(() =>
            {
                twitchSequence = null;

                ScheduleNextTwitch();
            });
    }


    // =========================================================
    // 全アニメーション停止
    // =========================================================

    private void StopAllAnimations()
    {
        if (introSequence != null)
        {
            introSequence.Kill();
            introSequence = null;
        }

        if (breathingTween != null)
        {
            breathingTween.Kill();
            breathingTween = null;
        }

        if (floatingTween != null)
        {
            floatingTween.Kill();
            floatingTween = null;
        }

        if (rotationTween != null)
        {
            rotationTween.Kill();
            rotationTween = null;
        }

        if (glitchSequence != null)
        {
            glitchSequence.Kill();
            glitchSequence = null;
        }

        if (twitchSequence != null)
        {
            twitchSequence.Kill();
            twitchSequence = null;
        }

        if (glitchTimer != null)
        {
            glitchTimer.Kill();
            glitchTimer = null;
        }

        if (twitchTimer != null)
        {
            twitchTimer.Kill();
            twitchTimer = null;
        }
    }


    // =========================================================
    // Destroy
    // =========================================================

    private void OnDestroy()
    {
        StopAllAnimations();
    }
}