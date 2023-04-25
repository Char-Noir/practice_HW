namespace HW2.Helpers
{
    internal static class FeatureHelper
    {
        public static void NumbersFromText(string text, out int sum, out int maxDigit)
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
            maxDigit = -1;
            if (sum == 0)
            {
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

        public static void GetDigitIndex(string text, out int index) {
            text = text.TrimStart();
            index = -1;
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

        public static void GetMaxValue(int[] books, out int max)
        {
            max = books.Max();
            if(max<0)
            {
                max = 0;
            }
        }

        public static void GetFastCarsIndex(int[] cars, out int startIndex, out int endIndex)
        {
            GetMaxValue(cars, out int max);
            startIndex = Array.IndexOf(cars, max)+1;
            endIndex = Array.LastIndexOf(cars, max)+1;
        }
        

    }
}
