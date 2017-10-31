using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeavingEarth
{
    class AvailableRocketsVM
    {
        public List<Rocket> AllRockets { get; }

        public AvailableRocketsVM()
        {
            AllRockets = new List<Rocket>();
            var types = Enum.GetValues(typeof(Rocket.RocketType));
            foreach (Rocket.RocketType type in types)
            {
                AllRockets.Add(Rocket.GetRocketForType(type));
            }
        }
    }
}
