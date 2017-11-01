using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace LeavingEarth
{
    public class Mission
    {
        public string Name { get; set; }
        public ObservableCollection<MissionStage> Stages { get; set; }
        public MissionShoppingList ShoppingList { get; set; }

        public Mission()
        {
            Stages = new ObservableCollection<MissionStage>();
        }

        public Mission(string name) : this()
        {
            Name = name;
        }

        public Mission(Mission original) : this()
        {
            Name = original.Name;
            foreach(MissionStage stage in original.Stages)
            {
                var newStage = new MissionStage(stage);
                newStage.OnGetMission += new Func<Mission>(GetMission);
                Stages.Add(newStage);
            }
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

        //when loading from disk, make sure all function handlers are set
        public void EnsureLinked()
        {
            foreach (MissionStage stage in Stages)
            {
                if (stage.OnGetMission == null)
                {
                    stage.OnGetMission += new Func<Mission>(GetMission);
                }
                stage.EnsureLinked();
            }
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

        public static async Task<string> GetNewMissionName(INavigation navigation)
        {
            bool valid = false;
            string missionName = null;
            while (!valid)
            {
                missionName = await Dialog.InputBox(navigation, "New Mission", "Enter Mission Name", "Mission to ");
                if (missionName == null)
                {
                    //cancelled
                    valid = true;
                }
                else
                {
                    if (missionName.Trim().Length == 0)
                    {
                        MessagingCenter.Send<Mission>(new Mission(), Message.BlankMissionName);
                    }
                    else if (DuplicateMissionName(missionName))
                    {
                        MessagingCenter.Send<Mission>(new Mission(), Message.DuplicateMissionName);
                    }
                    else
                    {
                        valid = true;
                    }
                }
            }
            return missionName;
        }
        private static bool DuplicateMissionName(string nameToTest)
        {
            var duplicates = App.Missions.Where(m => m.Name.Equals(nameToTest));
            return duplicates.Count() > 0;
        }

    }
}
