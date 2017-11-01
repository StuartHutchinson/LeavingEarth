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
    public partial class MissionStagePage : ContentPage
    {
        public MissionStagePageVM ViewModel { get; set; }

        //public MissionStagePage() : this(new MissionStage()) {}

        public MissionStagePage(MissionStage stage)
        {
            BindingContext = ViewModel = new MissionStagePageVM(stage);            
            InitializeComponent();
            MessagingCenter.Subscribe<MissionStagePageVM>(this, Message.ShoppingListReset, ShoppingListReset);
        }
        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<MissionStagePageVM>(this, Message.ShoppingListReset);
        }
        private async void ShoppingListReset(MissionStagePageVM vm)
        {
            bool proceed = await DisplayAlert("Shopping List", "Changing the rockets will reset the shopping list for this mission", "OK", "Cancel");
            if (proceed)
            {
                vm.ReallyAddRockets();
            }
        }
    }
}