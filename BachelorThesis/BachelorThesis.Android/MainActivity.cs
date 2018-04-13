using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Iconize;
using Xamarin.Forms;


namespace BachelorThesis.Droid
{
    [Activity(Label = "BachelorThesis", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {



        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            //   TwinTechsForms.NControl.SvgImageView.Init();
            LoadApplication(new App());


            //allowing the device to change the screen orientation based on the rotation
            MessagingCenter.Subscribe<Views.ProcessVisualisationPage>(this, "setLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });

            //during page close setting back to portrait
            MessagingCenter.Subscribe<Views.ProcessVisualisationPage>(this, "unlockOrientation", sender =>
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            });
        }
    }
}

