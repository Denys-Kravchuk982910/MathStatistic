using Newtonsoft.Json;
using StatisticSoftware.Models;
using StatisticSoftware.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace StatisticSoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<IntervalModel> subIntervals = new List<IntervalModel>();
        List<MLEngineerModel> engineerModels { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            engineerModels = DataService.GeneratePeople();

            this.DataList.ItemsSource = engineerModels;

            double R = engineerModels.Last().Salary
                - engineerModels.First().Salary;

            double CountOfInterval = Math.Ceiling((1 + 3.3221 * Math.Log10(engineerModels.Count)));
            double WidthOfInterval = Math.Ceiling(R / CountOfInterval);

            this.Rozmah.Text = R.ToString();
            this.Intervals.Text = Math.Ceiling((decimal)CountOfInterval).ToString();
            this.WidthInterval.Text = Math.Ceiling((decimal)WidthOfInterval).ToString();


            double xStart = engineerModels[0].Salary;

            for (int i = 0; i < CountOfInterval; i++)
            {
                double subInterval = xStart + WidthOfInterval;

                IntervalModel model = new IntervalModel { Start = xStart, End = subInterval };

                model.Middle = (subInterval + xStart) / 2;

                model.Count = engineerModels
                    .Where(x => x.Salary >= xStart && x.Salary <= subInterval).Count();

                subIntervals.Add(model);

                xStart += WidthOfInterval;
            }

            double mediana = ((engineerModels[engineerModels.Count / 2].Salary
                + engineerModels[engineerModels.Count / 2 - 1].Salary) / 2);

            this.InStR.ItemsSource = subIntervals
                .Select(x => new { Interval = x.Start + " - " + x.End, 
                    Count = x.Count, Middle = x.Middle });

            this.Mediana.Text = mediana.ToString();

            this.Moda.Text = engineerModels.GroupBy(x => x.Salary).OrderByDescending(x => x.Count()).First().Key.ToString();

            long dispSum = 0;

            for (int i = 0; i < engineerModels.Count; i++)
            {
                dispSum += (long)Math.Pow(engineerModels[i].Salary - mediana, 2);
            }

            double disp = dispSum / engineerModels.Count;

            this.Disp.Text = disp.ToString();

            this.SV.Text = Math.Ceiling(engineerModels.Sum(x => x.Salary) 
                / engineerModels.Count).ToString();

            this.KF.Text = Math.Round((Math.Sqrt(disp) / mediana) * 100, 2).ToString();
        }

        private void GenerateTable_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            string json = JsonConvert.SerializeObject(new
            {
                graphics = subIntervals.Select(x => new
                {
                    Middle = x.Middle,
                    Count = x.Count,
                })
            });

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5003/api/Data/set");

            requestMessage.Content = content;

            try
            {
                var response = client.SendAsync(requestMessage).Result;

                string result =  response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сервер не дав правильної відповіді!");
                } else
                {
                    MessageBox.Show("Дані успішно відправлено!");
                }
            } catch
            {
                MessageBox.Show("Something went wrong!");
            }
        }

        private void SecondLab_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow secondWindow = new SecondWindow(subIntervals, engineerModels);

            secondWindow.Show();
        }
    }
}
