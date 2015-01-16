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
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Util;

namespace healthy_eating
{
    [Activity(Label = "Статистика")]			
    public class StatisticsActivity : Activity
    {
        static HEDB database = new HEDB();
        protected float total_calories;      // Условно (надо бы вычислить точно)
        protected float total_water;         // Условно (надо бы вычислить точно)
        protected float p_max, f_max, c_max; // Максимальные бжу
        protected PieChart pie_stats;
        protected LinChart lin_stats;
        protected TextView txt_cals_water;
        protected TextView txt_pfc;



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Убираем автофокус
            this.Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

            // Ставим нужный layout ///////////////////////////////////////////////

            SetContentView (Resource.Layout.statistics);

            // Получаем все контролы //////////////////////////////////////////////

            pie_stats      = FindViewById<PieChart>(Resource.Id.PieChart1);
            lin_stats      = FindViewById<LinChart>(Resource.Id.LinChart1);
            txt_cals_water = FindViewById<TextView>(Resource.Id.text_cals_and_water);
            txt_pfc        = FindViewById<TextView>(Resource.Id.text_pfc_stats);

            // Назначаем действия /////////////////////////////////////////////////



            // Задачи во время запуска активности /////////////////////////////////

            Global.percent1 = 0.38f;
            Global.percent2 = 0.17f;
            Global.linechart1 = 600f;
            Global.linechart2 = 240f;
            Global.linechart3 = 870f;
            calculate_stats(); // Вычисляем всю нужную статистику (выводится на графики)
            write_description(); // Описываем в textView положение дел
        }

        protected void calculate_stats()
        {
            float p, f, c;
            float cals, water;

            p = f = c = cals = water = 0f;

            // Вычисляем требуемые показатели для пользователя
            p_max = 600;
            f_max = 600;
            c_max = 2400;
            total_calories = 3000f;
            total_water = 3; // Литра

            // Вычисляем по базе потреблённые продукты
            DateTime today = DateTime.Today;
            EatingDay eating_day = database.getEatingDay_by_Date(DateTime.Today);

            if (eating_day == null)
                return;

            List<EatingList> eating_lists = database.getEatingList_by_MealPlaneID(eating_day.mealPlaneID);

            if (eating_lists == null)
                return;

            foreach (var eating_list in eating_lists)
            {
                List<FoodPortionList> food_portion_lists = database.getFoodPortionList_by_EatingID(eating_list.eatingID);
                if (food_portion_lists == null)
                    return;

                foreach (var food_portion_list in food_portion_lists)
                {
                    FoodPortion food_portion = database.getFoodPortion(food_portion_list.portionID);
                    if (food_portion == null)
                        return;
                    int count = food_portion.count;

                    Food food = database.getFood(food_portion.foodID);
                    if (food == null)
                        return;
                    p += food.proteins * count / 100f;
                    f += food.fats     * count / 100f;
                    c += food.carbs    * count / 100f;

                    cals += food.calories * count / 100f;
                    //water += food.carbs * count / 100f;
                }
            }

            Global.percent1 = cals / total_calories;
            //Global.percent2 = water / total_water;
            Global.linechart1 = p / p_max;
            Global.linechart2 = f / f_max;
            Global.linechart3 = c / c_max;

        }

        protected void write_description()
        {
            string recomend;

            recomend = string.Format("Энергетическая ценность потреблённых вами продуктов составляет {0} Ккал. Было выпито {1:0.00} л воды, осталось ещё {2:0.00} л.",
                total_calories * Global.percent1, total_water * Global.percent2, total_water * (1 - Global.percent2));

            txt_cals_water.Text = recomend;

            txt_pfc.Text = ""; // TODO: сделай рекомендацию по бжу
        }
    }
}

