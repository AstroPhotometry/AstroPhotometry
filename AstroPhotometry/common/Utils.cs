
namespace AstroPhotometry
{
    public static class Utils
    {
        /**
         * remove suffix if exists - example.txt -> example
         */
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

        /**
         * remove after first substring if exists - example123.txt , "12" -> example12
         */
        public static string RemoveFromSubstring(string s, string substring)
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
