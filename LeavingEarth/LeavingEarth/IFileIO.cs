using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeavingEarth
{
    public interface IFileIO
    {
        void SaveMissions(ObservableCollection<Mission> missions);
        ObservableCollection<Mission> LoadMissions();
    }
}
