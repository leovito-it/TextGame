public class MyString
{
    static public bool Compare(string a, string b)
    {
        if (a == null || b == null) return false;
        return a.Trim().ToUpper().Equals(b.Trim().ToUpper());
    }
}
