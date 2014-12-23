
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
	[Activity (Label = "Профиль")]
	public class ProfileActivity : Activity
	{
		protected EditText     edt_name;   // Имя
		protected RadioButton  rad_man;    // Пол мужской
		protected RadioButton  rad_woman;  // Пол женский
		protected EditText     edt_length; // Рост
		protected SeekBar      skb_length; // Прокрутка роста

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Ставим нужный layout ///////////////////////////////////////////////

			SetContentView (Resource.Layout.profile);

			// Получаем все контролы //////////////////////////////////////////////

			edt_name   = FindViewById <EditText>    (Resource.Id.edit_name);
			rad_man    = FindViewById <RadioButton> (Resource.Id.radio_man);
			rad_woman  = FindViewById <RadioButton> (Resource.Id.radio_woman);
			edt_length = FindViewById <EditText>    (Resource.Id.edit_length);
			skb_length = FindViewById <SeekBar>     (Resource.Id.seekbar_length);

			// Назначаем действия /////////////////////////////////////////////////

			skb_length.ProgressChanged += (sender, e) =>
			{
				edt_length.Text = string.Format("{0}", skb_length.Progress);
			};

			edt_length.KeyPress += (sender, e) =>
			{
				e.Handled = false;
				if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
				{
					int value;
					if (Int32.TryParse(edt_length.Text, out value))
					{
						if (value > skb_length.Max) edt_length.Text = string.Format("{0}", skb_length.Max);
						else if (value < 0)         edt_length.Text = "0";
						skb_length.Progress = value;
					}
					e.Handled = true;
				}
			};


			/*
			btn_options.Click += (sender, e) =>
			{
				StartActivity(typeof(OptionsActivity));
			};
			*/

			// Задачи во время запуска активности /////////////////////////////////
		}
	}
}

