using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LeavingEarth
{
    public class AddRocketsPageVM : BaseVM
    {
        private TaskCompletionSource<MissionStageSolution> retval = new TaskCompletionSource<MissionStageSolution>();
        
        private INavigation navigation;

        public double RequiredPayload { get; set; }
        public double CurrentCapacity { get; set; }
        public MissionStageSolution Solution { get; set; }

        public string SolutionRocketList { get { return Solution.RocketList(); } }
        public Color CapacityColour { get { return Solution.IsSufficient() ? Color.LightGreen : Color.PaleVioletRed; } }

        public ObservableCollection<AddRocketsPageVM_AvailableRocket> AvailableRockets
        {
            get
            {
                var available = new ObservableCollection<AddRocketsPageVM_AvailableRocket>();
                var types = Enum.GetValues(typeof(Rocket.RocketType));
                foreach (Rocket.RocketType type in types)
                {
                    Rocket r = Rocket.GetRocketForType(type);
                    if (r.Available
                      && r.GetMaxPayload(GetDifficulty()) > 0)
                    {
                        available.Add(new AddRocketsPageVM_AvailableRocket(r, GetDifficulty(), this));
                    }
                }
                return available;
            }
        }

        public static Task<MissionStageSolution> Go(INavigation navigation, MissionStageSolution solution)
        {
            var page = new AddRocketsPage(solution);
            page.ViewModel.navigation = navigation;
            navigation.PushModalAsync(page);
            return page.ViewModel.retval.Task;
        }

        public AddRocketsPageVM(MissionStageSolution sol, INavigation nav)
        {
            Solution = new MissionStageSolution(sol); //make a copy so that if we cancel we haven't changed the original
            navigation = nav;
            RequiredPayload = sol.GetMissionStage().Payload;
            CurrentCapacity = sol.CalculateCapacity();
            AddRocketCommand = new Command<string>(AddRocket);
            RemoveRocketCommand = new Command<string>(RemoveRocket, HasRocketsOfType);
            OKCommand = new Command(OKPressed, RequirementsMet);
            CancelCommand = new Command(CancelPressed);
        }

        #region commands
        public Command AddRocketCommand { get; }
        public Command RemoveRocketCommand { get; }
        public Command OKCommand { get; }
        public Command CancelCommand { get; }

        private DifficultyLevel GetDifficulty()
        {
            return Solution.GetMissionStage().Difficulty;
        }

        private bool HasRocketsOfType(string rocketNameStr)
        {
            if (rocketNameStr == null)
            {
                return false;
            }
            Rocket.RocketType rocketType = Rocket.GetRocketType(rocketNameStr);
            return Solution.UsedRockets[rocketType] > 0;
        }

        private bool RequirementsMet()
        {
            return Solution.IsSufficient();
        }

        private void AddRocket(string rocketNameStr)
        {
            Rocket.RocketType rocketType = Rocket.GetRocketType(rocketNameStr);
            Solution.AddRocket(rocketType);            
            CurrentCapacity += Rocket.GetMaxPayload(rocketType, GetDifficulty());
            Update();
        }

        private void RemoveRocket(string rocketNameStr)
        {
            Rocket.RocketType rocketType = Rocket.GetRocketType(rocketNameStr);
            Solution.RemoveRocket(rocketType);
            CurrentCapacity -= Rocket.GetMaxPayload(rocketType, GetDifficulty());
            Update();
        }

        private void Update()
        {
            OnPropertyChanged(nameof(SolutionRocketList));
            OnPropertyChanged(nameof(CurrentCapacity));
            OnPropertyChanged(nameof(CapacityColour));
            //foreach (AddRocketsPageVM_AvailableRocket availableRocket in AvailableRockets)
            //{
            //    availableRocket.Update();
            //}
            OnPropertyChanged(nameof(AvailableRockets));
            RemoveRocketCommand.ChangeCanExecute();
            OKCommand.ChangeCanExecute();
        }

        private async void OKPressed()
        {
            await navigation.PopModalAsync();
            retval.SetResult(Solution);
        }

        private async void CancelPressed()
        {
            await navigation.PopModalAsync();
            retval.SetResult(null);
        }
        #endregion

        protected short GetNumRocketsUsed(Rocket.RocketType type)
        {
            return Solution.UsedRockets[type];
        }

        public class AddRocketsPageVM_AvailableRocket// : INotifyPropertyChanged
        {
            //public event PropertyChangedEventHandler PropertyChanged;

            //protected void OnPropertyChanged([CallerMemberName] string name = "")
            //{
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //}

            private Rocket rocket;
            private DifficultyLevel difficulty;
            private AddRocketsPageVM viewModel;

            public AddRocketsPageVM_AvailableRocket(Rocket r, DifficultyLevel d, AddRocketsPageVM vm)
            {
                rocket = r;
                difficulty = d;
                viewModel = vm;
            }

            public string NameAndCapacity
            {
                get
                {
                    return rocket.Name + " (" + rocket.GetMaxPayload(difficulty).ToString("#.##") + "T)";
                }
            }
            
            public string Name { get { return rocket.Name; } }//needed for the add/remove command parameter

            public short NumberUsed
            {
                get { return viewModel.GetNumRocketsUsed(rocket.Type); }
            }

            public FormattedString FormattedDescription
            {
                get
                {
                    return new FormattedString
                    {
                        Spans =
                        {
                            new Span{Text = NameAndCapacity, FontAttributes=FontAttributes.Bold},
                            new Span{Text=Environment.NewLine },
                            new Span{Text=CostAndMass }
                        }
                    };
                }
            }

            public string CostAndMass
            {
                get
                {
                    return "Cost $" + rocket.Cost + " Mass " + rocket.Mass + "T";
                }
            }

            //public void Update()
            //{
            //    OnPropertyChanged(nameof(NumberUsed));
            //}
        }
    }
}
