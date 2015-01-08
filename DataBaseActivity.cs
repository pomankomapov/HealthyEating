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
	[Activity (Label = "База данных")]			
	public class DataBaseActivity : Activity
	{
        static HEDB database = new HEDB();

        protected TextView     edt_food;
        protected TextView     edt_proteins;
        protected TextView     edt_fats;
        protected TextView     edt_carbs;
        protected TextView     edt_calories;
        protected Button       btn_select;
        protected Button       btn_save;
        protected ProgressBar  prb_saving;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Ставим нужный layout ///////////////////////////////////////////////

            SetContentView (Resource.Layout.db);

            // Получаем все контролы //////////////////////////////////////////////

            edt_food     = FindViewById<TextView>(Resource.Id.editText_db_food);
            edt_proteins = FindViewById<TextView>(Resource.Id.editText_db_proteins);
            edt_fats     = FindViewById<TextView>(Resource.Id.editText_db_fats);
            edt_carbs    = FindViewById<TextView>(Resource.Id.editText_db_carbs);
            edt_calories = FindViewById<TextView>(Resource.Id.editText_db_calories);
            btn_select   = FindViewById<Button>(Resource.Id.button_db_select);
            btn_save     = FindViewById<Button>(Resource.Id.button_db_save);
            prb_saving   = FindViewById<ProgressBar>(Resource.Id.progressBar_db);

            // Назначаем действия /////////////////////////////////////////////////

            btn_select.Click += (sender, e) => {
                StartActivity(typeof(ListFood));
            };

            btn_save.Click += (sender, e) => {
                prb_saving.Visibility = ViewStates.Visible;
                prb_saving.Enabled = true;

                update_food();

                prb_saving.Enabled = false;
                prb_saving.Visibility = ViewStates.Invisible;
            };
		}

        protected override void OnResume()
        {
            // Always call the superclass first
            base.OnResume();

            // Если пользователь что-то выбрал
            if (Global.choosed_food_ID == int.MaxValue)
                return;

            // То достаём из базы и пишем в контролы
            Food food = database.getFood(Global.choosed_food_ID);

            edt_food.Text = food.name;
            edt_proteins.Text = string.Format("{0}", food.proteins);
            edt_fats.Text = string.Format("{0}", food.fats);
            edt_carbs.Text = string.Format("{0}", food.carbs);
            edt_calories.Text = string.Format("{0}", food.calories);
        }

        protected void update_food()
        {
            // Считываем значения и проверяем правильность ввода ///////////////////////////////////

            string name = edt_food.Text.ToLower().Trim();

            // Проверки названия на корректность
            if (string.IsNullOrEmpty(name))
            {
                Global.print(this, "Введите название продукта");
                return;
            }

            int p, f, c; // Proteins, fats, carbs
            int calories;
            p = f = c = calories = -1;

            bool success = true;
            success &= int.TryParse(edt_proteins.Text, out p);
            success &= int.TryParse(edt_fats.Text,     out f);
            success &= int.TryParse(edt_carbs.Text,    out c);
            success &= int.TryParse(edt_calories.Text, out calories);

            // Проверки на разумные значения
            if (!success || p < 0 || f < 0 || c < 0 || calories < 0)
            {
                Global.print(this, "Не все поля заполнены правильно");
                return;
            }

            if (p > 100)
            {
                Global.print(this, "Количество белков выше разумной нормы");
                return;
            }

            if (f > 100)
            {
                Global.print(this, "Количество жиров выше разумной нормы");
                return;
            }

            if (c > 100)
            {
                Global.print(this, "Количество углеводов выше разумной нормы");
                return;
            }

            // Ищем продукт в базе ///////////////////////////////////////////////////////////////

            string food_name = name.ToLower();
            Food food = database.findFood(food_name);

            // Если продукт существует, то удаляем его
            if (food != null)
                database.delFood(food.ID);

            // Создаём запись по введённым данным
            database.addFood(food_name, p, f, c, calories, Global.userID);

            Global.print(this, string.Format("Добавлен продукт {0}", name));
        }
	}
}

