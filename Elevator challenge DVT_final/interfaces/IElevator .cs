using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_challenge_DVT_final.interfaces
{
    interface IElevator
    {
        void MoveToFloor(int targetFloor);
        bool AddPassengers(List<IPassenger> passengers, List<int> desiredFloors);
        int GetCurrentFloor();
        int GetCurrentCapacity();
        //void PrintStatus();
    }
}