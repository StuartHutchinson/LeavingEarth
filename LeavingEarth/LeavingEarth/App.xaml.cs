using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace LeavingEarth
{
    public partial class App : Application
    {
        public static ObservableCollection<Mission> Missions { get; set; }

        public App()
        {
            InitializeComponent();            
            MainPage = new NavigationPage(new LeavingEarth.MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //Missions = DependencyService.Get<IFileIO>().LoadMissions(); //todo - fix this
            Missions = new ObservableCollection<Mission>();
        }

        protected override void OnSleep()
        {
            //DependencyService.Get<IFileIO>().SaveMissions(Missions); //todo - fix this
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
