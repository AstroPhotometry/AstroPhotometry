
namespace AstroPhotometry
{
    public static class Utils
    {
        public static string RemoveFromEnd(string s, string suffix)
        {
            if (s.EndsWith(suffix))
            {
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                return s;
            }
        }
        public static string RemoveFromSubsting(string s, string substring)
        {
            if (s.IndexOf(substring) != -1)
            {
                return s.Substring(0, s.IndexOf(substring) + substring.Length);
            }
            else
            {
                return s;
            }
        }

    }
}
