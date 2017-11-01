using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LeavingEarth
{
    public class MissionStageSolution
    {
        public Dictionary<Rocket.RocketType, short> UsedRockets { get; }

        //public MissionStage Stage { get; set; }
        [JsonIgnore]
        public Func<MissionStage> OnGetMissionStage;

        [JsonIgnore]
        public string Description
        {
            get
            {
                var desc = RocketList();
                if (desc.Length > 0)
                {
                    desc += Environment.NewLine;
                }
                desc += "Solution Mass " + CalculateMass() + "T";
                return desc;
            }
            set { }
        }

        //public short Mass { get; set; }

        public MissionStage GetMissionStage()
        {
            if (OnGetMissionStage == null)
                throw new Exception("OnGetMissionStage handler is not assigned");

            return OnGetMissionStage();
        }

        public MissionStageSolution()
        {
            UsedRockets = InitialiseRockets();
        }

        //public MissionStageSolution(MissionStage stage) : this()
        //{            
        //    Stage = stage;
        //}

        public MissionStageSolution(MissionStageSolution original) : this()
        {
            //Stage = original.Stage;
            UsedRockets = new Dictionary<Rocket.RocketType, short>(original.UsedRockets);
            this.OnGetMissionStage = original.OnGetMissionStage;
        }

        private Dictionary<Rocket.RocketType, short> InitialiseRockets()
        {
            var dict = new Dictionary<Rocket.RocketType, short>();
            var types = Enum.GetValues(typeof(Rocket.RocketType));
            foreach (Rocket.RocketType type in types)
            {
                dict[type] = 0;
            }
            return dict;
        }

        public bool IsSufficient()
        {
            return CalculateCapacity() >= GetMissionStage().Payload;
        }

        public double CalculateCapacity()
        {
            double capacity = 0;
            var types = Enum.GetValues(typeof(Rocket.RocketType));
            foreach (Rocket.RocketType type in types)
            {
                var numRockets = UsedRockets[type];
                var rocketPayload = Rocket.GetMaxPayload(type, GetMissionStage().Difficulty);
                capacity += (numRockets * rocketPayload);
            }
            return capacity;
        }

        public short CalculateMass()
        {
            short mass = 0;
            var types = Enum.GetValues(typeof(Rocket.RocketType));
            foreach (Rocket.RocketType type in types)
            {
                var numRockets = UsedRockets[type];
                var rocketMass = Rocket.GetMass(type);
                mass += (short)(numRockets * rocketMass);
            }
            return mass;
        }

        public short CalculateCost()
        {
            short cost = 0;
            var types = Enum.GetValues(typeof(Rocket.RocketType));
            foreach (Rocket.RocketType type in types)
            {
                var numRockets = UsedRockets[type];
                var rocketCost = Rocket.GetCost(type);
                cost += (short)(numRockets * rocketCost);
            }
            return cost;
        }

        public string RocketList()
        {
            var list = "";
            list = AddCountIfNotEmpty(Rocket.RocketType.Juno, list);
            list = AddCountIfNotEmpty(Rocket.RocketType.Atlas, list);
            list = AddCountIfNotEmpty(Rocket.RocketType.Soyuz, list);
            list = AddCountIfNotEmpty(Rocket.RocketType.Saturn, list);
            return list;
        }

        private string AddCountIfNotEmpty(Rocket.RocketType rocketType, string str)
        {
            var count = UsedRockets[rocketType];
            if (count > 0)
            {
                if (str.Length > 0)
                {
                    //str += Environment.NewLine;
                    str += ", ";
                }
                str += count + "x" + Enum.GetName(typeof(Rocket.RocketType), rocketType); ;
            }
            return str;
        }
        
        public void RemoveRocket(Rocket.RocketType rocketType)
        {
            UsedRockets[rocketType]--;
        }
        public void AddRocket(Rocket.RocketType rocketType)
        {
            UsedRockets[rocketType]++;
        }

    }
}
