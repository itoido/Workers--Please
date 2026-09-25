using UnityEngine;

public class EyeCatch : MonoBehaviour
{
    // =========================================================
    // 応募者管理
    // =========================================================

    [Header("応募者管理")]

    [SerializeField]
    private ApplicantManager applicantManager;


    // =========================================================
    // Windowシステム
    // =========================================================

    [Header("Windowシステム")]

    [Tooltip("各Windowのデフォルト配置・順番表示を管理するシステム")]
    [SerializeField]
    private WindowLayoutSystem windowLayoutSystem;

    [Header("SearchWindow")]
    [Tooltip("SearchWindow内部の状態を初期化するUI")]
    [SerializeField]
    private SearchWindowUI searchWindowUI;


    // =========================================================
    // Game Flow
    // =========================================================

    [Header("Game Flow")]

    [Tooltip("Start / Game / End画面を管理するシステム")]
    [SerializeField]
    private GameFlowManager gameFlowManager;


    // =========================================================
    // EyeCatch Animation
    // =========================================================

    [Header("EyeCatch Animation")]

    [Tooltip("実際のEyeCatchアニメーションを担当するコンポーネント")]
    [SerializeField]
    private DefaultEyeCatchAnimation eyeCatchAnimation;


    // =========================================================
    // メッセージ
    // =========================================================

    [Header("メッセージ")]

    [Tooltip("通常の応募者切り替え時に表示する文字")]
    [SerializeField]
    private string nextApplicantMessage =
        "NEXT, PLEASE!";

    [Tooltip("全応募者終了時に表示する文字")]
    [SerializeField]
    private string gameEndMessage =
        "IT'S OVER!!";


    // =========================================================
    // テスト
    // =========================================================

    [Header("テスト設定")]

    [Tooltip("OFFにすると応募者を変更せずアニメーションだけ確認できます")]
    [SerializeField]
    private bool changeApplicant = true;


    // =========================================================
    // 通常の応募者切り替え
    // =========================================================

    public void PlayNextApplicantTransition()
    {
        if (!CheckCommonReferences())
        {
            return;
        }


        if (applicantManager == null)
        {
            Debug.LogError(
                "EyeCatch: ApplicantManagerが設定されていません。"
            );

            return;
        }


        if (eyeCatchAnimation.IsPlaying)
        {
            return;
        }


        eyeCatchAnimation.Play(

            // =================================================
            // メッセージ
            // =================================================

            nextApplicantMessage,


            // =================================================
            // 画面が完全に覆われた瞬間
            // =================================================

            onCovered: () =>
            {
                // ---------------------------------
                // Applicant切り替え
                // ---------------------------------

                if (changeApplicant)
                {
                    applicantManager
                        .NextApplicant();
                }


                // ---------------------------------
                // SearchWindow内部を初期化
                // ---------------------------------

                if (searchWindowUI != null)
                {
                    searchWindowUI
                        .ResetSearchWindow();
                }


                // ---------------------------------
                // Window位置・サイズ・表示状態を初期化
                // ---------------------------------

                if (windowLayoutSystem != null)
                {
                    windowLayoutSystem
                        .PrepareWindowsForPopup();
                }
            },


            // =================================================
            // EyeCatch終了
            // =================================================

            onComplete: () =>
            {
                // ---------------------------------
                // Windowを順番に表示
                // ---------------------------------

                if (windowLayoutSystem != null)
                {
                    windowLayoutSystem
                        .ShowWindowsInOrder();
                }


                Debug.Log(
                    "EyeCatch終了 → Window表示開始"
                );
            }
        );
    }


    // =========================================================
    // ゲーム終了EyeCatch
    // =========================================================

    public void PlayGameEndTransition()
    {
        if (!CheckCommonReferences())
        {
            return;
        }


        if (gameFlowManager == null)
        {
            Debug.LogError(
                "EyeCatch: GameFlowManagerが設定されていません。"
            );

            return;
        }


        if (eyeCatchAnimation.IsPlaying)
        {
            return;
        }


        eyeCatchAnimation.Play(

            // =================================================
            // 終了メッセージ
            // =================================================

            gameEndMessage,


            // =================================================
            // 画面が完全に覆われた瞬間
            //
            // IT'S OVER!! が表示され始めるのと同時に
            // 裏側をGame → Endへ切り替える
            // =================================================

            onCovered: () =>
            {
                gameFlowManager
                    .PrepareEndScreen();


                Debug.Log(
                    "IT'S OVER!! 表示開始 → 裏側をEndPanelへ変更"
                );
            },


            // =================================================
            // EyeCatch終了
            // =================================================

            onComplete: () =>
            {
                // ---------------------------------
                // ここでは画面切替しない
                //
                // すでにEndPanelになっているため、
                // EyeCatchが消えるだけでよい
                // ---------------------------------

                Debug.Log(
                    "終了EyeCatch完了"
                );
            }
        );
    }


    // =========================================================
    // 共通Inspector確認
    // =========================================================

    private bool CheckCommonReferences()
    {
        if (eyeCatchAnimation == null)
        {
            Debug.LogError(
                "EyeCatch: DefaultEyeCatchAnimationが設定されていません。"
            );

            return false;
        }


        if (windowLayoutSystem == null)
        {
            Debug.LogWarning(
                "EyeCatch: WindowLayoutSystemが設定されていません。" +
                "通常EyeCatch後のWindow再配置・Popupは行われません。"
            );
        }


        return true;
    }
}