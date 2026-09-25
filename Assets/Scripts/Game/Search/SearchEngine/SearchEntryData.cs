using System;
using UnityEngine;

[Serializable]
public class SearchEntryData
{
    [Header("検索対象の種類")]
    public SearchTargetType targetType;
    
    [Header("この検索で表示されるサイト")]
    public SearchResultData[] results;
}