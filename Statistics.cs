using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ParkingSimulation
{
    public class Statistics
    {
        public int Served { get; set; } = 0;
        public int Rejected { get; set; } = 0;
        public int MaxQueueLength { get; set; } = 0;
        public List<TimeSpan> WaitTimes { get; set; } = new List<TimeSpan>();
        public List<TimeSpan> ServiceTimes { get; set; } = new List<TimeSpan>();

        public void SaveToTxt(string path)
        {
            var lines = new List<string>
            {
                $"Total Served: {Served}",
                $"Total Rejected: {Rejected}",
                $"Max Queue Length: {MaxQueueLength}",
                $"Average Wait Time: {Average(WaitTimes)}",
                $"Average Service Time: {Average(ServiceTimes)}"
            };
            File.WriteAllLines(path, lines);
        }

        private string Average(List<TimeSpan> times) =>
            times.Count == 0 ? "0s" : TimeSpan.FromMilliseconds(times.Average(t => t.TotalMilliseconds)).ToString(@"hh\:mm\:ss\.fff");
    }
}
