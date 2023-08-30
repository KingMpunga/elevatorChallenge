// See https://aka.ms/new-console-template for more information
using Elevator_challenge_DVT_final.constants;
using Elevator_challenge_DVT_final.interfaces;
using Elevator_challenge_DVT_final.services;

class Program
{
    static void Main(string[] args)
    {
        ElevatorSystem elevatorSystem = new ElevatorSystem(
            ElevatorConstants.NumElevators,
            ElevatorConstants.ElevatorCapacity,
            ElevatorConstants.MaxWeight
        );

        while (true)
        {
            Console.Write($"Enter floor where elevator is called (1-{ElevatorConstants.NumFloors}): ");
            int callFloor = int.Parse(Console.ReadLine()!);

            Console.Write("Enter number of people to onboard elevator: ");
            int numPassengers = int.Parse(Console.ReadLine()!);

            List<IPassenger> passengers = new List<IPassenger>();
            List<int> desiredFloors = new List<int>();
            for (int i = 0; i < numPassengers; i++)
            {
                passengers.Add(new Passenger());

                Console.Write($"Enter desired floor for passenger {i + 1} (1-{ElevatorConstants.NumFloors}): ");
                int desiredFloor = int.Parse(Console.ReadLine()!);
                desiredFloors.Add(desiredFloor);
            }

            elevatorSystem.CallElevator(callFloor, passengers, desiredFloors);
        }
    }
}