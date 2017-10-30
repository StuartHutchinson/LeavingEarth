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
    public partial class AddRocketsPage : ContentPage
    {
        public AddRocketsPageVM ViewModel { get; }
        public AddRocketsPage(MissionStageSolution solution)
        {
            InitializeComponent();
            BindingContext = ViewModel = new AddRocketsPageVM(solution, Navigation);
        }
    }
}