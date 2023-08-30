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
    class ElevatorSystem
    {
        private List<IElevator> elevators;

        public ElevatorSystem(int numElevators, int elevatorCapacity, int maxWeight)
        {
            elevators = new List<IElevator>();
            for (int i = 0; i < numElevators; i++)
            {
                elevators.Add(new Elevator(elevatorCapacity, maxWeight, i + 1));
            }
        }
        public void CallElevator(int callFloor, List<IPassenger> passengers, List<int> desiredFloors)
        {
            if (callFloor <= 0 || callFloor > ElevatorConstants.NumFloors)
            {
                Console.WriteLine("This floor is not accessible.");
                return;
            }

            IElevator nearestElevator = FindNearestElevator(callFloor);
            int nearestElevatorNumber = elevators.IndexOf(nearestElevator) + 1; // Elevator number is index + 1
            nearestElevator.MoveToFloor(callFloor);

            if (nearestElevator.AddPassengers(passengers, desiredFloors))
            {
                Console.WriteLine($"Elevator {nearestElevatorNumber} reached floor {callFloor} to pick up passengers.");

                // Determine the current direction of the elevator
                Direction currentDirection = (nearestElevator as IElevatorWithDrop)?.GetCurrentDirection() ?? Direction.None;

                // Determine the target floor based on the passengers' desired floors and the current direction
                int targetFloor = GetDynamicTargetFloor(nearestElevator.GetCurrentFloor(), desiredFloors, currentDirection);
                nearestElevator.MoveToFloor(targetFloor);

                if (nearestElevator is IElevatorWithDrop elevatorWithDrop)
                {
                    int currentFloor = nearestElevator.GetCurrentFloor();
                    while (elevatorWithDrop.GetCurrentCapacity() > 0)
                    {
                        elevatorWithDrop.DropPassengers(currentFloor);

                        if (elevatorWithDrop.HasRemainingDesiredFloors())
                        {
                            Direction direction = elevatorWithDrop.GetCurrentDirection();

                            if (direction == Direction.Up && elevatorWithDrop.HasRemainingDesiredFloorsAbove(currentFloor))
                            {
                                currentFloor = GetClosestFloorAbove(currentFloor, elevatorWithDrop);
                            }
                            else if (direction == Direction.Down && elevatorWithDrop.HasRemainingDesiredFloorsBelow(currentFloor))
                            {
                                currentFloor = GetClosestFloorBelow(currentFloor, elevatorWithDrop);
                            }
                            else
                            {
                                direction = (direction == Direction.Up) ? Direction.Down : Direction.Up;
                                elevatorWithDrop.ChangeDirection(direction);

                                if (direction == Direction.Up)
                                {
                                    currentFloor = GetClosestFloorAbove(currentFloor, elevatorWithDrop);
                                }
                                else
                                {
                                    currentFloor = GetClosestFloorBelow(currentFloor, elevatorWithDrop);
                                }
                            }
                        }
                        else
                        {
                            break; // No more desired floors, stop elevator
                        }

                        elevatorWithDrop.MoveToFloor(currentFloor);

                        // Print elevator status after each drop
                        elevatorWithDrop.PrintStatus();
                    }
                }
                // Console.WriteLine($"Elevator {nearestElevatorNumber} reached floor {targetFloor}. Passengers have arrived.");
            }
        }
        public void PrintElevatorStatus()
        {
            foreach (var elevator in elevators)
            {
                //Print the elevator status when requested by the user.
                //elevator.PrintStatus();
            }
        }
        private int GetDynamicTargetFloor(int currentFloor, List<int> desiredFloors, Direction currentDirection)
        {
            var validFloors = currentDirection == Direction.Up
                ? desiredFloors.Where(floor => floor >= currentFloor)
                : desiredFloors.Where(floor => floor <= currentFloor);

            if (validFloors.Any())
            {
                int closestFloor = validFloors.OrderBy(floor => Math.Abs(floor - currentFloor)).First();
                return closestFloor;
            }

            int closestFloorOverall = desiredFloors.OrderBy(floor => Math.Abs(floor - currentFloor)).First();
            return closestFloorOverall;
        }
        private IElevator FindNearestElevator(int floor)
        {
            IElevator nearestElevator = elevators[0];
            int minDistance = Math.Abs(elevators[0].GetCurrentFloor() - floor);

            foreach (var elevator in elevators)
            {
                int distance = Math.Abs(elevator.GetCurrentFloor() - floor);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestElevator = elevator;
                }
            }

            return nearestElevator;
        }
        private int GetClosestFloorAbove(int currentFloor, IElevatorWithDrop elevator)
        {
            int minDistance = int.MaxValue;
            int closestFloor = currentFloor;

            foreach (var floor in elevator.GetDesiredFloorsAbove(currentFloor))
            {
                int distance = Math.Abs(floor - currentFloor);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestFloor = floor;
                }
            }

            return closestFloor;
        }

        private int GetClosestFloorBelow(int currentFloor, IElevatorWithDrop elevator)
        {
            int minDistance = int.MaxValue;
            int closestFloor = currentFloor;

            foreach (var floor in elevator.GetDesiredFloorsBelow(currentFloor))
            {
                int distance = Math.Abs(floor - currentFloor);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestFloor = floor;
                }
            }

            return closestFloor;
        }
    }
}