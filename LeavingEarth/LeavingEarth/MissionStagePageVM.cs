using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LeavingEarth
{
    public class MissionStagePageVM : BaseVM
    {
        private MissionStage Stage;
        private TaskCompletionSource<MissionStage> retval = new TaskCompletionSource<MissionStage>();
        INavigation Navigation;
        public Command RenameStageCommand { get; }
        public Command DeleteStageCommand { get; }
        public ICommand SaveStageCommand { get; }
        public ICommand DiscardStageCommand { get; }
        public ICommand AddRocketsCommand { get; }

        public MissionStagePageVM(MissionStage s)
        {
            Stage = s;
            RenameStageCommand = new Command(async () => await RenameStage());
            DeleteStageCommand = new Command(async () => await DeleteStage());
            SaveStageCommand = new Command(async () => await saveStage()); //TODO - do these need to await?
            DiscardStageCommand = new Command(async () => await discardStage());
            AddRocketsCommand = new Command(AddRockets);
        }

        private async Task RenameStage()
        {
            
        }

        private async Task DeleteStage()
        {

        }

        private async Task saveStage()
        {
            await Navigation.PopAsync();
            retval.SetResult(Stage);
        }

        private async Task discardStage()
        {
            await Navigation.PopAsync();
            retval.SetResult(null);
        }

        private async void AddRockets()
        {
            //await Navigation.PushModalAsync(new AddRocketsPage(Stage.Solution));
            var updatedSolution = await AddRocketsPageVM.Go(Navigation, Stage.Solution);
            if (updatedSolution != null)
            {
                Stage.Solution = updatedSolution;
                OnPropertyChanged(nameof(SolutionDescription));
                OnPropertyChanged(nameof(SolutionColour));
            }
        }
        
        public static Task<MissionStage> Go(INavigation nav, MissionStage stage)
        {
            // wait in this proc, until user did his input 
            //var tcs = new TaskCompletionSource<MissionStage>();

            //MissionStage stageCopy = new MissionStage(stage);

            var page = new MissionStagePage(stage);
            page.ViewModel.Navigation = nav;
            nav.PushAsync(page);

            return page.ViewModel.retval.Task;
        }

        public string StageDescription
        {
            get { return Stage.Description; }
            set { Stage.Description = value; }
        }

        public int DifficultyInt
        {
            get { return (int)Stage.Difficulty; }
            set { Stage.Difficulty = (DifficultyLevel)value; }
        }

        public List<int> DifficultyLevelInts
        {
            get
            {
                int[] DifficultyArray = (int[])Enum.GetValues(typeof(DifficultyLevel));
                return new List<int>(DifficultyArray);
            }
        }

        public short StagePayload
        {
            get { return Stage.Payload; }
            set { Stage.Payload = value; }
        }

        public string SolutionDescription
        {
            get { return Stage.Solution.Description; }
            set { Stage.Solution.Description = value; }
        }

        public Color SolutionColour { get { return Stage.Colour; } }
    }
}
