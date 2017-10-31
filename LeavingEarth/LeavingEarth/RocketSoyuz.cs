using System;

namespace LeavingEarth
{
    class RocketSoyuz : Rocket
    {
        private static RocketSoyuz the;

        private RocketSoyuz()
        {
            //Name = Enum.GetName(typeof(RocketType), RocketType.Soyuz);
            Type = RocketType.Soyuz;
            maxPayloads = new double[10] { 0, 71, 31, (double)(17*3+2)/3, 11, 7, (double)(4*3+1)/3, (double)(2*7+3)/7, 1, 0 };
            Mass = 9;
            Cost = 8;
            Available = true;
        }

        public static RocketSoyuz The()
        {
            if (the == null)
            {
                the = new RocketSoyuz();
            }
            return the;
        }
    }
}