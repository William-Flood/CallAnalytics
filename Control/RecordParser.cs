using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DataObjects;
using System.Text.RegularExpressions;

namespace Control
{
    public class RecordParser
    {
        const string HELPER_TITLE = "Helper";
        public static List<AnalyticsResult> Parse(string[] fileNames)
        {
            var resultList = new List<AnalyticsResult>();

            foreach(var file in fileNames)
            {
                var lastSeenUser = "Helper";
                var lastSeenTime = "";
                var lastSeenTimeDivisionMarker = "";
                var lineIndex = 0;
                var totalChangeToHelperCount = 0;
                var totalMinutesPast = 0d;
                using (var fileReader = new StreamReader(file))
                {
                    while (!fileReader.EndOfStream)
                    {
                        var line = fileReader.ReadLine();
                        line = Regex.Replace(line, "\\s+", " ");
                        if("".Equals(line))
                        {
                            continue;
                        }
                        var splitLine = line.Split(' ');
                        if (3 > splitLine.Length)
                        {
                            throw new Exception("Invalid line " + lineIndex + " in " + file);
                        }
                        int testInt;
                        if(2 > splitLine[1].Split(':').Length && !(Int32.TryParse(splitLine[1].Split(':')[0], out testInt) && Int32.TryParse(splitLine[1].Split(':')[1], out testInt)))
                        {
                            continue;
                        }
                        if (HELPER_TITLE.Equals(splitLine[0]) && !HELPER_TITLE.Equals(lastSeenUser))
                        {
                            var hourAndMinute = splitLine[1].Split(':');
                            var minutesPast = Int32.Parse(hourAndMinute[1]) - Int32.Parse(lastSeenTime.Split(':')[1]);
                            minutesPast += 60 * (Int32.Parse(hourAndMinute[0]) - Int32.Parse(lastSeenTime.Split(':')[0]));
                            if (!lastSeenTimeDivisionMarker.Equals(splitLine[2]))
                            {
                                minutesPast += 720;
                            } else if (0 > minutesPast)
                            {
                                minutesPast += 1440;
                            }
                            totalChangeToHelperCount++;
                            totalMinutesPast += minutesPast;
                        }
                        lastSeenUser = splitLine[0];
                        lastSeenTime = splitLine[1];
                        lastSeenTimeDivisionMarker = splitLine[2];
                        lineIndex++;
                    }
                }
                resultList.Add(new AnalyticsResult() { ObservationCount = totalChangeToHelperCount, AverageResponseTime = totalMinutesPast / totalChangeToHelperCount, Name = Path.GetFileName(file) });

            }
            return resultList; 
        }
    }
}
