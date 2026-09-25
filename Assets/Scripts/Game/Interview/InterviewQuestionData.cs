using System;
using UnityEngine;

[Serializable]
public class InterviewQuestionData
{
    [TextArea(1, 3)]
    public string question;

    [TextArea(1, 5)]
    public string answer;
}