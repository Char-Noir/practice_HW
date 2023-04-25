using HW2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2.Features
{
    internal class GetThickBook : BaseFeature
    {
        private const int MAX_BOOK_PAGES_LENGHT = 1024;
        public override void Run(bool isDefaultInput = false)
        {
            Console.Write("Input lenght of array: ");
            int[] value = Array.Empty<int>();
            int lenghtOfArray = ConsoleHelper.ReadIntFromConsole(PredicateHelper.CheckForNumberInRange(1024, 1), "Integer beetween 1 and 1024 inclusivly.");
            value = new int[lenghtOfArray];
            if (isDefaultInput)
            {
                Random random = new();
                for (int i = 0; i < lenghtOfArray; i++)
                {
                    value[i] = random.Next(1, MAX_BOOK_PAGES_LENGHT);
                }
                Console.WriteLine($"Input array: {value}");
            }
            else
            {
                Console.WriteLine("Input size of each book:");
                for (int i = 0; i < lenghtOfArray; i++)
                {
                    Console.Write($"{i + 1}. ");
                    int pages = ConsoleHelper.ReadIntFromConsole(PredicateHelper.CheckForNumberInRange(1, MAX_BOOK_PAGES_LENGHT), "Integer beetween 1 and 1024 inclusivly.");
                    value[i] = pages;
                }
            }
            FeatureHelper.GetMaxValue(value, out int max);
            if(max == 0 )
            {
                Console.WriteLine("There are no books or books have no pages");
                return;
            }
            Console.WriteLine($"Max books pages amount is {max}");
        }
    }
}
