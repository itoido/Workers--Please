using System;
using UnityEngine;


[Serializable]
public class AIImageDetectionData
{
    [Header("AI生成確率")]
    [Range(0f, 100f)]
    public float aiProbability = 50f;


    [Header("判定結果メッセージ")]
    [TextArea(2, 4)]
    public string resultMessage =
        "Unable to determine.";
}