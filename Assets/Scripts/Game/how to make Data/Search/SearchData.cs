using UnityEngine;

[CreateAssetMenu(
    fileName = "ApplicantSearchData",
    menuName = "WorkersPlease/Applicant Search Data"
)]
public class ApplicantSearchData : ScriptableObject
{
    [Header("検索可能な情報")]
    public SearchEntryData[] searchEntries;
}