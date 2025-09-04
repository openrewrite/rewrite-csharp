namespace Second;

public class Bar
{
    void Test()
    {
        string s = "hello";
        s = s.ToLower(System.Globalization.CultureInfo.CurrentCulture);
    }
}