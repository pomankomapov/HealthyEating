
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
        protected Button btn_db_options;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Ставим нужный layout ///////////////////////////////////////////////

			SetContentView (Resource.Layout.options);

            // Получаем все контролы //////////////////////////////////////////////

            btn_db_options = FindViewById<Button>(Resource.Id.button_db_options);

            // Назначаем действия /////////////////////////////////////////////////

            btn_db_options.Click += (sender, e) =>{
                StartActivity(typeof(DataBaseActivity));
            };
		}
	}
}

