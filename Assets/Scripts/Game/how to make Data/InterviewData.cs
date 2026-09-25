using UnityEngine;

[CreateAssetMenu(
    fileName = "InterviewData",
    menuName = "WorkersPlease/Interview Data"
)]
public class InterviewData : ScriptableObject
{
    [Header("面談情報")]
    public string interviewDate;

    public string interviewerName;

    [Header("質問と回答")]
    public InterviewQuestionData[] questions;
}