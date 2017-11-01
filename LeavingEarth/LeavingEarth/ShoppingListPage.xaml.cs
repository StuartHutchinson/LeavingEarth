
using Messier16.Forms.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LeavingEarth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShoppingListPage : ContentPage
	{
		public ShoppingListPage (Mission m)
		{
            BindingContext = new ShoppingListVM(m);
			InitializeComponent ();
		}
	}
}