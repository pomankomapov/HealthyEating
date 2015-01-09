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
        static int profileID = int.MaxValue;     // ID профиля пользователя
        protected ImageButton btn_profile;       // Кнопка профиля
        protected ImageButton btn_options;       // Кнопка настроек
      
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Ставим главный layout //////////////////////////////////////////////

			SetContentView (Resource.Layout.main);

			// Получаем все контролы //////////////////////////////////////////////

            btn_profile = FindViewById<ImageButton>(Resource.Id.button_profile);
            btn_options = FindViewById<ImageButton>(Resource.Id.button_options);

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
            Global.choosed_food_ID = int.MaxValue;

            HEDB database = new HEDB();
            //database.delAll(); // Временно
            database.delAllAllergic();

            profileID = find_main_profile();
            if (profileID == int.MaxValue) // Профиля нет
                StartActivity(typeof(ProfileActivity));
			fill_data();

            database.delAllFood(); // Проверка работоспособности
            database.addFood("Яблоко", 45, 0, 70, 100, 3);
            database.addFood("Абрикос", 45, 0, 70, 100, 3);
            database.addFood("Молоко", 45, 0, 70, 100, 3);
		} 

		/// <summary>
		/// Заполняет данные о пользователе для вывода в активности
		/// </summary>
		protected void fill_data()
		{
            int weight, calories, p, f, c, water, training;
            weight = calories = p = f = c = water = training = 0;

            // Читаем из базы информацию по пользователю
            Profile profile = database.getProfile(Global.userID);
            if (profile != null)
            {
                weight = profile.current_weight;
                calories = 0;
                p = f = c = 0; // TODO: Где сохранять дневное потребление?
                water = 0;
                training = 0;
            }

			string str_weight = string.Format("");
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


