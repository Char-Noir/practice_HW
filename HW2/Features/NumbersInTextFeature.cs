using HW2.Helpers;
using System.Text;

namespace HW2.Features
{
    internal class NumbersInTextFeature : BaseFeature
    {
        private const double SYMBOL_IS_DIGIT_CHANCE = 0.3d;
        private const string ALPHASPEC_SYMBOLS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+-&|!(){}[]^\"~*?:\\";
        public override void Run(bool isDefaultInput = false)
        {
            string value = string.Empty;
            if (isDefaultInput)
            {
                int lenghtOfString = ConsoleHelper.ReadIntFromConsole(PredicateHelper.CheckForNumberInRange(1024,1), "Integer beetween 1 and 1024 inclusivly.");
                StringBuilder stringBuilder = new();
                Random random = new();
                for (int i = 0; i < lenghtOfString; i++)
                {
                    if (random.NextDouble() <= SYMBOL_IS_DIGIT_CHANCE)
                    {
                        stringBuilder.Append(Math.Round(random.NextDouble()*10,0,MidpointRounding.ToZero));
                    }
                    else
                    {
                        stringBuilder.Append(ALPHASPEC_SYMBOLS[random.Next(ALPHASPEC_SYMBOLS.Length)]);
                    }
                }
                value = stringBuilder.ToString();
                Console.WriteLine($"Input string: {value}");
            }
            else
            {
                while (true)
                {
                    Console.Write("Input string: ");
                    value = Console.ReadLine();
                    if (value != null)
                    {
                        break;
                    }
                    Console.WriteLine("Try again!");
                }
            }
            FeatureHelper.NumbersFromText(value, out int sum, out int maxDigit);
            if(sum == 0)
            {
                Console.WriteLine("There are no digits in the string.");
                return;
            }
            Console.WriteLine($"Sum of digits in the string is {sum}");
            Console.WriteLine($"Maximum digit in the string is {maxDigit}");
        }

        public NumbersInTextFeature() {
            Name = "Find and process digits in text";
            Description = "Finds digits in the text, sums them, and finds the maximum.";
            InputRequirements = "A character string where at least one digit is desirable.";
            HasDefaultInput = true;
            DefaultInputRequirments = "The number of symbols to be generated for the string. The possible values are from 1 to 1024.";
        }
    }
}
