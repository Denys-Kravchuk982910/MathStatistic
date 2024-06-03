using StatisticSoftware.Models;
using StatisticSoftware.Services;
using StatisticSoftware.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StatisticSoftware
{
    /// <summary>
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        List<IntervalModel> _subIntervals { get; set; }
        List<MLEngineerModel> _engineerModels { get; set; }
        public SecondWindow(List<IntervalModel> subIntervals, 
            List<MLEngineerModel> engineerModels)
        {
            InitializeComponent();

            this._subIntervals = subIntervals;
            this._engineerModels = engineerModels;

            // 130000 = 13 * 10000

            double assessmentGenS = Math.Round(engineerModels.Sum(x => x.Salary) / engineerModels.Count, 2);
            double assessmentGenD = Math.Round(engineerModels.Sum(x => Math.Pow(x.Salary, 2)) / engineerModels.Count, 2);
            double assessmentGenDSqrt = Math.Round(Math.Sqrt(assessmentGenD / engineerModels.Count), 2);

            this.AssessmentGenS.Text = assessmentGenS.ToString();
            this.AssessmentGenD.Text = assessmentGenD.ToString();
            this.AvSqD.Text = assessmentGenDSqrt.ToString();


            double sumX = subIntervals.Sum(x => x.Middle);
            double sumIn2 = subIntervals.Sum(x => Math.Pow(x.Middle, 2));
            double sumIn3 = subIntervals.Sum(x => Math.Pow(x.Middle, 3));
            double sumIn4 = subIntervals.Sum(x => Math.Pow(x.Middle, 4));

            double sumY = subIntervals.Sum(x => x.Count);
            double sumY2 = subIntervals.Sum(x => x.Count * x.Middle);
            double sumY3 = subIntervals.Sum(x => x.Count * Math.Pow(x.Middle, 2));

            string formula = SquareService
                .ParabolicFunctionality(sumX, sumIn2, sumIn3, sumIn4, 
                sumY, sumY2, sumY3, subIntervals.Last().Middle);

            this.Equation.Text = formula;



            double accurance = 0.95;
            double laplas = 1.96;
            double averageKvError = Math.Round(Math.Sqrt(assessmentGenD / engineerModels.Count), 2);

            double errorInterval = laplas * averageKvError;

            this.AvSError.Text = averageKvError.ToString();
            this.TrustIntevalS.Text = (assessmentGenS - errorInterval) + "< x < " + (assessmentGenS + errorInterval);

            double X1InPow2 = Math.Round(Math.Pow(Math.Sqrt(2 * engineerModels.Count - 1) - laplas, 2) / 2, 2);
            double X2InPow2 = Math.Round(Math.Pow(Math.Sqrt(2 * engineerModels.Count - 1) + laplas, 2) / 2, 2);

            double leftD = Math.Round((engineerModels.Count / X2InPow2) * Math.Pow(averageKvError, 2), 2);
            double rightD = Math.Round((engineerModels.Count / X1InPow2) * Math.Pow(averageKvError, 2), 2);

            this.TrustIntevalD.Text = leftD + " < ς < " + rightD;

            double a = 130000;
            double tStudent = 1.98;


            ///
            /// Гіпотеза - перевірити чи дорівнює зарплата програмістів певному значенню.
            /// Гіпотеза є двостороньою, але мені потрібно перевірити чи середня 
            /// зарплата є більшою за деяке значення
            /// 
            /// H0 - середня зарплата рівна деякому значенню. Значення: 130000; H0: a = 130000
            /// H1 - середня зарплата більша за деяке значення. u > 130000; H1: a > 130000
            /// 
            /// В якості статистики критерію використаємо оцінку математичного сподівання
            /// 
            /// Критична область правостороння, та t(критичне) = 1.98; Tспост >= t(критичне)
            /// 
            /// Критерій перевірки нульової гіпотези:
            /// 
            /// Math.Round(((assessmentGenS - a) / assessmentGenD) * Math.Sqrt(engineerModels.Count - 1), 2);
            ///

            double TObs = Math.Round(((assessmentGenS - a) / assessmentGenD) * 
                Math.Sqrt(engineerModels.Count - 1), 6);

            if (TObs > tStudent)
            {
                this.HZero.Text = "Відхиляється";
            } else
            {
                this.HZero.Text = "Не відхиляється";
            }


            double FValue = Math.Round(tStudent - (assessmentGenS - a) / 
                Math.Sqrt(assessmentGenD) * Math.Sqrt(engineerModels.Count - 1), 2);

            this.PowerOfCriteria.Text = (LaplasDictionary.Laplas[FValue]).ToString();

            double tStudentBeta = StudentDictionary.Student[FValue + ";" + (engineerModels.Count - 1).ToString()];

            double n = Math.Pow(tStudent + tStudentBeta, 2) * assessmentGenD 
                / Math.Pow((a - assessmentGenS), 2);

            this.MinRangeOfItems.Text = Math.Ceiling(n).ToString();




            /// H0 - кількість програмістів, які заробляють більше 200000 не більше як 25%
            /// p < 25%
            double percentage = 0.25;

            double ZObs = (1 - percentage) / Math.Sqrt(percentage * (1 - percentage));

            double varToFind = Math.Round((1 - 2 * (1 - accurance)) / 2, 2);

            double val = LaplasDictionary.Laplas.Values.Where(x => x == varToFind).First();
            
            
            if (ZObs > val)
            {
                this.HGenRange.Text = "Не відхиляється";
            } else
            {
                this.HGenRange.Text = "Відхиляється";
            }
        }

        private void SecondLab_Click(object sender, RoutedEventArgs e)
        {
            ThirdWindow thirdWindow = new ThirdWindow(this._engineerModels, 
                this._subIntervals);

            thirdWindow.Show();
        }
    }
}
