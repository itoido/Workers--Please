using TMPro;
using UnityEngine;

public class IPLogRow : MonoBehaviour
{
    [Header("Log Cells")]
    [SerializeField] private TMP_Text timestampText;
    [SerializeField] private TMP_Text requestIDText;
    [SerializeField] private TMP_Text applicationIDText;
    [SerializeField] private TMP_Text clientIPText;

    [Header("IP Region Estimate")]
    [SerializeField] private TMP_Text countryText;
    [SerializeField] private TMP_Text prefectureText;
    [SerializeField] private TMP_Text cityText;

    [Header("Browser")]
    [SerializeField] private TMP_Text timezoneText;
    [SerializeField] private TMP_Text languageText;
    [SerializeField] private TMP_Text userAgentText;

    [Header("Request")]
    [SerializeField] private TMP_Text methodText;
    [SerializeField] private TMP_Text pathText;

    public void Setup(IPLogEntry entry)
    {
        if (entry == null)
            return;

        timestampText.text = Safe(entry.timestamp);
        requestIDText.text = Safe(entry.request_id);
        applicationIDText.text = Safe(entry.application_id);
        clientIPText.text = Safe(entry.client_ip);

        if (entry.ip_region_estimate != null)
        {
            countryText.text = Safe(entry.ip_region_estimate.country);
            prefectureText.text = Safe(entry.ip_region_estimate.prefecture);
            cityText.text = Safe(entry.ip_region_estimate.city);
        }
        else
        {
            countryText.text = "-";
            prefectureText.text = "-";
            cityText.text = "-";
        }

        timezoneText.text = Safe(entry.browser_timezone);
        languageText.text = Safe(entry.browser_language);
        userAgentText.text = Safe(entry.user_agent);

        methodText.text = Safe(entry.method);
        pathText.text = Safe(entry.path);
    }

    private string Safe(string value)
    {
        return string.IsNullOrEmpty(value) ? "-" : value;
    }
}