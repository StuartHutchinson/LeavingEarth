using Xamarin.Forms;

namespace LeavingEarth
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = new MainPageVM(Navigation);
            InitializeComponent();
        }
    }
}
