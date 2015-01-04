using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony;
using Java.Util;

namespace healthy_eating
{
    [Activity (Label = "Healthy Eating", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        static HEDB database = new HEDB();
        static int profileID = int.MaxValue;   // ID профиля пользователя
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

            Global.deviceID = "noneID";
            Global.userID = int.MaxValue;

            profileID = find_main_profile();
            if (profileID == int.MaxValue) // Профиля нет
                StartActivity(typeof(ProfileActivity));
			fill_data ();
            HEDB database = new HEDB();

            database.delAll(); // Временно
            database.delAllFood();
            database.addFood(0, "Яблоко", 45, 0, 70, 100, 3);
            database.addFood(1, "Абрикос", 45, 0, 70, 100, 3);
            database.addFood(2, "Молоко", 45, 0, 70, 100, 3);
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

        protected string get_phone_id()
        {
            // Получаем уникальный номер устройства
            string deviceID;
            var telephonyDeviceID = string.Empty;
            var telephonySIMSerialNumber = string.Empty;

            try
            {
                TelephonyManager telephonyManager = (TelephonyManager)this.ApplicationContext.GetSystemService(Context.TelephonyService);
                if (telephonyManager != null)
                {
                    if(!string.IsNullOrEmpty(telephonyManager.DeviceId))
                        telephonyDeviceID = telephonyManager.DeviceId;
                    if(!string.IsNullOrEmpty(telephonyManager.SimSerialNumber))
                        telephonySIMSerialNumber = telephonyManager.SimSerialNumber;
                }
                var androidID = Android.Provider.Settings.Secure.GetString(this.ApplicationContext.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
                var deviceUuid = new UUID(androidID.GetHashCode(), (((long)telephonyDeviceID.GetHashCode()) << 32) | telephonySIMSerialNumber.GetHashCode());
                deviceID = deviceUuid.ToString();
            }
            catch
            {
                deviceID = "";
                Android.Widget.Toast.MakeText(this, "Не удалось найти device ID", Android.Widget.ToastLength.Short).Show();
            }

            //Android.Widget.Toast.MakeText(this, deviceID, Android.Widget.ToastLength.Short).Show();

            return deviceID;
        }

        /// <summary>
        /// Находит ID профиля зарегистрированного на это устройство, сохраняя в Global
        /// </summary>
        /// <returns>Номер профиля</returns>
        protected int find_main_profile()
        {
            string deviceID = get_phone_id();
            if (string.IsNullOrEmpty(deviceID))
                return int.MaxValue;

            // Запись в глобальное пространство переменных
            Global.deviceID = deviceID;

            Profile profile = database.getProfileByDevice(deviceID);
            if (profile == null)
                return int.MaxValue;

            // Запись в глобальное пространство переменных
            Global.userID = profile.ID;

            return profile.ID;
        }
	}
}


