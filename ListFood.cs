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
        List<string> items = new List<string>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            List<Food> foods = database.getAllFood();

            foreach (var food in foods)
            {
                items.Add(string.Format("{0,-12} | {1}/{2}/{3}", food.name, food.proteins, food.fats, food.carbs));
            }

            ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
        }
        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var t = items[position];
            Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
            Console.WriteLine("Clicked on " + t);
        }
    }
}

