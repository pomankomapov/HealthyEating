
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
    [Activity(Label = "FoodActivity")]			
    public class FoodActivity : Activity
    {
		static HEDB database = new HEDB();
		protected ListView eatingList;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Ставим нужный layout ///////////////////////////////////////////////

            SetContentView (Resource.Layout.foodlist);

            // Получаем все контролы //////////////////////////////////////////////
			eatingList = FindViewById<ListView> (Resource.Id.EatingList);

            // Собираем View /////////////////////////////////////////////////

			var foodList = database.getAllFood ();
			int fl_count = foodList.Count ();
			List<String> foodArray = new List<String>();

			foreach(var item in foodList) {
				foodArray.Add (item.name);
			}

			ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Resource.Layout.default_ListItem, foodArray);
			eatingList.Adapter = adapter;

            //lsv_food.Adapter = new AdapterView();
            //ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemChecked, items);

            // Задачи во время запуска активности /////////////////////////////////
        }
    }
}

