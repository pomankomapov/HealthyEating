
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
	[Activity (Label = "Добавление приема пищи")]			
	public class addEatingActivity : Activity
	{
		static HEDB database = new HEDB();
		protected ImageButton btn_applyEating;                  // Кнопка ок
		protected ImageButton btn_setTime;			            // Кнопка установки времени
		protected ImageButton btn_addProducPortion;		        // Кнопка добавления продукт-порции
		protected ListView ppList;						        // Лист продукт порций
		private bool bool_new_eating;					        // True если это новый прием пищи
		private int current_eatingID; 					        // ID текущей продукт-порции
		private List<FoodPortionList> current_foodPortionsList; // Список продукт порций для добавления
		private DateTime cureent_time;							// Выбранное время

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			///////////////////////////////////////////////////////////////////////
			// Ставим нужный layout ///////////////////////////////////////////////
			///////////////////////////////////////////////////////////////////////

			SetContentView (Resource.Layout.addEating);


			///////////////////////////////////////////////////////////////////////
			// Получаем все контролы //////////////////////////////////////////////
			///////////////////////////////////////////////////////////////////////

			btn_applyEating = FindViewById<ImageButton> (Resource.Id.applyEating_button);
			btn_setTime = FindViewById<ImageButton> (Resource.Id.setTime_button);
			btn_addProducPortion = FindViewById<ImageButton> (Resource.Id.addProductPortion);
			ppList = FindViewById<ListView> (Resource.Id.productPortionList); //product portion list

			// Заполняем лист продукт порций //////////////////////////////////////
			// если не выбран прием пищи, то создаем новый
			bool_new_eating = false;

			var eatingID = Global.choosed_eating_ID;
				
			if (eatingID == int.MaxValue) {
				bool_new_eating = true;
				current_foodPortionsList = new List<FoodPortionList>();
			} else {
				// Ищем все продукт порции для выбранного приема пищи
				current_foodPortionsList = database.getFoodPortionList_by_EatingID(eatingID);
				current_eatingID = eatingID;
			}

			List<String> pp_array = new List<String>();

			//Заполняем ItemList
			foreach(var item in current_foodPortionsList) {
				string result_string = "";
				var _foodPortion = database.getFoodPortion (item.portionID);
				var _food = database.getFood (_foodPortion.foodID);
				result_string += "\n  " + _food.name + " - " + _foodPortion.count.ToString() + "г.";
				pp_array.Add (result_string);
			}

			ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Resource.Layout.default_ListItem, pp_array);
			ppList.Adapter = adapter;

			// Обработка событий /////////////////////////////////////////////////////////////

			btn_addProducPortion.Click += (sender, e) =>
			{
				//NewEating();
			};

			btn_setTime.Click += (sender, e) =>
			{

			};

			btn_applyEating.Click += (sender, e) =>
			{

			};
		}

		//update
	}
}

