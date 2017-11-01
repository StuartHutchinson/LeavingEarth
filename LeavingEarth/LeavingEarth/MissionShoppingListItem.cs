using Newtonsoft.Json;
using System;

namespace LeavingEarth
{
    public class MissionShoppingListItem
    {
        [JsonIgnore]
        public Action OnBoughtToggled;

        public Rocket.RocketType Type { get; }
        private bool bought;

        public bool Bought
        {
            get
            {
                return bought;
            }
            set
            {
                bought = value;
                OnBoughtToggled?.Invoke();
            }
        }

        //[JsonIgnore]
        //public string Name { get { return Enum.GetName(typeof(Rocket.RocketType), Type); } }

        [JsonIgnore]
        public string NameAndCost
        {
            get
            {
                return Enum.GetName(typeof(Rocket.RocketType), Type) + " - $" + Cost;
            }
        }

        [JsonIgnore]
        public short Cost { get { return Rocket.GetCost(Type); } }
        
        public MissionShoppingListItem(Rocket.RocketType type)
        {
            Type = type;
            Bought = false;
        }
    }
}
