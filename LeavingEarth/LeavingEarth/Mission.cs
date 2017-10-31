using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Newtonsoft.Json;

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
            stage.OnGetMission += new Func<Mission>(GetMission);
        }

        private Mission GetMission()
        {
            return this;
        }

        public async Task<string> GetNewStageName(INavigation navigation)
        {
            return await GetStageName(navigation, Name + " - New Stage");
        }
        public async Task<string> GetRenamedStageName(INavigation navigation, string originalName)
        {
            return await GetStageName(navigation, "Rename stage - " + originalName);
        }
        private async Task<string> GetStageName(INavigation navigation, string dialogTitle)
        {
            bool valid = false;
            string stageName = null;
            while (!valid)
            {
                stageName = await Dialog.InputBox(navigation, dialogTitle, "Enter Stage Name", "");
                if (stageName == null)
                {
                    //cancelled
                    valid = true;
                }
                else
                {
                    if (stageName.Trim().Length == 0)
                    {
                        MessagingCenter.Send<Mission>(this, Message.BlankStageName);
                    }
                    else if (DuplicateStageName(stageName))
                    {
                        MessagingCenter.Send<Mission>(this, Message.DuplicateStageName);
                    }
                    else
                    {
                        valid = true;
                    }
                }
            }
            return stageName;
        }
        private bool DuplicateStageName(string nameToTest)
        {
            var duplicates = Stages.Where(s => s.Description.Equals(nameToTest));
            return duplicates.Count() > 0;
        }

        //todo - this should confirm with the user and return a bool
        public void DeleteStage(MissionStage stage)
        {
            Stages.Remove(stage);
        }

        //these do a bit of extra work to fix the functions which cannot be serialiazed
        public static Mission JsonDeserialize(string line)
        {
            Mission m = JsonConvert.DeserializeObject<Mission>(line);
            foreach (MissionStage stage in m.Stages)
            {
                stage.OnGetMission = new Func<Mission>(m.GetMission);
            }
            return m;
        }
        public void PrepareForSave()
        {
            foreach (MissionStage stage in Stages)
            {
                stage.OnGetMission -= GetMission;
                stage.PrepareForSave();
            }
        }
    }
}
