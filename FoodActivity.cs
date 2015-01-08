
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
        protected ListView lsv_food;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Ставим нужный layout ///////////////////////////////////////////////

            SetContentView (Resource.Layout.foodlist);

            // Получаем все контролы //////////////////////////////////////////////

            lsv_food = FindViewById<ListView>(Resource.Id.listView_food);

            // Назначаем действия /////////////////////////////////////////////////

            //lsv_food.Adapter = new AdapterView();
            //ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemChecked, items);

            // Задачи во время запуска активности /////////////////////////////////
        }
    }
}

