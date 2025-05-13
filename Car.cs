using System.Diagnostics;

namespace ParkingSimulation
{
    public class Car
    {
        public int Id { get; }
        public Stopwatch WaitTimer { get; } = new Stopwatch();
        public Stopwatch ServiceTimer { get; } = new Stopwatch();

        public Car(int id)
        {
            Id = id;
        }
    }
}
