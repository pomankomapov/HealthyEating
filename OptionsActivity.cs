
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace healthy_eating
{
	[Activity (Label = "Настройки")]			
	public class OptionsActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.options);
		}
	}
}

