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
        }
    }
}