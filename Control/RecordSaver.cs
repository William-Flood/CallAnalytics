using System;
using System.Collections.Generic;
using System.Text;
using DataObjects;
using System.IO;

namespace Control
{
    public class RecordSaver
    {
        public static void SaveRecord(String fileName, List<AnalyticsResult> recordList)
        {
            var maxNameLength = 0;
            var maxTimeLength = 0;
            foreach (var record in recordList)
            {
                if (record.Name.Length > maxNameLength)
                {
                    maxNameLength = record.Name.Length;
                }
                if (record.AverageResponseTimeText.Length > maxTimeLength)
                {
                    maxTimeLength = record.AverageResponseTimeText.Length;
                }
            }
            try
            {
                using (var recordSaver = new StreamWriter(fileName))
                {

                    foreach (var record in recordList)
                    {
                        recordSaver.WriteLine(String.Format("{0,-" + maxNameLength + "}     {1,-" + maxNameLength + "}", record.Name, record.AverageResponseTimeText));
                    }
                    recordSaver.Flush();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to save file", ex);
            }
        }
    }
}
