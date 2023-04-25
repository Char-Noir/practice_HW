using HW2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2.Features
{
    internal class GetFastCarsFeature:BaseFeature
    {
        private const int MAX_CARS_SPEED = 1024;
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
                    value[i] = random.Next(1, MAX_CARS_SPEED);
                }
                Console.WriteLine($"Input array: {value}");
            }
            else
            {
                Console.WriteLine("Input speed of each car:");
                for (int i = 0; i < lenghtOfArray; i++)
                {
                    Console.Write($"{i + 1}. ");
                    int pages = ConsoleHelper.ReadIntFromConsole(PredicateHelper.CheckForNumberInRange(MAX_CARS_SPEED,1), "Integer beetween 1 and 1024 inclusivly.");
                    value[i] = pages;
                }
            }
            FeatureHelper.GetFastCarsIndex(value, out int start, out int end);
            if (start == end)
            {
                Console.WriteLine($"Only one fastest machine with a index {start}");
                return;
            }
            Console.WriteLine($"Two or more of the fastest machines with indices from {start} to {end} were found");
        }
        public GetFastCarsFeature()
        {
            Name = "Returns the number of the fastest or fastest machine";
            Description = "Among the sequence of cars, it finds the fastest one and returns either a single number or the first and the last.";
            InputRequirements = $"First, the number of cars, and then the speed for each car. The number of cars is between 1 and 1024 inclusive. Speed from 1 to {MAX_CARS_SPEED}";
            HasDefaultInput = true;
            DefaultInputRequirments = "The number of cars. This value is between 1 and 1024 inclusive";
        }
    }
}
