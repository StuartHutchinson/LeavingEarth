using System.Collections.ObjectModel;

namespace LeavingEarth
{
    public interface IFileIO
    {
        void SaveMissions(ObservableCollection<Mission> missions);
        ObservableCollection<Mission> LoadMissions();
    }
}
