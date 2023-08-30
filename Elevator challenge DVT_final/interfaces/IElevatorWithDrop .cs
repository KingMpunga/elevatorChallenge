using Elevator_challenge_DVT_final.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_challenge_DVT_final.interfaces
{
    interface IElevatorWithDrop : IElevator
    {
        void DropPassengers(int currentFloor);
        Direction GetCurrentDirection();
        bool HasRemainingDesiredFloors();
        void PrintStatus();
        bool HasRemainingDesiredFloorsAbove(int floor);
        bool HasRemainingDesiredFloorsBelow(int floor);
        void ChangeDirection(Direction newDirection);
        IEnumerable<int> GetDesiredFloorsAbove(int floor);
        IEnumerable<int> GetDesiredFloorsBelow(int floor);
    }
}