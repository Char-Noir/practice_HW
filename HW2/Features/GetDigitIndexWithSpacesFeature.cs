using HW2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2.Features
{
    internal class GetDigitIndexWithSpacesFeature : BaseFeature
    {
        private const int SPACES_LENGHT_MAX = 10;
        private const int SPACES_LENGHT_MIN = 3;


        public override void Run(bool isDefaultInput = false)
        {
            string value = string.Empty;
            if (isDefaultInput)
            {
                int lenghtOfString = ConsoleHelper.ReadIntFromConsole(PredicateHelper.CheckForNumberInRange(1024, 1), "Integer beetween 1 and 1024 inclusivly.");
                StringBuilder stringBuilder = new();
                Random random = new();
                stringBuilder.Append(new string(' ', random.Next(SPACES_LENGHT_MIN, SPACES_LENGHT_MAX)));
                
                for (int i = 0; i < lenghtOfString; i++)
                {
                    stringBuilder.Append(random.Next(10).ToString());
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
            FeatureHelper.GetDigitIndex(value, out int digitIndex);
            if(digitIndex == -1)
            {
                Console.WriteLine("There are no digits in the string.");
                return;
            }
            Console.WriteLine($"sequential number of the maximal digit {digitIndex}");

        }

        public GetDigitIndexWithSpacesFeature()
        {
            Name = "Get sequential number ";
            Description = "Ignoring spaces, it finds the maximum digit and returns the index of the first occurrence.";
            InputRequirements = "A string of characters that begins with spaces (any number from 0) and then any number of digits.";
            HasDefaultInput = true;
            DefaultInputRequirments = "The number of symbols to be generated for the string. Not including the number of spaces. The possible values are from 1 to 1024.";
        }
    }
}
