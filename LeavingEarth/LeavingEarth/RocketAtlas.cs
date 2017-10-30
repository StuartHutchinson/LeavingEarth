using System;

namespace LeavingEarth
{
    class RocketAtlas : Rocket
    {
        private static RocketAtlas the;

        private RocketAtlas()
        {
            Name = Enum.GetName(typeof(RocketType), RocketType.Atlas);
            maxPayloads = new double[10] { 0, 23, (double)(9*2+1)/2, 5, (double)(2*4+3)/4, (double)(1*5+2)/5, (double)1/2, 0, 0, 0 };
            Mass = 4;
            Available = true;
        }

        public static RocketAtlas The()
        {
            if (the == null)
            {
                the = new RocketAtlas();
            }
            return the;
        }
    }
}