using System;
using UnityEngine;

[Serializable]
public class SearchResultData
{
    [Header("サイト種類")]
    public SearchSiteType siteType;

    [Header("検索結果タイトル")]
    public string title;

    [Header("URL風表示")]
    public string displayUrl;

    [Header("検索結果の概要")]
    [TextArea(2, 5)]
    public string summary;

    [Header("サイトページデータ")]
    public SitePageData pageData;
}