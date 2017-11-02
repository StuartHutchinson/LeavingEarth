
using Android.App;
using Android.Content.PM;
using Android.OS;
using System.IO;

namespace LeavingEarth.Droid
{
    [Activity(Label = "LeavingEarth", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Config.PathApp = Path.Combine(documentsPath, "LeavingEarth");
            if (!System.IO.Directory.Exists(Config.PathApp))
            {
                System.IO.Directory.CreateDirectory(Config.PathApp);
            }

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //public override bool OnPrepareOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.top_menus, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
        //        ToastLength.Short).Show();
        //    return base.OnOptionsItemSelected(item);
        //}
    }
}

