using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Plugin.Permissions;
using Android.Content.PM;

namespace Clima.Droid
{
	[Activity (Label = "Clima", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : FormsAppCompatActivity
    {

		protected override void OnCreate (Bundle bundle)
		{
			
		    base.OnCreate (bundle);

		    Forms.Init(this, bundle);
		
		    LoadApplication(new App());
		}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}


