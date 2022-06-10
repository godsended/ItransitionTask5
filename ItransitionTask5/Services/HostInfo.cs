namespace ItransitionTask5.Services;

public class HostInfo : IHostInfo
{
    private readonly string hostUrl = "ws://159.89.11.210/Home/";
    
    public string GetHostUrl()
    {
        return hostUrl;
    }
}