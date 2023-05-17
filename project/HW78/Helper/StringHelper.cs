namespace HW78.Helper
{
    public static class StringHelper
    {
        public static string TruncateAndAddEllipsis(this string input, int length)
        {
            if (input == null || input.Length <= length)
            {
                return input;
            }

            return input.Substring(0, length - 3) + "...";
        }
    }
}
