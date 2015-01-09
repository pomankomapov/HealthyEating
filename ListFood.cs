using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace healthy_eating
{
    [Activity(Label = "Список продуктов", MainLauncher = false, Icon = "@drawable/icon")]
    public class ListFood : ListActivity
    {
        static HEDB database = new HEDB();
        protected List<string> items = new List<string>();
        protected string mode;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Получаем информацию от вызывающего
            mode = Intent.GetStringExtra("mode") ?? "none";

            List<Food> foods = database.getAllFood();

            foreach (var food in foods)
            {
                items.Add(string.Format("{0,-24}", food.name));
            }

            ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            string name = items[position];
            Global.print(this, name);

            Food food = database.findFood(name);

            // Сохраняем выбор пользователя
            if (food != null)
                Global.choosed_food_ID = food.ID;

            // Если выбор аллергенного продукта
            if (mode.Equals("Allergic"))
            {
                if (Global.choosed_food_ID != int.MaxValue)
                {
                    // Выбран аллергенный продукт
                    //  добавляем в базу
                    database.addAllergic(Global.userID, Global.choosed_food_ID);
                }
            }

            // Возвращаемся к предыдущему активити
            base.OnBackPressed();
        }
    }
}

