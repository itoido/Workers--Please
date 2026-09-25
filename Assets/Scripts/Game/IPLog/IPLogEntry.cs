using System;

[Serializable]
public class IPLogEntry
{
    public string timestamp;
    public string request_id;
    public string application_id;
    public string client_ip;

    public IPRegionEstimate ip_region_estimate;

    public string browser_timezone;
    public string browser_language;
    public string user_agent;
    public string method;
    public string path;
}

[Serializable]
public class IPRegionEstimate
{
    public string country;
    public string prefecture;
    public string city;
}