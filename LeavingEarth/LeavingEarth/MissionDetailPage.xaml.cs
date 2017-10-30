using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LeavingEarth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MissionDetailPage : ContentPage
    {
        public MissionDetailPage(Mission m)
        {
            InitializeComponent();
            MessagingCenter.Subscribe<MissionDetailPageVM>(this, Message.BlankStageName, BlankStageName);
            MessagingCenter.Subscribe<MissionDetailPageVM>(this, Message.DuplicateStageName, DuplicateStageName);
            BindingContext = new MissionDetailPageVM(m, Navigation);
        }

        async void BlankStageName(MissionDetailPageVM vm)
        {
            await DisplayAlert("Error", "You must enter a stage name", "OK");
        }
        async void DuplicateStageName(MissionDetailPageVM vm)
        {
            await DisplayAlert("Error", "This mission already contains a stage with that name", "OK");
        }
    }
}