
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
            MessagingCenter.Subscribe<Mission>(this, Message.BlankMissionName, BlankMissionName);
            MessagingCenter.Subscribe<Mission>(this, Message.DuplicateMissionName, DuplicateMissionName);
        }

        async void BlankMissionName(Mission m)
        {
            await DisplayAlert("Error", "You must enter a mission name", "OK");
        }
        async void DuplicateMissionName(Mission m)
        {
            await DisplayAlert("Error", "There is already a mission with that name", "OK");
        }
    }
}