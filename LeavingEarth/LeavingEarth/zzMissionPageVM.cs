using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace LeavingEarth
{
    class MissionPageVM : BaseVM
    {
        private Mission mission;

        public INavigation Navigation { get; set; }
        public ObservableCollection<MissionStage> Stages { get; set; }
        public ICommand AddStageCommand { get; }
        public ICommand ViewMissionCommand { get; }
        public ICommand ViewStageCommand { get; }        
        public MissionStage SelectedStage { get; set; }

        public string MissionName
        {
            get { return mission.Name; }
            set { mission.Name = value; }
        }

        public MissionPageVM(Mission m, INavigation nav)
        {
            AddStageCommand = new Command(NewStage);
            ViewMissionCommand = new Command(ViewMission);
            ViewStageCommand = new Command<MissionStage>(ViewStage);
            Navigation = nav;
            mission = m;
            Stages = m.Stages;
        }

        private async void ViewStage(MissionStage stage)
        {
            var newStage = await MissionStagePageVM.Edit(Navigation, stage);
            if (newStage != null)
            {
                var index = Stages.IndexOf(stage);
                Stages.Remove(stage);
                Stages.Insert(index, newStage);
                OnPropertyChanged(nameof(Stages));
            }
            SelectedStage = null;
            OnPropertyChanged(nameof(SelectedStage));
        }

        private void ViewMission()
        {
            MissionDetailPageVM.ViewMission(mission, this);
        }

        private async void NewStage()
        {
            string stageName = await Dialog.InputBox(Navigation, mission.Name + " - New Stage", "Enter Stage Name", "");

            while (stageName != null && stageName.Trim().Length == 0)
            {
                MessagingCenter.Send<MissionPageVM>(this, "BlankStageName");
                stageName = await Dialog.InputBox(Navigation, mission.Name + " - New Stage", "Enter Stage Name", "");
            }

            if (stageName == null) return; //cancelled

            MissionStage stage = new MissionStage(stageName);
            //if we have previous stages, try to calculate the payload for this one
            if (Stages.Count > 0)
            {
                var lastStage = Stages[Stages.Count - 1];
                short lastStageSolutionMass = lastStage.Solution.Mass;
                stage.Payload = (short)(lastStage.Payload + lastStageSolutionMass);
            }
            Stages.Add(stage);
            ViewStage(stage);
        }
    }
}
