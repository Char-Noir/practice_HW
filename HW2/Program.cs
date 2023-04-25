using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW2.Features;
using HW2.Helpers;

namespace HW2
{
    public class Program
    {
        public static void Main(string[] args)
        {

            List<BaseFeature> features = new List<BaseFeature>
            {
                new NumbersInTextFeature(),
                new GetDigitIndexWithSpacesFeature(),
                new GetThickBookFeature(),
                new GetFastCarsFeature()
            };
            ConsoleHelper.HandleMenu(features);
        }
    }
}
