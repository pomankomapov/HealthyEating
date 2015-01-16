
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
		private MealPlane current_mealPlane;          // План питания на текущий день
		private EatingDay eatingToday;				  // Текущий день питания
		private List<Eating> current_eatings;		  // Список приемов пищи

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

			current_eatings = new List<Eating>();

			// Текущий день
			DateTime today = DateTime.Today;
			System.Console.Out.WriteLine (today.ToString());

			//Ищем текущий день питания
			eatingToday = database.getEatingDay_by_Date (today);
			//Если в базе нет текущего дня, то его следует добавить
			if (eatingToday == null) {
				eatingToday = database.addEatingDay (today);
			}

			//текущий план питания (используется при добавлении приемов пищи)
			current_mealPlane = database.getMealPlane (eatingToday.mealPlaneID);

			updateEatingListView ();

			// Обработка событий /////////////////////////////////////////////////////////////

			RegisterForContextMenu (eatingList);

			btn_neweating.Click += (sender, e) =>
			{
				NewEating();
			};

			btn_back.Click += (sender, e) =>
			{
				base.OnBackPressed();
			};

			eatingList.ItemClick += (sender, e) => {
				editEating(e.Position);
			};
        }


		////////////////////////////////////////////////////////////////
		///		Контекстное меню для удаления
		////////////////////////////////////////////////////////////////

		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo) {
			menu.Add(Menu.None, 1, Menu.None, "Удалить");
		}

		public override bool OnContextItemSelected(IMenuItem item) {
			if (item.ItemId == 1) {
				AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo) item.MenuInfo;
				var eating_type = database.EatingTypeRus [(int)current_eatings [info.Position].eatingType];
				DialogDelOkCancel("Вы действительно хотите удалить '" + eating_type +"'?", info.Position);
			}
			return true;
		}


		////////////////////////////////////////////////////////////////
		///		Функции удаления
		////////////////////////////////////////////////////////////////
		private void delEating(int position) {
			var eating_type = database.EatingTypeRus [(int)current_eatings [position].eatingType];
			//Убиваем запись в плане питания
			database.delEatingListItem (current_eatings [position].ID, current_mealPlane.ID);

			//Убиваем прием пищи
			database.delEating (current_eatings [position].ID);

			updateEatingListView ();
			Global.print (this, eating_type + " удален");
		}



		//////////////////////////////////////////////////////////////////////
		/// 	Диалог подтверждения удаления
		//////////////////////////////////////////////////////////////////////

		protected void DialogDelOkCancel(String del_note, int del_position)
		{
			Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);
			AlertDialog alertdialog = builder.Create();
			alertdialog.SetTitle("Удаление приема пищи");
			alertdialog.SetMessage(del_note);

			alertdialog.SetButton("Ок", (s, e) => {
				delEating(del_position);
				alertdialog.Dismiss();
			});

			alertdialog.SetButton2("Отмена", (s, e) => {
				alertdialog.Dismiss();
			});

			alertdialog.Show();
		}



		///////////////////////////////////////////////////////////////////////
		// 		Обновление списка приемов пищи
		///////////////////////////////////////////////////////////////////////

		protected void updateEatingListView() {
			//Ищем все приемы пищи по плану питания на текущий день
			var eatings_list = database.getEatingList_by_MealPlaneID(eatingToday.mealPlaneID);

			current_eatings.Clear ();

			List<String> eatings_array = new List<String>();
			//Заполняем ItemList
			foreach(var item in eatings_list) {
				var eating = database.getEating (item.eatingID);

				//По пути обновляем лист текущих приемов пищи
				current_eatings.Add (eating);

				var result_string = database.EatingTypeRus[eating.eatingType];
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
		}


		///////////////////////////////////////////////////////////////////////
		// 		Диалог добавления приемов пищи
		///////////////////////////////////////////////////////////////////////

		protected void NewEating()
		{
			Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);

			AlertDialog alertdialog = builder.Create();
			alertdialog.SetTitle("Выберите тип");

			alertdialog.Show();
			alertdialog.SetContentView (Resource.Layout.food_types_dialog);

			Button food_type_button1 = alertdialog.FindViewById<Button>(Resource.Id.food_type_button1);
			Button food_type_button2 = alertdialog.FindViewById<Button>(Resource.Id.food_type_button2);
			Button food_type_button3 = alertdialog.FindViewById<Button>(Resource.Id.food_type_button3);
			Button food_type_button4 = alertdialog.FindViewById<Button>(Resource.Id.food_type_button4);

			//Все кроме nosh можно добавлять только один раз. Скрываем если нужно.
			bool breakfast_hide = false;
			bool lunch_hide = false;
			bool dinner_hide = false;

			foreach(var item in current_eatings) {
				if (item.eatingType == (int)EatingType.breakfast) {
					breakfast_hide = true;
				} else if(item.eatingType == (int)EatingType.lunch) {
					lunch_hide = true;
				} else if(item.eatingType == (int)EatingType.dinner) {
					dinner_hide = true;
				}
			}

			if (breakfast_hide) {
				food_type_button1.Visibility = ViewStates.Gone;
			} else {
				food_type_button1.Visibility = ViewStates.Visible;
			}
			if (lunch_hide) {
				food_type_button2.Visibility = ViewStates.Gone;
			} else {
				food_type_button2.Visibility = ViewStates.Visible;
			}
			if (dinner_hide) {
				food_type_button3.Visibility = ViewStates.Gone;
			} else {
				food_type_button3.Visibility = ViewStates.Visible;
			}

			food_type_button1.Text = database.EatingTypeRus [(int)EatingType.breakfast];
			food_type_button2.Text = database.EatingTypeRus [(int)EatingType.lunch];
			food_type_button3.Text = database.EatingTypeRus [(int)EatingType.dinner];
			food_type_button4.Text = database.EatingTypeRus [(int)EatingType.nosh];

			food_type_button1.Click += (sender, e) =>
			{
				Global.choosed_eating_type = EatingType.breakfast;
				Global.choosed_eating_ID = int.MaxValue;
				alertdialog.Dismiss();
				addEatingShow();
			};

			food_type_button2.Click += (sender, e) =>
			{
				Global.choosed_eating_type = EatingType.lunch;
				Global.choosed_eating_ID = int.MaxValue;
				alertdialog.Dismiss();
				addEatingShow();
			};
				
			food_type_button3.Click += (sender, e) =>
			{
				Global.choosed_eating_type = EatingType.dinner;
				Global.choosed_eating_ID = int.MaxValue;
				alertdialog.Dismiss();
				addEatingShow();
			};

			food_type_button4.Click += (sender, e) =>
			{
				Global.choosed_eating_type = EatingType.nosh;
				Global.choosed_eating_ID = int.MaxValue;
				alertdialog.Dismiss();
				addEatingShow();
			};
		}

		protected void editEating(int position) {
			Global.choosed_eating_ID = current_eatings [position].ID;
			addEatingShow ();
		}

		protected void addEatingShow() {
			Global.choosed_mealPlain_ID = current_mealPlane.ID;
			StartActivityForResult(typeof(addEatingActivity), 1);
		}

		//Апдейт при добавлении/удалении
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
			if (resultCode == Result.Ok) {
				updateEatingListView ();
			}
		}

    }

	/////////////////////////////////////////////////////////////
	//   Нужен обработчик событий клика по списку приемов пищи!!!!

}

