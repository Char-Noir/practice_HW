
using HW2.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2.Helpers
{
    internal static class ConsoleHelper
    {
        public const int LineLenght = 45;
        public const char ContentSeparator = '#';
        public static void HandleMenu(List<BaseFeature> features)
        {
            bool isExited = false;
            while(!isExited) {
                var result = new StringBuilder();
                var contentSeparator = new string(ConsoleHelper.ContentSeparator, ConsoleHelper.LineLenght);
                result.AppendLine(contentSeparator);
                result.AppendLine("Possible features:");
                for (int i = 0; i < features.Count; i++)
                {
                    result.AppendLine($"{i + 1}. {features[i].GetName()}");
                }
                result.AppendLine("0. Exit");
                Console.WriteLine(result.ToString());
                int variant = ReadIntFromConsole(PredicateHelper.CheckForNumberInRange(features.Count, 0), $"Number must be beetween 0 and {features.Count} inclusively");
                if(variant == 0)
                {
                    isExited = true;
                    continue;
                }
                if (features[variant - 1].HasDefaultInput)
                {
                    Console.WriteLine("Do you want to use default or custom input? (d/c)");
                    string answer = Console.ReadLine();
                    if(answer != null && answer.Contains("d")) {
                        Console.WriteLine(features[variant - 1].ShowFullFormat(true));
                        features[variant - 1].Run(true);
                        continue;
                    }
                }
                Console.WriteLine(features[variant-1].ShowFullFormat());
                features[variant-1].Run();

            }
            
        }

        public static string[] SplitLongLine(string content, int lenght = LineLenght)
        {
            if(content == null || lenght == 0 || content.Length <= lenght)
            {
                return new string[] { string.Empty };
            }

            List<string> lines = new List<string>();
            string[] strings = content.Split(' ');
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < strings.Length; i++)
            {
                if((stringBuilder.Length + strings[i].Length + 1) > lenght)
                {
                    lines.Add(stringBuilder.ToString());
                    stringBuilder = new StringBuilder();
                }
                stringBuilder.Append(strings[i]);
                stringBuilder.Append(' ');
            }
            lines.Add(stringBuilder.ToString());
            return lines.ToArray();
        }

        public static Double ReadDoubleFromConsole(Predicate<Double> predicate, string errorMessage)
        {
            double value = 0;
            bool correctInput = false;
            while(!correctInput)
            {
                string input = Console.ReadLine().Replace('.', ',');
                
                bool isCorrect = double.TryParse(input, out value);
                correctInput = isCorrect && predicate(value);
                if (!correctInput)
                {
                    Console.WriteLine(errorMessage);
                }
            }
            return value;
        }

        public static int ReadIntFromConsole(Predicate<int> predicate, string errorMessage)
        {
            int value = 0;
            bool correctInput = false;
            while (!correctInput)
            {
                string input = Console.ReadLine();

                bool isCorrect = int.TryParse(input, out value);
                correctInput = isCorrect && predicate(value);
                if (!correctInput)
                {
                    Console.WriteLine(errorMessage);
                }
            }
            return value;
        }
    }
}
