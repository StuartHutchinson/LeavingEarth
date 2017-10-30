using System;

namespace LeavingEarth
{
    class RocketSaturn : Rocket
    {
        private static RocketSaturn the;

        private RocketSaturn()
        {
            Name = Enum.GetName(typeof(RocketType), RocketType.Saturn);
            maxPayloads = new double[10] { 0, 180, 80, (double)(46*3+2)/3, 30, 20, (double)(13*3+1)/3, (double)(8*7+4)/7, 5, (double)(2*9+2)/9 };
            Mass = 20;
            Available = true;
        }

        public static RocketSaturn The()
        {
            if (the == null)
            {
                the = new RocketSaturn();
            }
            return the;
        }
    }
}