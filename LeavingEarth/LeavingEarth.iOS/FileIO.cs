using LeavingEarth.iOS;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(FileIO))]
namespace LeavingEarth.iOS
{
    public class FileIO : LeavingEarth.IFileIO
        {
        public void SaveMissions(ObservableCollection<Mission> missions)
        {
            string filePath = Path.Combine(Config.PathApp, "missions.txt");
            using (var file = File.Open(filePath, FileMode.Open, FileAccess.Write))
            using (var strm = new StreamWriter(file))
            {
                foreach (Mission m in missions)
                {
                    var json = JsonConvert.SerializeObject(m, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    strm.WriteLine(json);
                }
            }
        }

        public ObservableCollection<Mission> LoadMissions()
        {
            ObservableCollection<Mission> missions = new ObservableCollection<Mission>();
            string filePath = Path.Combine(Config.PathApp, "missions.txt");
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath);
            }
            using (var file = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var strm = new StreamReader(file))
            {
                while (!strm.EndOfStream)
                {
                    var line = strm.ReadLine();
                    Mission m = JsonConvert.DeserializeObject<Mission>(line);
                    missions.Add(m);
                }
            }
            return missions;
        }
    }
}