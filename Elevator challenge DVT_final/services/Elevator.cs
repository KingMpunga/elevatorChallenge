using Elevator_challenge_DVT_final.constants;
using Elevator_challenge_DVT_final.enums;
using Elevator_challenge_DVT_final.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_challenge_DVT_final.services
{
    class Elevator : ElevatorBase, IElevatorWithDrop
    {
        private int MaxWeight { get; }
        private int CurrentWeight { get; set; }
        private Dictionary<int, List<int>> PassengersDesiredFloors { get; set; }
        private int ElevatorNumber { get; }

        public Elevator(int capacity, int maxWeight, int elevatorNumber)
            : base(capacity)
        {
            MaxWeight = maxWeight;
            CurrentWeight = 0;
            PassengersDesiredFloors = new Dictionary<int, List<int>>();
            ElevatorNumber = elevatorNumber;
            CurrentDirection = Direction.None; // Initialize the direction
        }

        public override bool AddPassengers(List<IPassenger> passengers, List<int> desiredFloors)
        {
            int totalWeight = passengers.Sum(passenger => passenger.GetWeight());

            if (CurrentWeight + totalWeight > MaxWeight)
            {
                Console.WriteLine("Exceeds elevator weight limit. Cannot board passengers.");
                return false;
            }

            CurrentWeight += totalWeight;

            for (int i = 0; i < passengers.Count; i++)
            {
                if (!PassengersDesiredFloors.ContainsKey(passengers[i].GetHashCode()))
                {
                    PassengersDesiredFloors[passengers[i].GetHashCode()] = new List<int>();
                }

                PassengersDesiredFloors[passengers[i].GetHashCode()].Add(desiredFloors[i]);
            }

            return base.AddPassengers(passengers, desiredFloors);
        }

        public void DropPassengers(int currentFloor)
        {
            List<int> passengerHashesToRemove = new List<int>();

            foreach (var passengerHash in PassengersDesiredFloors.Keys.ToList())
            {
                if (PassengersDesiredFloors[passengerHash].Contains(currentFloor))
                {
                    Console.WriteLine($"Passenger desires to get off at floor {currentFloor}");
                    PassengersDesiredFloors[passengerHash].Remove(currentFloor);

                    if (PassengersDesiredFloors[passengerHash].Count == 0)
                    {
                        passengerHashesToRemove.Add(passengerHash);
                    }
                }
            }

            foreach (var passengerHashToRemove in passengerHashesToRemove)
            {
                PassengersDesiredFloors.Remove(passengerHashToRemove);
            }

            UpdateCurrentCapacity();
        }
        public bool HasRemainingDesiredFloorsAbove(int floor)
        {
            return PassengersDesiredFloors.Values.Any(list => list.Count > 0 && list.Max() > floor);
        }

        public bool HasRemainingDesiredFloorsBelow(int floor)
        {
            return PassengersDesiredFloors.Values.Any(list => list.Count > 0 && list.Min() < floor);
        }

        public void ChangeDirection(Direction newDirection)
        {
            CurrentDirection = newDirection;
        }
        public Direction GetCurrentDirection()
        {
            return CurrentDirection;
        }

        public bool HasRemainingDesiredFloors()
        {
            var t = PassengersDesiredFloors.Values.Any(list => list.Count > 0);
            return t;
        }

        public void PrintStatus()
        {
            Console.WriteLine($"Elevator {ElevatorNumber} is on floor {GetCurrentFloor()} and moving {CurrentDirection}");
        }

        public IEnumerable<int> GetDesiredFloorsAbove(int floor)
        {
            return PassengersDesiredFloors.Values.SelectMany(list => list).Where(f => f > floor);
        }

        public IEnumerable<int> GetDesiredFloorsBelow(int floor)
        {
            return PassengersDesiredFloors.Values.SelectMany(list => list).Where(f => f < floor);
        }

        private void UpdateCurrentCapacity()
        {
            int totalPassengers = PassengersDesiredFloors.Values.Sum(list => list.Count);
            CurrentCapacity = totalPassengers;
        }
    }
}