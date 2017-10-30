
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LeavingEarth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MissionPage : ContentPage
    {
        //Mission mission = null;

        public MissionPage()
        {
            BindingContext = new MissionPageVM(Navigation);            
            InitializeComponent();
            MessagingCenter.Subscribe<MissionPageVM>(this, Message.BlankMissionName, BlankMissionName);
            MessagingCenter.Subscribe<MissionPageVM>(this, Message.DuplicateMissionName, DuplicateMissionName);
        }

        async void BlankMissionName(MissionPageVM vm)
        {
            await DisplayAlert("Error", "You must enter a mission name", "OK");
        }
        async void DuplicateMissionName(MissionPageVM vm)
        {
            await DisplayAlert("Error", "There is already a mission with that name", "OK");
        }
    }
}