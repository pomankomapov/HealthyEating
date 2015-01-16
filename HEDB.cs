using System;
using SQLite;
using System.Collections.Generic;

namespace healthy_eating
{
	public enum EatingType : int
	{
		breakfast = 0,
		lunch = 1,
		dinner = 2,
		nosh = 3
	};

    // Healthy Eating DataBase
	public class HEDB
	{
		// Коннектор к базе
		private SQLiteConnection hedb;

		//Интерпретация типов приемов пищи на русский
		public readonly List<String> EatingTypeRus;
		public readonly List<DateTime> EatingTimes;

		///////////////////////////////////////////////////////////////////////////
		//            Блок инициализации   ////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////

		public HEDB ()
		{
			EatingTypeRus = new List<string> {"Завтрак", "Обед", "Ужин", "Перекус"};
			EatingTimes = new List<DateTime> { new DateTime(2015, 1, 1, 7, 0, 0), new DateTime(2015, 1, 1, 12, 0, 0), new DateTime(2015, 1, 1, 18, 0, 0), new DateTime(2015, 1, 1, 15, 0, 0) };
			initializeDatabase();
		}

		private void initializeDatabase() {
			// Создание или подключение к базе HelthyEating.db
			string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			hedb = new SQLiteConnection (System.IO.Path.Combine (folder, "HelthyEating.db"));

            hedb.CreateTable<Profile>();   		 // Создаем таблицу профилей
            hedb.CreateTable<Profession>();      // Создаем таблицу профессий
            hedb.CreateTable<Lifestyle>();  	 // Создаем таблицу образов жизни
            hedb.CreateTable<Allergic>();    	 // Создаем таблицу аллергенных продуктов
            hedb.CreateTable<Food>();       	 // Создаем таблицу продуктов
            hedb.CreateTable<FoodPortion>();	 // Создаем таблицу продукт-порция
            hedb.CreateTable<Eating>();      	 // Создаем таблицу приём пищи
			hedb.CreateTable<EatingDay>();	 	 // Создаем таблицу дней питания
			hedb.CreateTable<FoodPortionList>(); // Создаем таблицу списка продукт-порций
			hedb.CreateTable<EatingList>();		 // Создаем таблицу списка приемов пищи
			hedb.CreateTable<EatingTemplate>();  // Создаем таблицу шаблонов приемов пищи
			hedb.CreateTable<MealPlane>();		 // Создаем таблицу планов питания

		}

        /// <summary>
        /// Очищает все таблицы
        /// </summary>
        public void delAll()
        {
            hedb.DeleteAll<Profile>();
            hedb.DeleteAll<Profession>();
            hedb.DeleteAll<Lifestyle>();
            hedb.DeleteAll<Allergic>();
            hedb.DeleteAll<Food>();
            hedb.DeleteAll<FoodPortion>();
            hedb.DeleteAll<Eating>();
			hedb.DeleteAll<EatingDay>();
			hedb.DeleteAll<FoodPortionList>();
			hedb.DeleteAll<EatingList>();
			hedb.DeleteAll<EatingTemplate>();
			hedb.DeleteAll<MealPlane>();
        }


		/*#######################################################################*/
		/*########################		 МЕТОДЫ       ###########################*/
		/*#######################################################################*/


		///////////////////////////////////////////////////////////////////////////
		//				 "Профиль"   //////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавляет новый "профиль пользователя" в базу.
		/// </summary>
        public Profile addProfile(string _deviceID, string _name, int _current_weight, int _desired_weight, 
                                  int _growth, int _age, bool _man, bool _show_recommends)
        {
			// Добавляем новый профиль
			var _profile = new Profile {
                deviceID = _deviceID,
                name = _name,
                current_weight = _current_weight,
                desired_weight = _desired_weight,
				growth = _growth,
				age = _age,
                man = _man,
				show_recommends = _show_recommends
			};
			hedb.Insert (_profile);
			return _profile;
		}

        public Profile getProfile(int ID)
        {
            Profile result;
            try
            {
                var query = hedb.Table<Profile>().Where(x => x.ID == ID);
                result = query.First();
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public Profile getProfileByDevice(string deviceID)
        {
            Profile result;
            try
            {
                var query = hedb.Table<Profile>().Where(x => x.deviceID == deviceID);
                result = query.First();
            }
            catch
            {
                result = null;
            }

            return result;
        }

		/// <summary>
		/// Удаляет "профиль пользователя" по заданному (int)ID.
		/// <returns>Возвращает: (int)число удаленных строк</returns>
		/// </summary>
		public int delProfile(int ID) {
			return hedb.Delete<Profile> (ID);
		}

		/// <summary>
		/// Удаляет все записи типа "профиль пользователя"(Profile).
		/// <returns>Возвращает: (int)число удаленных строк</returns>
		/// </summary>
		public int delAllProfile() {
			return hedb.DeleteAll<Profile> ();
		}



		///////////////////////////////////////////////////////////////////////////
		//					 "Профессия"   ////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавляет новую "профессию" в базу по заданному "профилю"(Profile).
		/// </summary>
		public Profession addProfession(Profile _profile, string _name) {
			// Добавляем новый профиль
			var _profession = new Profession {
				profileID = _profile.ID,
				name = _name		
			};
			hedb.Insert (_profession);
			return _profession;
		}

		/// <summary>
		/// Удаляет "профессию" по заданному (int)ID.
		/// <returns>Возвращает: (int)число удаленных строк</returns>
		/// </summary>
		public int delProfession(int ID) {
			return hedb.Delete<Profession> (ID);
		}

		/// <summary>
		/// Удаляет все записи типа "профессия"(Profession).
		/// <returns>Возвращает: (int)число удаленных строк</returns>
		/// </summary>
		public int delAllProfession() {
			return hedb.DeleteAll<Profession> ();
		}



		///////////////////////////////////////////////////////////////////////////
		// 					   "Образ жизни" //////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Adds the lifestyle.
        /// </summary>
        /// <returns>The lifestyle.</returns>
        /// <param name="_name">Name.</param>
        public Lifestyle addLifestyle(string _name)
        {
            // Добавляем новый профиль
            var _lifestyle = new Lifestyle {
                name = _name
            };
            hedb.Insert (_lifestyle);
            return _lifestyle;
        }

        public Lifestyle getLifestyle(int ID)
        {
            //var query = hedb.Table<Lifestyle>().Where(x => x.ID == ID);
            //var result = query.ElementAt(0);
            var result = hedb.Get<Lifestyle>(ID);

            return result;
        }

       
        public int delLifestyle(int ID) {
            return hedb.Delete<Lifestyle> (ID);
        }


        public int delAllLifestyle() {
            return hedb.DeleteAll<Lifestyle> ();
        }



		///////////////////////////////////////////////////////////////////////////
		//                     "Аллергенные продукты" /////////////////////////////
		///////////////////////////////////////////////////////////////////////////

        public Allergic addAllergic(int _profileID, int _foodID)
        {
            var _allergic = new Allergic {
                profileID = _profileID,
                foodID = _foodID
            };
            hedb.Insert (_allergic);
            return _allergic;
        }

        public Allergic getAllergic(int profileID)
        {
            Allergic result;
            try
            {
                var query = hedb.Table<Allergic>().Where(x => x.profileID == profileID);
                result = query.First();
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public Allergic findAllergic(int profileID, string name)
        {
            name = name.Trim();

            Food food = findFood(name);
            if (food == null)
                return null;

            Allergic result;
            try
            {
                var query = hedb.Table<Allergic>().Where(x => x.foodID == food.ID).Where(x => x.profileID == profileID);
                result = query.First();
            }
            catch
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Получить названия всех аллергенных продуктов пользователя
        /// </summary>
        /// <returns>The all allergic.</returns>
        /// <param name="profileID">Profile I.</param>
        public List<string> getAllAllergicNames(int profileID)
        {
            var table = hedb.Table<Allergic>().Where(x => x.profileID == profileID);
            List<string> foods = new List<string>();
            foreach (var item in table)
            {
                Food food = getFood(item.foodID);
                foods.Add(food.name);
            }

            if (foods.Count < 1)
            {
                foods.Add("Отсутствуют");
            }

            return foods;
        }

        public void delAllergic(int profileID, int foodID) {
            var query = hedb.Table<Allergic>().Where(x => x.foodID == foodID).Where(x => x.profileID == profileID);

            foreach (var item in query)
            {
                hedb.Delete<Allergic>(item.ID);
            }
        }

        public int delAllAllergic() {
            return hedb.DeleteAll<Allergic> ();
        }



		///////////////////////////////////////////////////////////////////////////
		//                     "Продукты" /////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Добавляет продукт в базу. Принудительно понижает регистр!
        /// </summary>
        /// <returns>Продукт</returns>
        /// <param name="_name">Имя</param>
        /// <param name="_proteins">Белки</param>
        /// <param name="_fats">Жиры</param>
        /// <param name="_carbs">Угдеводы</param>
        /// <param name="_calories">Калории</param>
        /// <param name="_ProfileID">Номер профиля добавившего продукт</param>
        public Food addFood(string _name, float _proteins, float _fats, float _carbs, float _calories, int _ProfileID)
        {
            _name = _name.ToLower().Trim();

            // Добавляем новый профиль
            var _food = new Food {
                 name      = _name, 
                 proteins  = _proteins,  
                 fats      = _fats, 
                 carbs     = _carbs,
                 calories  = _calories, 
                 profileID = _ProfileID
            };
            hedb.Insert (_food);
            return _food;
        }

        public Food getFood(int ID)
        {
            Food result;
            try
            {
                var query = hedb.Table<Food>().Where(x => x.ID == ID);
                result = query.First();
            }
            catch
            {
                result = null;
            }

            return result;
        }

		public Food getFood(string _name)
		{
			_name = _name.ToLower().Trim();

			Food result;
			try
			{
				var query = hedb.Table<Food>().Where(x => x.name == _name);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

        public Food findFood(string name)
        {
            name = name.Trim();

            Food result;
            try
            {
                var query = hedb.Table<Food>().Where(x => name.Equals(x.name));
                result = query.First();
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public List<Food> getAllFood()
        {
            var table = hedb.Table<Food>();
            List<Food> foods = new List<Food>();
            foreach (var item in table)
            {
                foods.Add(item);
            }
            return foods;
        }

        public int delFood(int ID) {
            return hedb.Delete<Food> (ID);
        }

        public int delAllFood() {
            return hedb.DeleteAll<Food> ();
        }



		////////////////////////////////////////////////////////////////////////////
		//                     "Продукт-порции" ////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавляет продукт-порцию в базу.
		/// </summary>
		/// <returns>FoodPortion</returns>
		/// <param name="_foodID">ID продукта</param>
		/// <param name="_count">количество</param>
		public FoodPortion addFoodPortion(int _foodID, int _count)
		{
			if (_foodID < 0 && _count < 0) {
				return null;
			}

			// Добавляем новую продукт-порцию
			var _foodPortion = new FoodPortion {
				foodID = _foodID, 
				count  = _count
			};

			hedb.Insert (_foodPortion);
			return _foodPortion;
		}

		public FoodPortion addFoodPortion(FoodPortion new_foodPorion)
		{
			// Добавляем новую продукт-порцию
			hedb.Insert (new_foodPorion);
			return new_foodPorion;
		}

		public FoodPortion UpdateFoodPortion(FoodPortion foodportion_to_update) {
			if (getFoodPortion (foodportion_to_update.ID) == null) {
				return null;
			}

			hedb.Update (foodportion_to_update);
			return foodportion_to_update;
		}

		public FoodPortion getFoodPortion(int ID)
		{
			FoodPortion result;
			try
			{
				var query = hedb.Table<FoodPortion>().Where(x => x.ID == ID);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public List<FoodPortion> getAllFoodPortion()
		{
			var table = hedb.Table<FoodPortion>();
			List<FoodPortion> foodPortions = new List<FoodPortion>();
			foreach (var item in table)
			{
				foodPortions.Add(item);
			}
			return foodPortions;
		}

		public int delFoodPortion(int ID) {
			return hedb.Delete<FoodPortion> (ID);
		}

		public int delAllFoodPortion() {
			return hedb.DeleteAll<FoodPortion> ();
		}



		////////////////////////////////////////////////////////////////////////////
		//                     "Приемы пищи"    ////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавление.
		/// </summary>
		/// <returns>Eating</returns>
		/// <param name="_time">Время приема пищи</param>
		/// <param name="_eated">Съеден</param>

		public Eating addEating(DateTime _time, EatingType _eatingType, bool _eaten = false)
		{
			// Добавляем новый прием пищи
			var _eating = new Eating {
				time = _time,
				eaten  = _eaten,
				eatingType = (int)_eatingType
			};

			hedb.Insert (_eating);
			return _eating;
		}

		public Eating addEating(Eating new_eating)
		{
			// Добавляем новый прием пищи
			hedb.Insert (new_eating);
			return new_eating;
		}

		public Eating UpdateEating(Eating eating_to_update) {
			if (getEating (eating_to_update.ID) == null) {
				return null;
			}

			hedb.Update (eating_to_update);
			return eating_to_update;
		}

		public Eating getEating(int ID)
		{
			Eating result;
			try
			{
				var query = hedb.Table<Eating>().Where(x => x.ID == ID);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public List<Eating> getAllEating()
		{
			var table = hedb.Table<Eating>();
			List<Eating> eatings = new List<Eating>();
			foreach (var item in table)
			{
				eatings.Add(item);
			}
			return eatings;
		}

		public int delEating(int ID) {
			return hedb.Delete<Eating> (ID);
		}

		public int delAllEating() {
			return hedb.DeleteAll<Eating> ();
		}



		////////////////////////////////////////////////////////////////////////////
		//            "Шаблоны приемов пищи"    ////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавление.
		/// </summary>
		/// <returns>EatingTemplate</returns>

		public EatingTemplate addEatingTemplate()
		{
			// Добавляем новый шаблон
			var _eating = new EatingTemplate{};
			hedb.Insert (_eating);
			return _eating;
		}

		public EatingTemplate getEatingTemplate(int ID)
		{
			EatingTemplate result;
			try
			{
				var query = hedb.Table<EatingTemplate>().Where(x => x.ID == ID);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public List<EatingTemplate> getAllEatingTemplate()
		{
			var table = hedb.Table<EatingTemplate>();
			List<EatingTemplate> eatings = new List<EatingTemplate>();
			foreach (var item in table)
			{
				eatings.Add(item);
			}
			return eatings;
		}

		public int delEatingTemplate(int ID) {
			return hedb.Delete<EatingTemplate> (ID);
		}

		public int delAllEatingTemplate() {
			return hedb.DeleteAll<EatingTemplate> ();
		}



		////////////////////////////////////////////////////////////////////////////
		//                   "Планы питания"    ////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавление.
		/// </summary>
		/// <returns>MealPlane</returns>
		public MealPlane addMealPlane()
		{
			// Добавляем новый шаблон
			var _eating = new MealPlane{};
			hedb.Insert (_eating);
			return _eating;
		}

		public MealPlane getMealPlane(int ID)
		{
			MealPlane result;
			try
			{
				var query = hedb.Table<MealPlane>().Where(x => x.ID == ID);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public List<MealPlane> getAllMealPlane()
		{
			var table = hedb.Table<MealPlane>();
			List<MealPlane> eatings = new List<MealPlane>();
			foreach (var item in table)
			{
				eatings.Add(item);
			}
			return eatings;
		}

		public int delMealPlane(int ID) {
			return hedb.Delete<MealPlane> (ID);
		}

		public int delAllMealPlane() {
			return hedb.DeleteAll<MealPlane> ();
		}



		////////////////////////////////////////////////////////////////////////////
		//   "Списки приемов пищи для плана питания"    ////////////////////////////
		////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавление.
		/// </summary>
		/// <returns>EatingList</returns>
		/// <param name="_mealPlaneID">ID плана питания</param>
		/// <param name="_eatingID">ID приема пищи</param>
		public EatingList addEatingList(int _mealPlaneID, int _eatingID)
		{
			if (_mealPlaneID < 0 && _eatingID < 0) {
				return null;
			}

			// Добавляем новый прием пищи
			var _EatingList = new EatingList {
				mealPlaneID = _mealPlaneID,
				eatingID  = _eatingID
			};

			hedb.Insert (_EatingList);
			return _EatingList;
		}

		/*
		public List<EatingList> addEatingList(int _mealPlane, List<Eating> _eatings)
		{
			if (_mealPlane < 0) {
				return null;
			}
				
			var eatinglist = new List<EatingList> ();

			foreach (var item in _eatings) {
				var _EatingList = new EatingList {
					mealPlaneID = _mealPlane,
					eatingID  = item.ID
				};

				hedb.Insert (_EatingList);

				eatinglist.Add (_EatingList);
			}

			return eatinglist;
		}
		*/

		public List<EatingList> getEatingList_by_MealPlaneID(int MealPlaneID)
		{
			List<EatingList> result = new List<EatingList>();
			try
			{
				var query = hedb.Table<EatingList>().Where(x => x.mealPlaneID == MealPlaneID);
				foreach (var item in query)
				{
					result.Add(item);
				}
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public EatingList getEatingList(int ID)
		{
			EatingList result;
			try
			{
				var query = hedb.Table<EatingList>().Where(x => x.ID == ID);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public List<EatingList> getAllEatingList()
		{
			var table = hedb.Table<EatingList>();
			List<EatingList> eatingList = new List<EatingList>();
			foreach (var item in table)
			{
				eatingList.Add(item);
			}
			return eatingList;
		}

		public int delEatingList(int ID) {
			return hedb.Delete<EatingList> (ID);
		}

		public int delEatingListItem(int EatingID, int MealPlaneID) {
			var query = hedb.Table<EatingList>().Where(x => (x.mealPlaneID == MealPlaneID) && (x.eatingID == EatingID));

			if (query.Count() == 0) {
				return 0;
			}

			EatingList delEating = query.First ();
			return hedb.Delete<EatingList> (delEating.ID);
		}

		public int delEatingList_by_MealPlaneID(int MealPlaneID) {
			var query = hedb.Table<EatingList>().Where(x => x.mealPlaneID == MealPlaneID);
			int result = 0;
			foreach (var item in query)
			{
				result += hedb.Delete<EatingList> (item.ID);
			}
			return result;
		}

		public int delAllEatingList() {
			return hedb.DeleteAll<EatingList> ();
		}



		////////////////////////////////////////////////////////////////////////////
		//           "Cписок продукт-порций"    ////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавление.
		/// </summary>
		/// <returns>FoodPortionList</returns>
		/// <param name="_portionID">ID проодукт-порции</param>
		/// <param name="_eatingID">ID приема пищи</param>
		public FoodPortionList addFoodPortionList(int _portionID, Eating _eating)
		{
			if (_portionID < 0) {
				return null;
			}

			// Добавляем новый прием пищи
			var _foodPortionList = new FoodPortionList {
				portionID = _portionID,
				eatingID  = _eating.ID
			};

			hedb.Insert (_foodPortionList);
			return _foodPortionList;
		}

		/// <summary>
		/// Добавление.
		/// </summary>
		/// <returns>FoodPortionList</returns>
		/// <param name="_portionID">ID проодукт-порции</param>
		/// <param name="_eatingTemplateID">ID шаблона приема пищи</param>
		public FoodPortionList addFoodPortionList(int _portionID, EatingTemplate _eatingTemplate)
		{
			if (_portionID < 0) {
				return null;
			}

			// Добавляем новый прием пищи
			var _foodPortionList = new FoodPortionList {
				portionID = _portionID,
				eatingTemplateID = _eatingTemplate.ID
			};

			hedb.Insert (_foodPortionList);
			return _foodPortionList;
		}

		/// <summary>
		/// Добавление списка порций.
		/// </summary>
		/// <returns>FoodPortionList</returns>
		/// <param name="_portionID">ID проодукт-порции</param>
		/// <param name="_eatingID">ID приема пищи</param>
		public List<FoodPortionList> addFoodPortionList(List<FoodPortion> portions, Eating _eating)
		{
			var PortionsList = new List<FoodPortionList> ();

			foreach (var item in portions) {
				var _foodPortionList = new FoodPortionList {
					portionID = item.ID,
					eatingID  = _eating.ID
				};
				hedb.Insert (_foodPortionList);
				PortionsList.Add (_foodPortionList);
			}

			return PortionsList;
		}

		/// <summary>
		/// Добавление списка порций.
		/// </summary>
		/// <returns>FoodPortionList</returns>
		/// <param name="_portionID">ID проодукт-порции</param>
		/// <param name="_eatingID">ID приема пищи</param>
		public List<FoodPortionList> addFoodPortionList(List<FoodPortion> portions, EatingTemplate _eatingTemplate)
		{
			var PortionsList = new List<FoodPortionList> ();

			foreach (var item in portions) {
				var _foodPortionList = new FoodPortionList {
					portionID = item.ID,
					eatingID  = _eatingTemplate.ID
				};
				hedb.Insert (_foodPortionList);
				PortionsList.Add (_foodPortionList);
			}

			return PortionsList;
		}

		public FoodPortionList addFoodPortionList(FoodPortionList fplist)
		{
			hedb.Insert (fplist);
			return fplist;
		}

		public FoodPortionList UpdateFoodPortionList(FoodPortionList foodportionlist_to_update) {
			if (getFoodPortionList (foodportionlist_to_update.ID) == null) {
				return null;
			}

			hedb.Update (foodportionlist_to_update);
			return foodportionlist_to_update;
		}

		public List<FoodPortionList> getFoodPortionList_by_EatingID(int EatingID)
		{
			List<FoodPortionList> result = new List<FoodPortionList>();
			try
			{
				var query = hedb.Table<FoodPortionList>().Where(x => x.eatingID == EatingID);
				foreach (var item in query)
				{
					result.Add(item);
				}
			}
			catch
			{
				result = null;
			}

			return result;
		}

		/*
		public List<FoodPortionList> getFoodPortionList_by_EatingTemplateID(int EatingTemplateID)
		{
			List<FoodPortionList> result = new List<FoodPortionList>();
			try
			{
				var query = hedb.Table<FoodPortionList>().Where(x => x.eatingTemplateID == EatingTemplateID);
				foreach (var item in query)
				{
					result.Add(item);
				}
			}
			catch
			{
				result = null;
			}

			return result;
		}
		*/

		public FoodPortionList getFoodPortionList(int ID)
		{
			FoodPortionList result;
			try
			{
				var query = hedb.Table<FoodPortionList>().Where(x => x.ID == ID);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public List<FoodPortionList> getAllFoodPortionList()
		{
			var table = hedb.Table<FoodPortionList>();
			List<FoodPortionList> foodPortionLists = new List<FoodPortionList>();
			foreach (var item in table)
			{
				foodPortionLists.Add(item);
			}
			return foodPortionLists;
		}

		public int delFoodPortionList(int ID) {
			return hedb.Delete<FoodPortionList> (ID);
		}

		public int delFoodPortionList_by_EatingID(int EatingID) {
			var query = hedb.Table<FoodPortionList>().Where(x => x.eatingID == EatingID);
			int result = 0;
			foreach (var item in query)
			{
				result += hedb.Delete<FoodPortionList> (item.ID);
			}
			return result;
		}

		/*
		public int delFoodPortionList_by_EatingTemplateID(int EatingTemplateID) {
			var query = hedb.Table<FoodPortionList>().Where(x => x.eatingTemplateID == EatingTemplateID);
			int result = 0;
			foreach (var item in query)
			{
				result += hedb.Delete<FoodPortionList> (item.ID);
			}
			return result;
		}
		*/

		public int delAllFoodPortionList() {
			return hedb.DeleteAll<FoodPortionList> ();
		}
			


		////////////////////////////////////////////////////////////////////////////
		//                     "Дни питания"    ////////////////////////////////////
		//		Внимание! Работу с mealPlane нужно переделывать для многих планов!
		////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавление. (если параметр DateTime и он единстевенен, тогда автоматом добавится новый план питания)
		/// </summary>
		/// <returns>EatingDay</returns>
		public EatingDay addEatingDay(DateTime _eatingDate)
		{
			var query = hedb.Table<EatingDay>().Where(x => x.eatingDate == _eatingDate);

			if (query.Count() > 0) {
				return null;
			}

			// Добавим план питания
			var _mealPlane = addMealPlane ();
			//Console.Out.WriteLine ("New MealPlane ID = " + _mealPlane.ID.ToString ());
			// Добавляем новый
			var _eating_day = new EatingDay{
				mealPlaneID = _mealPlane.ID,
				allCalories = 0,
				eatingDate = _eatingDate
			};

			hedb.Insert (_eating_day);
			return _eating_day;
		}

		/// <summary>
		/// Добавление.
		/// </summary>
		/// <returns>EatingDay</returns>
		public EatingDay addEatingDay(int _mealPlaneID, int _allCalories, DateTime _eatingDate)
		{
			var query = hedb.Table<EatingDay>().Where(x => x.mealPlaneID == _mealPlaneID);
			var query_day = hedb.Table<EatingDay>().Where(x => x.eatingDate == _eatingDate);

			if (query.Count() > 0 || query_day.Count() > 0) {
				return null;
			}

			// Добавляем новый
			var _eating = new EatingDay{
				mealPlaneID = _mealPlaneID,
				allCalories = _allCalories,
				eatingDate = _eatingDate
			};

			hedb.Insert (_eating);
			return _eating;
		}

		public EatingDay setEatingDay_allCallories(int _ID, int _allCalories)
		{
			if (_allCalories < 0) {
				return null;
			}

			var query = hedb.Table<EatingDay>().Where(x => x.ID == _ID);

			if (query.Count() == 0) {
				return null;
			}

			// Добавляем новый
			var _eating = query.First ();
			_eating.allCalories = _allCalories;
			hedb.Update (_eating);
			return _eating;
		}

		public EatingDay getEatingDay(int ID)
		{
			EatingDay result;
			try
			{
				var query = hedb.Table<EatingDay>().Where(x => x.ID == ID);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public EatingDay getEatingDay_by_Date(DateTime _date)
		{
			EatingDay result;
			try
			{
				var query = hedb.Table<EatingDay>().Where(x => x.eatingDate == _date);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public EatingDay getEatingDay_by_mealPlaneID(int _mealPlaneID)
		{
			EatingDay result;
			try
			{
				var query = hedb.Table<EatingDay>().Where(x => x.mealPlaneID == _mealPlaneID);
				result = query.First();
			}
			catch
			{
				result = null;
			}

			return result;
		}

		/*
		public List<EatingDay> getAllEatingDay()
		{
			var table = hedb.Table<EatingDay>();
			List<EatingDay> eatings = new List<EatingDay>();
			foreach (var item in table)
			{
				eatings.Add(item);
			}
			return eatings;
		}

		public int delEatingDay(int ID) {
			return hedb.Delete<EatingDay> (ID);
		}

		public int delAllEatingDay() {
			return hedb.DeleteAll<EatingDay> ();
		}
		*/
	}





	/*#######################################################################*/
	/*########################		 Сущности     ###########################*/
	/*#######################################################################*/

	/// <summary>
	/// Профиль пользователя
	/// </summary>
	public class Profile
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
        public string deviceID { get; set; }
        public string name { get; set; }
		public int current_weight { get; set; }
		public int desired_weight { get; set; }
		public int growth { get; set; }
		public int age { get; set; }
        public bool man { get; set; }
		public bool show_recommends { get; set; }
		public string Message { get; set; }
	}

	/// <summary>
	/// Профессия
	/// </summary>
	public class Profession
	{
		[PrimaryKey]
		public int profileID { get; set; }
		public string name { get; set; }
	}

    /// <summary>
    /// Образ жизни
    /// </summary>
    public class Lifestyle
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string name { get; set; }
    }

    /// <summary>
    /// Аллергенный продукт
    /// </summary>
    public class Allergic
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int profileID { get; set; }
        public int foodID { get; set; }
    }

    /// <summary>
    /// Продукт питания
    /// </summary>
    public class Food
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string name { get; set; }
        public float proteins { get; set; }
        public float fats { get; set; }
        public float carbs { get; set; } // Сокращение от carbohydrates (используется в языке)
        public float calories { get; set; }
        public int profileID { get; set; }
    }

    /// <summary>
    /// Продукт-порция
    /// </summary>
    public class FoodPortion
    {
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
        public int foodID { get; set; }
        public int count { get; set; } 
    }

	/// <summary>
	/// Лист продукт-порций для приема пищи и шаблонов приема пищи.
	/// </summary>
	public class FoodPortionList
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int portionID { get; set; }
		public int eatingID { get; set; } //Если для приема пищи ( тогда eatingTemplateID пуст )
		public int eatingTemplateID { get; set; } //Если для шаблона приема пищи ( тогда eatingID пуст )
	}

    /// <summary>
    /// Приём пищи
    /// </summary>
    public class Eating
    {
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
        public bool eaten { get; set; }
		public int eatingType { get; set; }
		public DateTime time { get; set; }
    }

    /// <summary>
    /// Шаблон приёма пищи
    /// </summary>
    public class EatingTemplate
    {
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
    }

	/// <summary>
	/// План питания
	/// </summary>
	public class MealPlane
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
	}

	/// <summary>
	/// Список приемов пищи для плана питания
	/// </summary>
	public class EatingList
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int mealPlaneID { get; set; }
		public int eatingID { get; set; }
	}

	/// <summary>
	/// День питания
	/// </summary>
	public class EatingDay
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int mealPlaneID { get; set; }
		public int allCalories { get; set; }
		public DateTime eatingDate { get; set; }
	}
}

