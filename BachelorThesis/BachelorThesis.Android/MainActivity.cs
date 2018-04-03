﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;
using NControl.Controls.Droid;
using NControl.Droid;


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
            CachedImageRenderer.Init(false);
            NControlViewRenderer.Init();
            NControls.Init();
          //  CachedImageRenderer.Init();

            LoadApplication(new App());
        }
    }
}

