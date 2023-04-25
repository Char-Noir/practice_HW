using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HW2.Helpers
{
    internal static class PredicateHelper
    {
        public static Predicate<double> CheckForDigitsAfterPoint(int maxLenght)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return x => x.ToString(nfi)[(x.ToString(nfi).IndexOf(".") + 1)..].Length <= maxLenght;
        }

        public static Predicate<double> CheckForBiggerThenNumberOrEquals(double minValue) => x => x >= minValue;

        public static Predicate<int> CheckForBiggerThenNumberOrEquals(int minValue) => x => x >= minValue;
        public static Predicate<int> CheckForLessThenNumberOrEquals(int maxValue) => x => x <= maxValue;

        public static Predicate<int> CheckForNumberInRange(int maxValue, int minValue) => x => x <= maxValue && x >= minValue;
        

        public static Predicate<double> CheckForDigitsAfterPointAndBiggerThenNumber(double minValue, int maxLenght)
        {
            return x => CheckForBiggerThenNumberOrEquals(minValue).Invoke(x) && CheckForDigitsAfterPoint(maxLenght).Invoke(x);
        }
    }
}
