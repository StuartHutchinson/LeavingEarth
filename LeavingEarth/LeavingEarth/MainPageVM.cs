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
        public ICommand ViewMissionListCommand { get; }
        public INavigation Navigation;

        public MainPageVM(INavigation nav)
        {
            Navigation = nav;
            ViewMissionListCommand = new Command(ViewMissionList);
        }
        
        private async void ViewMissionList()
        {
            await Navigation.PushAsync(new MissionPage());
        }
    }
}
