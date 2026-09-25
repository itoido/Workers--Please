using UnityEngine;
using TMPro;

public class InterviewItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI answerText;

    public void Setup(InterviewQuestionData data)
    {
        questionText.text = "Q. " + data.question;
        answerText.text = "A. " + data.answer;
    }
}