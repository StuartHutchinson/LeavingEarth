using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LeavingEarth
{
    class MissionPageVM : BaseVM
    {
        public ObservableCollection<Mission> Missions { get { return App.Missions; } }
        public ICommand NewMissionCommand { get; }
        public ICommand ViewMissionCommand { get; }
        public INavigation Navigation;
        public Mission SelectedMission { get; set; }

        public MissionPageVM(INavigation nav)
        {
            //LoadMissions();
            Navigation = nav;
            NewMissionCommand = new Command(NewMission);
            ViewMissionCommand = new Command<Mission>(ViewMission);
        }

        //private void LoadMissions()
        //{
        //    Missions = DependencyService.Get<IFileIO>().LoadMissions();
        //}

        protected async void NewMission()
        {
            string missionName = await GetNewMissionName();
            if (missionName == null)
            {
                return; //cancelled
            }

            Mission mission = new Mission(missionName);
            App.Missions.Add(mission);
            ViewMission(mission);
        }

        private async Task<string> GetNewMissionName()
        {
            bool valid = false;
            string missionName = null;
            while (!valid)
            {
                missionName = await Dialog.InputBox(Navigation, "New Mission", "Enter Mission Name", "Mission to ");
                if (missionName == null)
                {
                    //cancelled
                    valid = true;
                }
                else
                {
                    if (missionName.Trim().Length == 0)
                    {
                        MessagingCenter.Send<MissionPageVM>(this, Message.BlankMissionName);
                    }
                    else if (DuplicateMissionName(missionName))
                    {
                        MessagingCenter.Send<MissionPageVM>(this, Message.DuplicateMissionName);
                    }
                    else
                    {
                        valid = true;
                    }
                }
            }
            return missionName;
        }
        private bool DuplicateMissionName(string nameToTest)
        {
            var duplicates = Missions.Where(m => m.Name.Equals(nameToTest));
            return duplicates.Count() > 0;
        }

        private async void ViewMission(Mission m)
        {
            await Navigation.PushAsync(new MissionDetailPage(m));
            SelectedMission = null;
            OnPropertyChanged(nameof(SelectedMission));
        }
    }
}
