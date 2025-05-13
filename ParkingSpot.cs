using System;
using System.Threading;

namespace ParkingSimulation
{
    public class ParkingSpot
    {
        public int Id { get; }
        public bool IsOccupied { get; private set; }
        private readonly object lockObj = new object();
        private readonly Action<int, bool, int?> updateUI;
        private readonly Action<string> log;
        private readonly Action<TimeSpan> logServiceTime;

        public ParkingSpot(int id, Action<int, bool, int?> updateUI, Action<string> logCallback, Action<TimeSpan> serviceLogger)
        {
            Id = id;
            IsOccupied = false;
            this.updateUI = updateUI;
            log = logCallback;
            logServiceTime = serviceLogger;
        }

        public bool TryOccupy(Car car)
        {
            lock (lockObj)
            {
                if (IsOccupied) return false;

                IsOccupied = true;
                updateUI?.Invoke(Id, true, car.Id);
                car.ServiceTimer.Start();

                new Thread(() =>
                {
                    Thread.Sleep(new Random().Next(8000, 12000));
                    car.ServiceTimer.Stop();
                    logServiceTime?.Invoke(car.ServiceTimer.Elapsed);
                    Release();
                }).Start();

                return true;
            }
        }

        public void Release()
        {
            lock (lockObj)
            {
                IsOccupied = false;
                updateUI?.Invoke(Id, false, null);
            }
        }
    }
}
