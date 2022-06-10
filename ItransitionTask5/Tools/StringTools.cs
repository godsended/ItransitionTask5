namespace ItransitionTask5.Tools;

public static class StringTools
{
    public static bool StringEmpty(params string[] strings)
    {
        foreach (var s in strings)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                return true;
        }
        return false;
    }
}