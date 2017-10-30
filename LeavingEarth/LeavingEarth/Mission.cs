using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LeavingEarth
{
    public class Mission
    {
        public string Name { get; set; }
        public ObservableCollection<MissionStage> Stages { get; set; }

        public Mission(string name)
        {
            Name = name;
            Stages = new ObservableCollection<MissionStage>();
        }

        public void AddStage(MissionStage stage)
        {
            if (Stages == null)
            {
                Stages = new ObservableCollection<MissionStage>();
            }
            Stages.Add(stage);
        }
    }
}
