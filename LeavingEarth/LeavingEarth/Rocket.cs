using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeavingEarth
{
    public abstract class Rocket
    {
        public enum RocketType { Juno, Atlas, Soyuz, Saturn };

        protected double[] maxPayloads;

        public RocketType Type { get; set; }
        //public string Name { get; set; }
        public string Name { get { return Enum.GetName(typeof(RocketType), Type); } }
        public bool Available { get; set; }
        public short Mass { get; set; }
        public short Cost { get; set; }

        public double GetMaxPayload(DifficultyLevel difficulty)
        {
            return maxPayloads[(int)difficulty];
        }

        public static double GetMaxPayload(RocketType type, DifficultyLevel difficulty)
        {
            Rocket r = GetRocketForType(type);
            return r.GetMaxPayload(difficulty);
        }

        public static short GetMass(RocketType type)
        {
            Rocket r = GetRocketForType(type);
            return r.Mass;
        }

        public static Rocket GetRocketForType(RocketType type)
        {
            Rocket rocket = null;
            switch (type)
            {
                case RocketType.Juno:
                    rocket = RocketJuno.The();
                    break;
                case RocketType.Atlas:
                    rocket = RocketAtlas.The();
                    break;
                case RocketType.Soyuz:
                    rocket = RocketSoyuz.The();
                    break;
                case RocketType.Saturn:
                    rocket = RocketSaturn.The();
                    break;
                default:
                    throw new Exception("Unexpected rocket type " + type);
            }
            return rocket;
        }

        public static RocketType GetRocketType(string str)
        {
            return (RocketType)Enum.Parse(typeof(RocketType), str);
        }
    }
}
