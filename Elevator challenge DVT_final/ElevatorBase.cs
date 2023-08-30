using Elevator_challenge_DVT_final.enums;
using Elevator_challenge_DVT_final.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_challenge_DVT_final
{
    class ElevatorBase : IElevator
    {
        protected int CurrentFloor { get; set; }
        protected Direction CurrentDirection { get;  set; }
        protected int Capacity { get; }
        protected int CurrentCapacity { get; set; }

        public ElevatorBase(int capacity)
        {
            CurrentFloor = 1;
            CurrentDirection = Direction.None;
            Capacity = capacity;
            CurrentCapacity = 0;
        }

        public virtual void MoveToFloor(int targetFloor)
        {
            CurrentDirection = targetFloor > CurrentFloor ? Direction.Up : Direction.Down;
            while (CurrentFloor != targetFloor)
            {
                Console.WriteLine($"Elevator is at floor {CurrentFloor}");
                CurrentFloor += (CurrentDirection == Direction.Up) ? 1 : -1;
            }
            Console.WriteLine($"Elevator reached floor {CurrentFloor}");
            //CurrentDirection = Direction.None;
        }

        public virtual bool AddPassengers(List<IPassenger> passengers, List<int> desiredFloors)
        {
            if (CurrentCapacity + passengers.Count > Capacity)
            {
                Console.WriteLine("Elevator is at full capacity. Cannot board passengers.");
                return false;
            }

            CurrentCapacity += passengers.Count;
            Console.WriteLine($"{passengers.Count} passengers boarded. Current capacity: {CurrentCapacity}");

            for (int i = 0; i < passengers.Count; i++)
            {
                Console.WriteLine($"Passenger {i + 1} desires to go to floor {desiredFloors[i]}");
            }

            return true;
        }

        public int GetCurrentFloor()
        {
            return CurrentFloor;
        }

        public int GetCurrentCapacity()
        {
            return CurrentCapacity;
        }
    }
}