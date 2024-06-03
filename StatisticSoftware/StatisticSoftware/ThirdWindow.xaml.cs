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
    /// Interaction logic for ThirdWindow.xaml
    /// </summary>
    public partial class ThirdWindow : Window
    {
        public ThirdWindow(List<MLEngineerModel> engineerModels, List<IntervalModel> _subIntervals)
        {
            InitializeComponent();

            List<MLExperience> mLExperiences = DataService.GenerateExperience(engineerModels.Count);

            List<object> items = new List<object>();

            for (int i = 0; i < engineerModels.Count; i++)
            {
                var item = new
                {
                    Experience = mLExperiences[i].Experience,
                    Position = engineerModels[i].Position,
                    Salary = engineerModels[i].Salary,
                };

                items.Add(item);
            }

            this.DataList.ItemsSource = items;

            int CountOfIntervals = _subIntervals.Count;

            double R = mLExperiences.Last().Experience
                - mLExperiences.First().Experience;
            double WidthOfInterval = Math.Ceiling(R / CountOfIntervals);

            double xStart = mLExperiences[0].Experience;

            List<IntervalModel> intervals = new List<IntervalModel>();

            for (int i = 0; i < CountOfIntervals; i++)
            {
                double subInt = xStart + WidthOfInterval;

                IntervalModel model = new IntervalModel { Start = xStart, End = subInt };

                model.Middle = Math.Round((subInt + xStart) / 2, 1);

                model.Count = mLExperiences
                    .Where(x => x.Experience >= xStart && x.Experience <= subInt).Count();

                intervals.Add(model);

                xStart += WidthOfInterval;
            }


            List<object> subInterval = new List<object>();

            for (int i = 0; i < _subIntervals.Count; i++)
            {
                subInterval.Add(new
                {
                    Salary = _subIntervals[i].Middle,
                    Experience = intervals[i].Middle
                });
            }

            this.InStR.ItemsSource = subInterval;


            double salarySum = engineerModels.Sum(x => x.Salary);
            double salarySumSquared = engineerModels.Sum(x => Math.Round(Math.Pow(x.Salary, 2), 2));

            double experienceSum = mLExperiences.Sum(x => x.Experience);
            double experienceSumSquared = mLExperiences.Sum(x => Math.Round(Math.Pow(x.Experience, 2), 2));

            double sumCount = 0;

            for (int i = 0; i < _subIntervals.Count; i++)
            {
                double sum = _subIntervals[i].Middle * intervals[i].Middle * _subIntervals[i].Count;

                sumCount += sum;
            }

            double xDashed = Math.Round(salarySum / engineerModels.Count, 4);
            double yDashed = Math.Round(experienceSum / engineerModels.Count, 4);

            double Sx = Math.Round((salarySumSquared / engineerModels.Count) - Math.Pow(xDashed, 2), 2);
            double Sy = Math.Round((experienceSumSquared / engineerModels.Count) - Math.Pow(yDashed, 2), 2);

            double Mu = Math.Pow(sumCount / engineerModels.Count, 3) - xDashed * yDashed;

            double Byx = Mu / Sx;
            double Bxy = Mu / Sy;

            string Yx = (Byx + "x").ToString() + " + " +
                (Byx * xDashed + yDashed).ToString();

            string Xy = (Bxy + "y").ToString() + " + " +
                (Bxy * yDashed + xDashed).ToString();

            this.RegressionXy.Text = Xy;
            this.RegressionYx.Text = Yx;

            double a = 0.05;

            double korelationKoef = Math.Round(Math.Sqrt(Byx * Bxy), 2);

            this.KorelationKoef.Text = korelationKoef.ToString();

            double statistic = korelationKoef * Math.Sqrt(engineerModels.Count - 2) 
                / Math.Sqrt(Math.Abs(1 - Math.Pow(korelationKoef, 2)));

            if (statistic > StudentDictionary.Student["0.95;119"])
            {
                this.KorelationKoefDiff.Text = "Значно більший 0";
            }


            double accurance = 0.95;

            double zFisher = 1 / 2 * Math.Ceiling((Math.Log(Math.Abs((1 + korelationKoef) / (1 - korelationKoef)))));


            // Довірчий інтервал для M(z)

            double leftSide = Math.Round(zFisher - LaplasDictionary.Laplas[Math.Round(1 - accurance, 2)] / Math.Sqrt(engineerModels.Count - 2), 2);
            double rightSide = Math.Round(zFisher + LaplasDictionary.Laplas[Math.Round(1 - accurance, 2)] / Math.Sqrt(engineerModels.Count - 2), 2);

            this.Mz.Text = leftSide + " < M(x) <" + rightSide;
            this.RoInterval.Text = "0.581 <= R <= 0.844";


            double SxSqrt = Math.Round(Math.Sqrt(Sx), 2);
            double SySqrt = Math.Round(Math.Sqrt(Sy), 2);

            double leftBetayx = Byx - StudentDictionary.Student["0.05;119"] * 
                SySqrt * Math.Sqrt(Math.Abs(1- korelationKoef)) 
                / SxSqrt * Math.Sqrt(engineerModels.Count - 2);

            double rightBetayx = Byx + StudentDictionary.Student["0.05;119"] *
                SySqrt * Math.Sqrt(Math.Abs(1 - korelationKoef))
                / SxSqrt * Math.Sqrt(engineerModels.Count - 2);

            this.RegressionLineYX.Text = leftBetayx + " <= Byx <= " + rightBetayx;


            double leftBetaxy = Bxy - StudentDictionary.Student["0.05;119"] *
                SxSqrt * Math.Sqrt(Math.Abs(1 - Math.Pow(korelationKoef, 2)))
                / SySqrt * Math.Sqrt(engineerModels.Count - 2);

            double rightBetaxy = Bxy + StudentDictionary.Student["0.05;119"] *
                SxSqrt * Math.Sqrt(Math.Abs(1 - Math.Pow(korelationKoef, 2)))
                / SySqrt * Math.Sqrt(engineerModels.Count - 2);

            this.RegressionLineXY.Text = leftBetaxy + " <= Byx <= " + rightBetaxy;
        }
    }
}
