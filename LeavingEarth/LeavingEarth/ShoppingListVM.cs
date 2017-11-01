using System;
using System.Collections.ObjectModel;

namespace LeavingEarth
{
    class ShoppingListVM : BaseVM
    {
        private Mission mission;
        public ObservableCollection<MissionShoppingListItem> RequiredRockets { get; }

        public ShoppingListVM(Mission m)
        {
            mission = m;
            if (mission.ShoppingList == null)
            {
                mission.ShoppingList = new MissionShoppingList(mission);
            }
            RequiredRockets = mission.ShoppingList.Items;
            foreach (MissionShoppingListItem item in RequiredRockets)
            {
                item.OnBoughtToggled += new Action(delegate { OnPropertyChanged(nameof(RemainingCost)); });
            }
        }

        public string MissionName { get { return mission.Name; } }

        public string TotalCost
        {
            get
            {
                var cost = 0;
                foreach(MissionShoppingListItem item in RequiredRockets)
                {
                    cost += item.Cost;
                }
                return "Total Cost $" + cost;
            }
        }
        public string RemainingCost
        {
            get
            {
                var cost = 0;
                foreach (MissionShoppingListItem item in RequiredRockets)
                {
                    if (!item.Bought)
                    {
                        cost += item.Cost;
                    }
                }
                return "Remaining $" + cost;
            }
        }
    }
}
