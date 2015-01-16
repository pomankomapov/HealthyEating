
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
		static HEDB database = new HEDB();						  // База данных
		protected ImageButton btn_applyEating;                    // Кнопка ок
		protected ImageButton btn_setTime;			              // Кнопка установки времени
		protected ImageButton btn_addProducPortion;		          // Кнопка добавления продукт-порции
		protected ListView ppListView;						      // Лист продукт порций
		private bool bool_new_eating;					          // True если это новый прием пищи
		private Eating current_eating; 					          // Текущий пррием пищи
		private List<FoodPortionList> current_foodPortionsList;   // Список продукт порций для добавления
		private List<FoodPortion> current_foodPortionsList_Items; // Список элементов продукт порций для листа
		private List<Food> allFoods;							  // Список всех имеющихся в базе продуктов
		protected TextView eatingTypeLabel;						  // Выбранный тип приема пищи

		//Для диалогов
		protected TextView pp_productSelected;
		protected TextView pp_portionText;
		protected ListView pp_productList;
		protected SeekBar pp_portion_seekbar;
		protected ImageButton pp_applyPortion;

		// Минимальный вес порции
		private const int min_weight = 10;						  


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
			eatingTypeLabel = FindViewById<TextView> (Resource.Id.eatingTypeLabel);
			ppListView = FindViewById<ListView> (Resource.Id.productPortionList); //product portion list

			eatingTypeLabel.Text = database.EatingTypeRus[(int)Global.choosed_eating_type];
			//ppListView.SetScrollContainer (false);


			/////////////////////////////////////////////////////////////////
			/// 	Инициализация контента
			/////////////////////////////////////////////////////////////////

			// Инициализация листа продукт порций //////////////////////////////////////
			// если не выбран прием пищи, то создаем новый
			bool_new_eating = false;
			// инициализируем список продуктов
			allFoods = database.getAllFood ();

			var eatingID = Global.choosed_eating_ID;
				
			if (eatingID == int.MaxValue) {
				bool_new_eating = true;
				current_foodPortionsList = new List<FoodPortionList>();
				current_foodPortionsList_Items = new List<FoodPortion>();
				current_eating = new Eating {
					eaten = false,
					eatingType = (int)Global.choosed_eating_type,
					time = database.EatingTimes[(int) Global.choosed_eating_type]
				};
			} else {
				current_eating = database.getEating (eatingID);
				// Ищем все продукт порции для выбранного приема пищи
				current_foodPortionsList = database.getFoodPortionList_by_EatingID(eatingID);
				current_foodPortionsList_Items = new List<FoodPortion>();
				foreach (var item in current_foodPortionsList) {
					current_foodPortionsList_Items.Add(database.getFoodPortion(item.portionID));
				}
			}

			updateProductPortionList ();


			/////////////////////////////////////////////////////////////////
			/// 	Обработка событий
			/////////////////////////////////////////////////////////////////

			ppListView.ItemClick += (sender, e) => {
				EditPortion(e.Position);
			};

			btn_addProducPortion.Click += (sender, e) =>
			{
				EditPortion(-1);
			};

			btn_setTime.Click += (sender, e) =>
			{
				EditTime();
			};

			btn_applyEating.Click += (sender, e) =>
			{
				ApplyChanges();
			};
		}



		/////////////////////////////////////////////////////////////////
		/// 	Диалог создания/изменения продукт-порций
		/////////////////////////////////////////////////////////////////

		protected void EditPortion(int portionID)
		{
			FoodPortion selectedPortion = null;
			int choosenProductIndex = -1;

			if (portionID >= 0) {
				//Взять порцию из списка и поехали!
				selectedPortion = current_foodPortionsList_Items [portionID];
			}

			Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);

			AlertDialog alertdialog = builder.Create();
			alertdialog.SetTitle("Выберите продукт и его размер порции");

			alertdialog.SetButton("Close", (s, e) => {
				//StartActivity(typeof(ProfileActivity));
			});

			alertdialog.Show();
			alertdialog.SetContentView (Resource.Layout.addProductPortion);

			pp_productSelected = alertdialog.FindViewById<TextView>(Resource.Id.productSelected);
			pp_portionText = alertdialog.FindViewById<TextView>(Resource.Id.portionText);
			pp_productList = alertdialog.FindViewById<ListView>(Resource.Id.productList);
			pp_portion_seekbar = alertdialog.FindViewById<SeekBar>(Resource.Id.portion_seekbar);
			pp_applyPortion = alertdialog.FindViewById<ImageButton>(Resource.Id.applyPortion);

			if (portionID >= 0) {
				var foodItem = database.getFood (selectedPortion.foodID);
				pp_productSelected.Text = foodItem.name;
				pp_productList.Visibility = ViewStates.Gone;
				pp_portion_seekbar.Progress = selectedPortion.count - min_weight;
			} else {
				pp_productSelected.Text = "выберите продукт";

				pp_productList.Visibility = ViewStates.Visible;

				List<String> food_array = new List<String>();

				//Заполняем List
				foreach(var item in allFoods) {
					food_array.Add (item.name);
				}

				ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Resource.Layout.mini_ListItem, food_array);
				pp_productList.Adapter = adapter;

				pp_productList.ItemClick += (sender, e) => {
					choosenProductIndex = e.Position;
					pp_productSelected.Text = allFoods[choosenProductIndex].name;
				};

				pp_portion_seekbar.Progress = 0;
			}

			pp_portionText.Text = string.Format ("{0}", min_weight + pp_portion_seekbar.Progress);

			pp_portion_seekbar.ProgressChanged += (sender, e) => {
				int new_weight = min_weight + pp_portion_seekbar.Progress;
				pp_portionText.Text = string.Format("{0}", new_weight);
			};


			pp_applyPortion.Click += (sender, e) =>
			{
				if (portionID >= 0) {
					current_foodPortionsList_Items[portionID].count = pp_portion_seekbar.Progress + min_weight;

					updateProductPortionList();
					alertdialog.Dismiss();
				} else {
					if(choosenProductIndex >= 0) {
						int _foodID = allFoods[choosenProductIndex].ID;
						current_foodPortionsList.Add(new FoodPortionList{});// Заполнится при общем сабмите
						current_foodPortionsList_Items.Add(new FoodPortion{
							count = pp_portion_seekbar.Progress + min_weight,
							foodID = _foodID
						});

						updateProductPortionList();
						alertdialog.Dismiss();
					} else {
						Global.print(this, "Вы должны выбрать продукт!");
					}
				}
			};
		}


		/////////////////////////////////////////////////////////////////
		/// 	Диалог изменения времени
		/////////////////////////////////////////////////////////////////

		protected void EditTime()
		{
			Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);

			AlertDialog alertdialog = builder.Create();
			alertdialog.SetTitle("Выберите желаемое время");

			alertdialog.Show();
			alertdialog.SetContentView (Resource.Layout.setTimeDialog);

			var pp_eating_timepicker = alertdialog.FindViewById<TimePicker>(Resource.Id.eating_timepicker);
			var pp_applyTime = alertdialog.FindViewById<ImageButton>(Resource.Id.applyTime);

			pp_eating_timepicker.SetIs24HourView (new Java.Lang.Boolean(true));

			pp_eating_timepicker.CurrentHour = new Java.Lang.Integer(current_eating.time.Hour);
			pp_eating_timepicker.CurrentMinute =  new Java.Lang.Integer(current_eating.time.Minute);

			//pp_eating_timepicker.TimeChanged += (sender, e) => {};

			pp_applyTime.Click += (sender, e) =>
			{
				current_eating.time = new DateTime(2015, 1, 1, pp_eating_timepicker.CurrentHour.IntValue(), pp_eating_timepicker.CurrentMinute.IntValue(), 0);
				alertdialog.Dismiss();
			};
		}


		/////////////////////////////////////////////////////////////////
		/// 	Обновление списка продукт-порций
		/////////////////////////////////////////////////////////////////

		public void updateProductPortionList() {
			List<String> pp_array = new List<String>();

			//Заполняем ItemList

			foreach (var item in current_foodPortionsList_Items) {
				var _food = database.getFood (item.foodID);
				pp_array.Add (_food.name + " - " + item.count.ToString () + "г.");
			}

			ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Resource.Layout.default_ListItem, pp_array);
			ppListView.Adapter = adapter;
		}


		/////////////////////////////////////////////////////////////////
		/// 	Сохранение в БД
		/////////////////////////////////////////////////////////////////

		public void ApplyChanges() {
			//Проверяем - не пуст ли список порций
			if (current_foodPortionsList.Count != 0) {

				//Если это новый прием пищи
				if (bool_new_eating) {
					//Создаем новый прием пищи
					Eating dbEating = database.addEating (current_eating);

					//Создание списка продукт порций
					int i = 0;
					while (i < current_foodPortionsList_Items.Count) {
						//создание продукт порции
						var dbPortion = database.addFoodPortion (current_foodPortionsList_Items [i]);
						//подготовка для добавления в список продукт порций
						current_foodPortionsList [i].eatingID = dbEating.ID;
						current_foodPortionsList [i].portionID = dbPortion.ID;
						//добавление в список
						database.addFoodPortionList (current_foodPortionsList [i]);
						i += 1;
					}

					//Привязываемся к текущему плану питания
					database.addEatingList (Global.choosed_mealPlain_ID, dbEating.ID);
					Global.print(base.BaseContext, database.EatingTypeRus[dbEating.eatingType] + " добавлен");
				}
				//Если это изменение уже существующего приема пищи
				else {
					//Обновляем прием пищи
					Eating dbEating = database.UpdateEating (current_eating);

					//Обновляем список порций
					int i = 0;
					while (i < current_foodPortionsList_Items.Count) {
						//Обновляем сами продукты если они есть и добавляем если небыло
						FoodPortion dbPortion;
						if (current_foodPortionsList_Items [i].ID == 0) {
							dbPortion = database.addFoodPortion (current_foodPortionsList_Items [i]);
						} else {
							dbPortion = database.UpdateFoodPortion (current_foodPortionsList_Items [i]);
						}

						//подготовка для добавления в список продукт порций
						current_foodPortionsList [i].eatingID = dbEating.ID;
						current_foodPortionsList [i].portionID = dbPortion.ID;

						//Обновляем списки если они есть и добавляем если небыло
						if (current_foodPortionsList [i].ID == 0) {
							database.addFoodPortionList (current_foodPortionsList [i]);
						} else {
							database.UpdateFoodPortionList (current_foodPortionsList [i]);
						}

						i += 1;
					}

					Global.print(base.BaseContext,  database.EatingTypeRus[dbEating.eatingType] + " обновлен");
				}

				SetResult (Result.Ok);
				Global.choosed_eating_ID = int.MaxValue;
				Finish ();

			} else {
				Global.print (this, "Нужно добавить хотя бы одну порцию!");
			}
		}
	}

	/*
	Допилить:
		Удаление продукт порций и списков
	*/
}

