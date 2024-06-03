using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSoftware.Tables
{
    public static class LaplasDictionary
    {
        public static Dictionary<double, double> Laplas = new Dictionary<double, double> 
        {
            { 1.9, 0.9713 },
            { 1.645, 0.45 },
            { 0.05, 1.96 },
        };
    }
}
