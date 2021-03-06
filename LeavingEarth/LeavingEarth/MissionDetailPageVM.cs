﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace LeavingEarth
{
    class MissionDetailPageVM : BaseVM
    {
        private INavigation navigation;

        public Mission Mission { get; set; }
        public string MissionName { get { return Mission.Name; } }
        public ObservableCollection<MissionStage> Stages { get; set; }

        public Command AddStageCommand { get; }
        public Command ViewStageCommand { get; }
        public Command DeleteMissionCommand { get; }
        public Command CopyMissionCommand { get; }
        public Command ShoppingListCommand { get; }
        public MissionStage SelectedStage { get; set; }

        public MissionDetailPageVM(Mission m, INavigation nav)
        {
            Mission = m;
            Stages = m.Stages;
            navigation = nav;
            AddStageCommand = new Command(NewStage);
            ViewStageCommand = new Command<MissionStage>(ViewStage);
            DeleteMissionCommand = new Command(DeleteMission);
            CopyMissionCommand = new Command(CopyMission);
            ShoppingListCommand = new Command(ViewShoppingList);
        }

        private async void NewStage()
        {
            string stageName = await Mission.GetNewStageName(navigation);
            if (stageName == null) return; //cancelled

            MissionStage stage = new MissionStage(stageName);
            //if we have previous stages, try to calculate the payload for this one
            if (Stages.Count > 0)
            {
                var lastStage = Stages[Stages.Count - 1];
                short lastStageSolutionMass = lastStage.Solution.CalculateMass();
                stage.Payload = (short)(lastStage.Payload + lastStageSolutionMass);
            }
            //Stages.Add(stage);
            Mission.AddStage(stage);
            ViewStage(stage);
        }
        //SH - 30 Oct 2017 - Moved to Mission
        //private async Task<string> GetNewStageName()
        //{
        //    bool valid = false;
        //    string stageName = null;
        //    while (!valid)
        //    {
        //        stageName = await Dialog.InputBox(navigation, MissionName + " - New Stage", "Enter Stage Name", "");
        //        if (stageName == null)
        //        {
        //            //cancelled
        //            valid = true;
        //        }
        //        else
        //        {
        //            if (stageName.Trim().Length == 0)
        //            {
        //                MessagingCenter.Send<MissionDetailPageVM>(this, Message.BlankStageName); 
        //            }
        //            else if (DuplicateStageName(stageName))
        //            {
        //                MessagingCenter.Send<MissionDetailPageVM>(this, Message.DuplicateStageName);
        //            }
        //            else
        //            {
        //                valid = true;
        //            }
        //        }
        //    }           
        //    return stageName;
        //}
        //private bool DuplicateStageName(string nameToTest)
        //{
        //    var duplicates = Stages.Where(s => s.Description.Equals(nameToTest));
        //    return duplicates.Count() > 0;
        //}

        private async void DeleteMission()
        {
            App.Missions.Remove(Mission);
            await navigation.PopAsync();
        }

        private async void ViewShoppingList()
        {
            await navigation.PushAsync(new ShoppingListPage(Mission));
        }

        private async void CopyMission()
        {
            string missionName = await Mission.GetNewMissionName(navigation);
            if (missionName == null)
            {
                return; //cancelled
            }

            Mission copy = new Mission(Mission);
            copy.Name = missionName;
            App.Missions.Add(copy);
            //pop this page off the stack (back to the mission list)
            await navigation.PopAsync();
        }

        private async void ViewStage(MissionStage stage)
        {
            var newStage = await MissionStagePageVM.Go(navigation, stage);
            if (newStage != null)
            {
                //replace the stage with the edited version
                newStage.OnGetMission = stage.OnGetMission;
                var index = Stages.IndexOf(stage);
                Stages.Remove(stage);
                Stages.Insert(index, newStage);
                //moved down - always do this in case stage has been deleted
                //OnPropertyChanged(nameof(Stages));
            }
            OnPropertyChanged(nameof(Stages));
            SelectedStage = null;
            OnPropertyChanged(nameof(SelectedStage));
        }
    }
}
