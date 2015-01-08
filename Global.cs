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

