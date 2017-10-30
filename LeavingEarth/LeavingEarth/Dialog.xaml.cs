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
    public partial class Dialog : ContentPage
    {
        private const string BUTTON_OK = "OK";
        private const string BUTTON_CANCEL = "Cancel";

        string dialogText = "";
        string inputText = "";
        string originalInputText = "";
        List<Button> buttons = new List<Button>();

        private Dialog()
        {
            InitializeComponent();
        }

        public static Dialog CreateMessage(string message)
        {
            Dialog d = new Dialog();
            d.entryBox.IsVisible = false;
            d.dialogText = message;
            d.ButtonCancel.IsVisible = false;
            return d;
        }

        public static Dialog CreateEntry(string message, ref string input)
        {
            Dialog d = new Dialog
            {
                inputText = new string(input.ToCharArray()),
                originalInputText = input,
                dialogText = message
            };
            return d;
        }

        public static Task<string> InputBox(INavigation navigation, string title, string message, string text)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = title, HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = message, HorizontalOptions=LayoutOptions.Center };
            var txtInput = new Entry { Text = text };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = txtInput.Text;
                await navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                await navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(null);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }

        private void ButtonClicked(ListView list, EventArgs args)
        {
            Button b = (Button)list.SelectedItem;
            switch(b.Text)
            {
                case BUTTON_OK: SaveData();
                    break;
                case BUTTON_CANCEL: Close();
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Unknown button " + b.Text);
                    break;
            }
        }

        private void SaveData()
        {
            originalInputText = inputText;
            Close();
        }

        private async void Close()
        {
            await Navigation.PopModalAsync();
        }
    }
}