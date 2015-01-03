﻿using System;
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
	[Activity (Label = "База данных")]			
	public class DataBaseActivity : Activity
	{
        static HEDB database = new HEDB();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Ставим нужный layout ///////////////////////////////////////////////

            SetContentView (Resource.Layout.db);

            // Получаем все контролы //////////////////////////////////////////////


            // Назначаем действия /////////////////////////////////////////////////

            //database.getAllFood();

            StartActivity(typeof(ListFood));
		}
	}
}

