using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace healthy_eating
{
	[Activity (Label = "Healthy Eating", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected Button    btn_profile;       // Кнопка профиля
		protected Button    btn_options;       // Кнопка настроек
		protected TextView  txt_stat_weight;   // Вес
		protected TextView  txt_stat_calories; // Калории
		protected TextView  txt_stat_pfc;      // БЖУ
		protected TextView  txt_stat_water;    // Вода
		protected TextView  txt_stat_training; // Упражнения

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Ставим главный layout //////////////////////////////////////////////

			SetContentView (Resource.Layout.main);

			// Получаем все контролы //////////////////////////////////////////////

			btn_profile       = FindViewById <Button>   (Resource.Id.button_profile);
			btn_options       = FindViewById <Button>   (Resource.Id.button_options);
			txt_stat_weight   = FindViewById <TextView> (Resource.Id.text_stat_weight);
			txt_stat_calories = FindViewById <TextView> (Resource.Id.text_stat_calories);
			txt_stat_pfc      = FindViewById <TextView> (Resource.Id.text_stat_pfc);
			txt_stat_water    = FindViewById <TextView> (Resource.Id.text_stat_water);
			txt_stat_training = FindViewById <TextView> (Resource.Id.text_stat_training);

			// Назначаем действия /////////////////////////////////////////////////

			btn_profile.Click += (sender, e) =>
			{
				StartActivity(typeof(ProfileActivity));
			};

			btn_options.Click += (sender, e) =>
			{
				StartActivity(typeof(OptionsActivity));
			};

			// Задачи во время запуска активности /////////////////////////////////

			fill_data ();
		}

		static string value_to_str(int value)
		{
			if (value != 0)
				return string.Format("{0}", value);
			return "--";
		}

		/// <summary>
		/// Заполняет данные о пользователе для вывода в активности
		/// </summary>
		protected void fill_data(int weight = 0, int calories = 0, int p = 0, int f = 0,
								 int c = 0, int water = 0, int training = 0)
		{
			string str_weight =  string.Format("");

			txt_stat_weight.Text = value_to_str(weight) + " кг";
			txt_stat_calories.Text = value_to_str(calories) + " Ккал";
			txt_stat_pfc.Text = value_to_str(p) + "/" + value_to_str(f) + "/" + value_to_str(c);
			txt_stat_water.Text = value_to_str(water) + " л";
			txt_stat_training.Text = value_to_str(-training) + " Ккал"; // Сжигание
		}
	}
}


