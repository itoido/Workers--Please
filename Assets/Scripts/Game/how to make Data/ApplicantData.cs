using UnityEngine;

[CreateAssetMenu(
    fileName = "ApplicantData",
    menuName = "WorkersPlease/Applicant Data"
)]

public class ApplicantData : ScriptableObject
{
    [Header("基本情報")]
    public Sprite FacialImage;
    public string applicantName;
    public string email;
    public string location;


    [Header("経歴")]
    [TextArea(5, 15)]
    public string Background;


    [Header("補足")]
    [TextArea(5, 15)]
    public string supplement;


    [Header("面談履歴")]
    public InterviewData interviewData;


    [Header("IP Log")]
    public TextAsset ipLogFile;


    [Header("検索データ")]
    public ApplicantSearchData searchData;


    [Header("AI画像判定")]
    public AIImageDetectionData aiImageDetectionData;


    [Header("正解")]
    public bool isFraud;


    [Header("エンディング解説")]
    [TextArea(3, 10)]
    public string fraudExplanation;
}