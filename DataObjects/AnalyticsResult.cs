using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects
{
    public class AnalyticsResult
    {
        public String Name { get; set; }
        public Double AverageResponseTime { get; set; }
        public String AverageResponseTimeText
        {
            get
            {
                return String.Format("{0:0.#} min", AverageResponseTime);
            }
        }
        public Int32 ObservationCount { get; set; }
    }
}
