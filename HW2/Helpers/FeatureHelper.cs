namespace HW2.Helpers
{
    internal static class FeatureHelper
    {
        public static void NumbersFromText(string text, out int? sum, out int? maxDigit)
        {
            sum = 0;
            bool[] digits = new bool[10];
            for (int i = 0; i < text.Length; i++)
            {
                char symbol = text[i];
                if (char.IsDigit(symbol))
                {
                    int digit = symbol - '0';
                    sum += digit;
                    digits[digit] = true;
                }
            }
            maxDigit = null;
            if (sum == 0)
            {
                sum = null;
                return;
                
            }
            for (int i = 9; i >= 0; i--)
            {
                if (digits[i])
                {
                    maxDigit = i;
                    break;
                }
            }
        }

        public static void GetDigitIndex(string text, out int? index) {
            text = text.TrimStart();
            index = null;
            for (int i = 9; i>= 0; i--)
            {
                int ind = text.IndexOf(i.ToString());
                if(ind != -1)
                {
                    index = ind+1;
                    break;
                }
            }
        }
    }
}
