using UnityEngine;

[System.Serializable]
public class SearchSiteIconEntry
{
    public SearchSiteType siteType;
    public Sprite icon;
}

public class SearchSiteIconDatabase : MonoBehaviour
{
    [SerializeField]
    private SearchSiteIconEntry[] siteIcons;

    public Sprite GetIcon(SearchSiteType siteType)
    {
        foreach (SearchSiteIconEntry entry in siteIcons)
        {
            if (entry.siteType == siteType)
            {
                return entry.icon;
            }
        }

        return null;
    }
}