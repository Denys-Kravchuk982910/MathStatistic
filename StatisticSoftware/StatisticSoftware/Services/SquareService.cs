using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSoftware.Services
{
    public static class SquareService
    {
        public static string ParabolicFunctionality(double sumX, double sumIn2, double sumIn3, double sumIn4,
            double sumY, double sumY2, double sumY3, double lastX)
        {
            double firstMatrix = CalculateMatrix(sumIn4, sumIn3, sumIn2, sumIn3, 
                sumIn2, sumX, sumIn2, sumX, lastX);

            double secondMatrix = CalculateMatrix(sumY3, sumIn3, sumIn2, sumY2,
                sumIn2, sumX, sumY, sumX, lastX);

            double threeMatrix = CalculateMatrix(sumIn4, sumY3, sumIn2, sumIn3,
                sumY2, sumX, sumIn2, sumY, lastX);

            double fourMatrix = CalculateMatrix(sumIn4, sumIn3, sumY3, sumIn3,
                sumIn2, sumY2, sumIn2, sumX, lastX);

            double a = Math.Round(secondMatrix / firstMatrix, 12);
            double b = Math.Round(threeMatrix / firstMatrix, 12);
            double c = Math.Round(fourMatrix / firstMatrix, 12);

            string formula = $"y={a}x^2-{b}x+{c}";

            return formula;
        }

        public static double CalculateMatrix(double item1, double item2, double item3,
            double item4, double item5, double item6, double item7, double item8, double item9)
        {
            double result = item1 * (item5 * item9 - item6 * item8);

            result += item2 * (item4 * item9 - item6 * item7);

            result += item3 * (item4 * item8 - item5 * item7);


            return Math.Round(result, 2);
        }
    }
}
