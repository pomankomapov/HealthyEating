using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony;
using System.IO;
using Android.Content.PM;
using Java.IO;
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
        protected ImageButton btn_food;          // Кнопка пищи
		protected ImageButton btn_stats;         // Кнопка статистики
      
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Ставим главный layout //////////////////////////////////////////////

			SetContentView (Resource.Layout.main);

			// Получаем все контролы //////////////////////////////////////////////

            btn_profile = FindViewById<ImageButton>(Resource.Id.button_profile);
            btn_options = FindViewById<ImageButton>(Resource.Id.button_options);
            btn_food = FindViewById<ImageButton>(Resource.Id.button_food);
			btn_stats   = FindViewById<ImageButton>(Resource.Id.button_stats);

			// Назначаем действия /////////////////////////////////////////////////

			btn_profile.Click += (sender, e) =>
			{
				StartActivity(typeof(ProfileActivity));
			};

			btn_options.Click += (sender, e) =>
			{
                StartActivity(typeof(DataBaseActivity));
			};

            btn_food.Click += (sender, e) => {
                StartActivity(typeof(FoodActivity));
            };

			btn_stats.Click += (sender, e) => {
				StartActivity(typeof(StatisticsActivity));
			};

			// Задачи во время запуска активности /////////////////////////////////

            Global.deviceID = "noneID";
            Global.userID = int.MaxValue;
            Global.choosed_food_ID = int.MaxValue;
			Global.choosed_eating_ID = int.MaxValue;

            HEDB database = new HEDB();
            database.delAll(); // Временно
            database.delAllAllergic();

            profileID = find_main_profile();
            if (profileID == int.MaxValue) // Профиля нет
                StartActivity(typeof(ProfileActivity));
			fill_data();

			database.delAll(); // Всё удаляем
			init_db();
		} 



		protected void init_db()
		{
			// Получение пути к файлу
			/*
            PackageManager m = PackageManager;
            String s = PackageName;
            PackageInfo p = m.GetPackageInfo(s, 0);
            s = p.ApplicationInfo.DataDir;
            var path = System.IO.Path.Combine(s, "Assets/product.xml");
            */

			// Чтение файла из asserts и парсинг
			var file = Assets.Open("product.xml");
			//FileStream file = System.IO.File.Open(path, FileMode.Open);
			var reader = new StreamReader(file);
			var str = reader.ReadToEnd();
			parse_xml(str);
		}

		protected void parse_xml(string s)
		{
			string name, proteins, fats, carbs, calories;
			float p, f, c, cals;
			string[] items = s.Split(new string[] { "<item>" }, StringSplitOptions.None);

			foreach (string item in items)
			{
				name     = get_attr(item, "name").Replace ('.', ',');
				proteins = get_attr(item, "proteins").Replace ('.', ',');
				fats     = get_attr(item, "fats").Replace ('.', ',');
				carbs    = get_attr(item, "carbs").Replace ('.', ',');
				calories = get_attr(item, "calories").Replace ('.', ',');

				if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(proteins)
					||  string.IsNullOrEmpty(fats) || string.IsNullOrEmpty(carbs)
					||  string.IsNullOrEmpty(calories))
					continue;

				if (!float.TryParse(proteins, out p))
					continue;

				if (!float.TryParse(fats, out f))
					continue;

				if (!float.TryParse(carbs, out c))
					continue;

				if (!float.TryParse(calories, out cals))
					continue;

				// Debug
				System.Console.Out.WriteLine("{0} | {1}/{2}/{3} - {4}", name, proteins, fats, carbs, calories);

				database.addFood(name, p, f, c, cals, Global.userID);
			}
		}

		protected string get_attr(string str, string attr)
		{
			string s = string.Copy(str);
			string tag_open  = "<" + attr + ">";
			string tag_close = "</" + attr + ">";

			if (s.Contains(tag_open) && s.Contains(tag_close))
			{
				int start = s.IndexOf(tag_open, 0) + tag_open.Length;
				int end = s.IndexOf(tag_close, start);

				return s.Substring(start, end - start);
			}

			return "";
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


