using System;
using Android.Content;
using Android.Widget;

namespace healthy_eating
{
    // Глобальное пространство переменных
    //  доступно из любого активити

    public static class Global
    {
        public static int userID { get; set; }       // ID пользователя этого устройства
        public static string deviceID { get; set; }  // ID устройства

        public static int choosed_food_ID { get; set; } // ID выбранной еды из базы
		public static int choosed_eating_ID { get; set; } // ID выбранного приема пищи
		public static EatingType choosed_eating_type { get; set; } // тип выбранного приема пищи
		public static int choosed_mealPlain_ID { get; set; } // ID текущего плана питания

		public static float percent1;  // Калорий в день потреблено
		public static float percent2;  // Воды поглощено

		public static float linechart1; // Белки
		public static float linechart2; // Жиры
		public static float linechart3; // Углеводы

        public static void print(Android.Content.Context context, string str)
        {
            Android.Widget.Toast.MakeText(context, str, ToastLength.Short).Show();
        }

        public static string val2str(int value)
        {
            if (value != 0)
                return string.Format("{0}", value);
            return "--";
        }
    }
}

