using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace LeavingEarth
{
    class MainPageVM : BaseVM
    {
        public Command ViewMissionListCommand { get; }
        public Command ViewAvailableRocketsCommand { get; }
        public INavigation Navigation;

        public MainPageVM(INavigation nav)
        {
            Navigation = nav;
            ViewMissionListCommand = new Command(ViewMissionList);
            ViewAvailableRocketsCommand = new Command(ViewAvailableRockets);
        }
        
        private async void ViewMissionList()
        {
            await Navigation.PushAsync(new MissionPage());
        }

        private async void ViewAvailableRockets()
        {
            await Navigation.PushAsync(new AvailableRocketsPage());
        }
    }
}
