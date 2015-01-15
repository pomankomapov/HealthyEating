
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
    [Activity(Label = "Приемы пищи")]			
    public class FoodActivity : Activity
    {
		static HEDB database = new HEDB();
		protected ListView eatingList;
		protected ImageButton btn_neweating;          // Кнопка добавления приема пищи
		protected ImageButton btn_back;				  // Кнопка возврата к предыдущему активити
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			///////////////////////////////////////////////////////////////////////
            // Ставим нужный layout ///////////////////////////////////////////////
			///////////////////////////////////////////////////////////////////////

            SetContentView (Resource.Layout.foodlist);


			///////////////////////////////////////////////////////////////////////
            // Получаем все контролы //////////////////////////////////////////////
			///////////////////////////////////////////////////////////////////////

			eatingList = FindViewById<ListView> (Resource.Id.EatingList);
			btn_neweating = FindViewById<ImageButton>(Resource.Id.button_neweating);
			btn_back = FindViewById<ImageButton>(Resource.Id.button_back);

			///////////////////////////////////////////////////////////////////////
			// Инициализация компонентов + Собираем View //////////////////////////
			///////////////////////////////////////////////////////////////////////

			// Текущий день
			DateTime today = DateTime.Today;
			System.Console.Out.WriteLine (today.ToString());

			//Ищем текущий день питания
			EatingDay eatingToday = database.getEatingDay_by_Date (today);
			//Если в базе нет текущего дня, то его следует добавить
			if (eatingToday == null) {
				eatingToday = database.addEatingDay (today);
			}

			///// Временное добавление приемов пищи
			/*
			List<FoodPortion> fp_list1 = new List<FoodPortion> ();
			List<FoodPortion> fp_list2 = new List<FoodPortion> ();
			List<FoodPortion> fp_list3 = new List<FoodPortion> ();
			List<FoodPortion> fp_list4 = new List<FoodPortion> ();
			var eating1 = database.addEating (new DateTime (2015, 1, 1, 12, 00, 00), false);
			var eating2 = database.addEating (new DateTime (2015, 1, 1, 13, 00, 00), false);
			var eating3 = database.addEating (new DateTime (2015, 1, 1, 15, 30, 00), false);
			var eating4 = database.addEating (new DateTime (2015, 1, 1, 18, 00, 00), false);

			var food1 = database.getFood ("Яблоко");
			var food2 = database.getFood ("Абрикос");
			var food3 = database.getFood ("Молоко");
			var food4 = database.getFood ("Молоко1");
			var food5 = database.getFood ("Абрикос2");
			var food6 = database.getFood ("Яблоко1");

			fp_list1.Add (database.addFoodPortion (food1.ID, 100));
			fp_list1.Add (database.addFoodPortion (food2.ID, 100));
			fp_list2.Add (database.addFoodPortion (food3.ID, 100));
			fp_list2.Add (database.addFoodPortion (food2.ID, 150));
			fp_list3.Add (database.addFoodPortion (food2.ID, 100));
			fp_list4.Add (database.addFoodPortion (food3.ID, 100));

			database.addFoodPortionList (fp_list1, eating1);
			database.addFoodPortionList (fp_list2, eating2);
			database.addFoodPortionList (fp_list3, eating3);
			database.addFoodPortionList (fp_list4, eating4);

			database.addEatingList (eatingToday.mealPlaneID, EatingType.breakfast, eating1.ID);
			database.addEatingList (eatingToday.mealPlaneID, EatingType.nosh, eating2.ID);
			database.addEatingList (eatingToday.mealPlaneID, EatingType.dinner, eating3.ID);
			database.addEatingList (eatingToday.mealPlaneID, EatingType.lunch, eating4.ID);
			*/
			//Ищем все приемы пищи по плану питания на текущий день
			var eatings_list = database.getEatingList_by_MealPlaneID(eatingToday.mealPlaneID);

			List<String> eatings_array = new List<String>();
			//Заполняем ItemList
			foreach(var item in eatings_list) {
				var result_string = database.EatingTypeRus[item.eatingType];
				var foodPortionsList = database.getFoodPortionList_by_EatingID (item.eatingID);
				foreach (var _foodpl in foodPortionsList) {
					var _foodPortion = database.getFoodPortion (_foodpl.portionID);
					var _food = database.getFood (_foodPortion.foodID);
					result_string += "\n  " + _food.name + " - " + _foodPortion.count.ToString() + "г.";
				}
				eatings_array.Add (result_string);
			}

			ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Resource.Layout.default_ListItem, eatings_array);
			eatingList.Adapter = adapter;


			// Обработка событий /////////////////////////////////////////////////////////////

			btn_neweating.Click += (sender, e) =>
			{
				NewEating();
			};

			btn_back.Click += (sender, e) =>
			{
				base.OnBackPressed();
			};


            // Задачи во время запуска активности /////////////////////////////////
        }

		protected void NewEating()
		{
			Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);

			AlertDialog alertdialog = builder.Create();
			alertdialog.SetTitle("Выберите тип");

			alertdialog.SetButton("Close", (s, e) => {
				//StartActivity(typeof(ProfileActivity));
			});
			/*
			alertdialog.SetButton2(database.EatingTypeRus [(int)EatingType.dinner], (s, e) => {
				//StartActivity(typeof(ProfileActivity));
			});

			alertdialog.SetButton3(database.EatingTypeRus [(int)EatingType.lunch], (s, e) => {
				//StartActivity(typeof(ProfileActivity));
			});*/

			alertdialog.Show();
			alertdialog.SetContentView (Resource.Layout.food_types_dialog);

			Button food_type_button1 = alertdialog.FindViewById<Button>(Resource.Id.food_type_button1);
			Button food_type_button2 = alertdialog.FindViewById<Button>(Resource.Id.food_type_button2);
			Button food_type_button3 = alertdialog.FindViewById<Button>(Resource.Id.food_type_button3);
			Button food_type_button4 = alertdialog.FindViewById<Button>(Resource.Id.food_type_button4);

			food_type_button1.Text = database.EatingTypeRus [(int)EatingType.breakfast];
			food_type_button2.Text = database.EatingTypeRus [(int)EatingType.dinner];
			food_type_button3.Text = database.EatingTypeRus [(int)EatingType.lunch];
			food_type_button4.Text = database.EatingTypeRus [(int)EatingType.nosh];

			food_type_button1.Click += (sender, e) =>
			{
				alertdialog.Dismiss();
				addEatingShow();
			};

			food_type_button2.Click += (sender, e) =>
			{
				alertdialog.Dismiss();
				addEatingShow();
			};
				
			food_type_button3.Click += (sender, e) =>
			{
				alertdialog.Dismiss();
				addEatingShow();
			};

			food_type_button4.Click += (sender, e) =>
			{
				alertdialog.Dismiss();
				addEatingShow();
			};
		}

		protected void addEatingShow() {
			StartActivity(typeof(addEatingActivity));
		}
    }
}

