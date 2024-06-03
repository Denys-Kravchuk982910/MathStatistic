using LiveChartsCore.Defaults;
using StatisticSoftware.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSoftware.Services
{
    public static class DataService
    {
        public static List<MLEngineerModel> GeneratePeople() 
        {
            List<MLEngineerModel> americanEgineers = new List<MLEngineerModel>();
            List<MLEngineerModel> canadianEgineers = new List<MLEngineerModel>();
            List<MLEngineerModel> britainEgineers = new List<MLEngineerModel>();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "ds_salaries.csv");

            using (StreamReader sr = new StreamReader(
                new FileStream(path,
                FileMode.Open, FileAccess.Read)))
            {
                string? line = sr.ReadLine();

                while (line != null)
                {
                    string[] items = line.Split(',');

                    if (items[3] == "Data Scientist"
                        || items[3] == "Data Engineer"
                        || items[3] == "Data Analyst"
                        || items[3] == "Analytics Engineer")
                    {
                        switch (items[9])
                        {
                            case "US":
                                {
                                    if (americanEgineers.Count < 40)
                                    {
                                        americanEgineers.Add(new MLEngineerModel
                                        {
                                            Position = items[3],
                                            Country = "US",
                                            Salary = int.Parse(items[6]),
                                        });
                                    }

                                    break;
                                }
                            case "CA":
                                {
                                    if (canadianEgineers.Count < 40)
                                    {
                                        canadianEgineers.Add(new MLEngineerModel
                                        {
                                            Position = items[3],
                                            Country = "CA",
                                            Salary = int.Parse(items[6]),
                                        });
                                    }

                                    break;
                                }
                            case "GB":
                                {
                                    if (britainEgineers.Count < 40)
                                    {
                                        britainEgineers.Add(new MLEngineerModel
                                        {

                                            Position = items[3],
                                            Country = "GB",
                                            Salary = int.Parse(items[6]),
                                        });
                                    }
                                    break;
                                }
                        }
                    }

                    line = sr.ReadLine();

                    if (americanEgineers.Count >= 40
                        && canadianEgineers.Count >= 40
                        && britainEgineers.Count >= 40)
                    {
                        break;
                    }
                }
            }

            americanEgineers = americanEgineers.OrderBy(x => x.Salary).ToList();
            canadianEgineers = canadianEgineers.OrderBy(x => x.Salary).ToList();
            britainEgineers = britainEgineers.OrderBy(x => x.Salary).ToList();

            List<MLEngineerModel> mLEngineerModels = new List<MLEngineerModel>();

            mLEngineerModels.AddRange(americanEgineers);
            mLEngineerModels.AddRange(canadianEgineers);
            mLEngineerModels.AddRange(britainEgineers);

            mLEngineerModels = mLEngineerModels.OrderBy(x => x.Salary).ToList();

            return mLEngineerModels;
        }

        public static List<MLExperience> GenerateExperience(int count) 
        {
            List<MLExperience> experiences = new List<MLExperience>();
            Random random = new Random();

            double minValue = 0.3;
            double maxValue = 15.0;

            for (int i = 0; i < count; i++)
            {
                experiences.Add(new MLExperience
                {
                    Experience = Math.Round(minValue + (random.NextDouble() * (maxValue - minValue)), 1)
                });
            }


            return experiences.OrderBy(x => x.Experience).ToList();
        }
    }
}
