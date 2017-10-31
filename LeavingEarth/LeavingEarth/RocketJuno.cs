using System;

namespace LeavingEarth
{
    class RocketJuno : Rocket
    {
        private static RocketJuno the;
        
        private RocketJuno()
        {
            //Name = Enum.GetName(typeof(RocketType), RocketType.Juno);
            Type = RocketType.Juno;
            maxPayloads = new double[10] { 0,3,1, (double)1/3,0,0,0,0,0,0};
            Mass = 1;
            Cost = 1;
            Available = true;
        }

        public static RocketJuno The()
        {
            if (the == null)
            {
                the = new RocketJuno();
            }
            return the;
        }
    }
}
