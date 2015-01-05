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

        protected void update_food()
        {
            // Сначала ищем в базе продукт
            // TODO: написать подобие хеш-функции
            //      для определения похожих названий

            string name = edt_food.Text.Trim();

            // Проверки названия на корректность
            if (string.IsNullOrEmpty(name))
            {
                Global.print(this, "Введите название продукта");
                return;
            }

            int p, f, c; // Proteins, fats, carbs
            int calories;
            p = f = c = calories = 0;

            int.TryParse(edt_proteins.Text, out p);
            int.TryParse(edt_fats.Text,     out f);
            int.TryParse(edt_carbs.Text,    out c);
            int.TryParse(edt_calories.Text, out calories);

            // Проверки на разумные значения
            if (p < 0 || f < 0 || c < 0 || calories < 0)
            {

            }
        }
	}
}

