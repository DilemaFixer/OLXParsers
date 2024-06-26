namespace OLXCategoryParser;

public static class StringArrExtension
{
    public static string ArrLineUp(this string[] lines)
    {
        string result = "";

        foreach (string line in lines)
        {
            result += line + " ";
        }

        return result;
    }
}