using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ParkingSimulation
{
    public class ParkingManager
    {
        private readonly List<ParkingSpot> spots;
        private readonly Queue<Car> carQueue = new Queue<Car>();
        private readonly Random rand = new Random();
        private int carCounter = 0;
        private readonly Action<string> log;
        private readonly Statistics statistics;
        private readonly Action<int, bool, int?> updateSpotStatus;

        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        public ParkingManager(int spotCount, Action<string> logCallback, Statistics stats, Action<int, bool, int?> spotStatusUpdater)
        {
            spots = Enumerable.Range(0, spotCount)
                              .Select(id => new ParkingSpot(id, spotStatusUpdater, logCallback, t => stats.ServiceTimes.Add(t)))
                              .ToList();

            log = logCallback;
            statistics = stats;
            updateSpotStatus = spotStatusUpdater;
        }

        public void StartSimulation()
        {
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            new Thread(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Thread.Sleep(rand.Next(2000, 5000));
                    if (token.IsCancellationRequested) break;

                    Car car = new Car(Interlocked.Increment(ref carCounter));
                    car.WaitTimer.Start();

                    log($"🚗 Car {car.Id} arrived");

                    lock (carQueue)
                    {
                        carQueue.Enqueue(car);
                        if (carQueue.Count > statistics.MaxQueueLength)
                            statistics.MaxQueueLength = carQueue.Count;
                    }

                    ProcessQueue();
                }
            }).Start();
        }

        public void StopSimulation()
        {
            tokenSource?.Cancel();
        }

        private void ProcessQueue()
        {
            new Thread(() =>
            {
                while (!token.IsCancellationRequested && carQueue.Count > 0)
                {
                    Car car;
                    lock (carQueue)
                    {
                        if (carQueue.Count == 0) return;
                        car = carQueue.Peek();
                    }

                    var freeSpot = spots.FirstOrDefault(s => !s.IsOccupied);
                    if (freeSpot != null && freeSpot.TryOccupy(car))
                    {
                        car.WaitTimer.Stop();
                        statistics.WaitTimes.Add(car.WaitTimer.Elapsed);

                        lock (carQueue)
                            carQueue.Dequeue();

                        statistics.Served++;
                        log($"✅ Car {car.Id} parked at Spot {freeSpot.Id}");
                    }

                    Thread.Sleep(500);
                }
            }).Start();
        }
    }
}
