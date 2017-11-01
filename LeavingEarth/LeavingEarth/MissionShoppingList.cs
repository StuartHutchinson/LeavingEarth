using System;
using System.Collections.ObjectModel;

namespace LeavingEarth
{
    public class MissionShoppingList
    {
        public ObservableCollection<MissionShoppingListItem> Items { get; }

        public MissionShoppingList()
        {
            Items = new ObservableCollection<MissionShoppingListItem>();
        }

        public MissionShoppingList(Mission m) : this()
        {
            var types = Enum.GetValues(typeof(Rocket.RocketType));
            foreach (Rocket.RocketType type in types)
            { 
                foreach (MissionStage stage in m.Stages)
                {
                    var numRockets = stage.Solution.UsedRockets[type];
                    for (short i=0; i<numRockets; i++)
                    {
                        var item = new MissionShoppingListItem(type);
                        Items.Add(item);
                    }
                }
            }
        }
    }
}
