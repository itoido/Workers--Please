using UnityEngine;

public class InterviewWindowUI : MonoBehaviour
{
    [Header("質問表示先")]
    [SerializeField]
    private Transform interviewContent;

    [Header("質問Prefab")]
    [SerializeField]
    private InterviewItem interviewItemPrefab;

    public void ShowInterview(InterviewData interviewData)
    {
        ClearQuestions();

        if (interviewData == null)
        {
            Debug.LogWarning(
                "InterviewDataが設定されていません"
            );

            return;
        }

        if (interviewData.questions == null)
            return;

        foreach (
            InterviewQuestionData question
            in interviewData.questions
        )
        {
            InterviewItem item =
                Instantiate(
                    interviewItemPrefab,
                    interviewContent
                );

            item.Setup(question);
        }
    }

    private void ClearQuestions()
    {
        foreach (Transform child in interviewContent)
        {
            Destroy(child.gameObject);
        }
    }
}