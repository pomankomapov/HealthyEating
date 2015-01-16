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
using Android;

namespace healthy_eating
{
	[Activity (Label = "Редактирование списка продуктов")]			
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
        protected Button       btn_delete;
        protected Button       btn_destroy;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Убираем автофокус
            this.Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

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
            btn_delete   = FindViewById<Button>(Resource.Id.button_db_delete);
            btn_destroy  = FindViewById<Button>(Resource.Id.button_db_destroy);

            // Назначаем действия /////////////////////////////////////////////////

            btn_select.Click += (sender, e) => {
                StartActivity(typeof(ListFood));
            };

            btn_save.Click += (sender, e) => {
                update_food();
            };

            btn_delete.Click += (sender, e) => {
                delete_food();
            };

            btn_destroy.Click += (sender, e) => {
                destroy_food();
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
            if (food == null)
                return;

            edt_food.Text = food.name;
            edt_proteins.Text = string.Format("{0}", food.proteins);
            edt_fats.Text = string.Format("{0}", food.fats);
            edt_carbs.Text = string.Format("{0}", food.carbs);
            edt_calories.Text = string.Format("{0}", food.calories);
        }

        protected void update_food()
        {
            bool was = false; // Продукт был в базе

            // Считываем значения и проверяем правильность ввода ///////////////////////////////////

            string name = edt_food.Text.ToLower().Trim();

            // Проверки названия на корректность
            if (string.IsNullOrEmpty(name))
            {
                Global.print(this, "Введите название продукта");
                return;
            }

            float p, f, c; // Proteins, fats, carbs
            float calories;
            p = f = c = calories = -1;

            bool success = true;
            success &= float.TryParse(edt_proteins.Text, out p);
            success &= float.TryParse(edt_fats.Text,     out f);
            success &= float.TryParse(edt_carbs.Text,    out c);
            success &= float.TryParse(edt_calories.Text, out calories);

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
            {
                database.delFood(food.ID);
                was = true;
            }

            // Создаём запись по введённым данным
            database.addFood(food_name, p, f, c, calories, Global.userID);

            if (was) Global.print(this, string.Format("Изменён продукт {0}", name));
            else     Global.print(this, string.Format("Добавлен продукт {0}", name));
        }

        protected void delete_food()
        {
            string name = edt_food.Text.ToLower().Trim();

            // Проверки названия на корректность
            if (string.IsNullOrEmpty(name))
            {
                Global.print(this, "Такого продукта нет");
                return;
            }

            // Ищем продукт в базе
            Food food = database.findFood(name);

            if (food == null)
            {
                Global.print(this, "Такого продукта нет");
                return;
            }

            Global.print(this, string.Format("Удалён продукт {0}", name));
            database.delFood(food.ID);
            Global.choosed_food_ID = int.MaxValue; // Убираем выбор пользователя
        }

        protected void destroy_food()
        {
            Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alertdialog = builder.Create();
            alertdialog.SetTitle("Вы уверены?");
            alertdialog.SetMessage("Очистку списка продуктов нельзя отменить");
            alertdialog.SetButton("Да", (s, e) =>
            {
                    database.delAllFood();
            });

            alertdialog.SetButton2("Нет", (s, e) =>
            {
                    // Фууууфф..)
            });

            alertdialog.Show();
        }
	}
}

