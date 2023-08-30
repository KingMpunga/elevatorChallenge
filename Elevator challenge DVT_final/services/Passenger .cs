using Elevator_challenge_DVT_final.constants;
using Elevator_challenge_DVT_final.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_challenge_DVT_final.services
{
    public class Passenger : IPassenger
    {
        public int GetWeight()
        {
            return PassengerConstants.StandardWeight;
        }
    }
}