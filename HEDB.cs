using System;
using SQLite;
using System.Collections.Generic;

namespace healthy_eating
{
    // Healthy Eating DataBase
	public class HEDB
	{
		private SQLiteConnection hedb;

        // Блок инициализации /////////////////////////////////////////////////////////////////////////////////////

		public HEDB ()
		{
			initializeDatabase();
		}

		private void initializeDatabase() {
			// Создание или подключение к базе HelthyEating.db
			string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			hedb = new SQLiteConnection (System.IO.Path.Combine (folder, "HelthyEating.db"));

            hedb.CreateTable<Profile>();     // Создаем таблицу профилей
            hedb.CreateTable<Profession>();  // Создаем таблицу профессий
            hedb.CreateTable<Lifestyle>();   // Создаем таблицу образов жизни
            hedb.CreateTable<Food>();        // Создаем таблицу продуктов
            hedb.CreateTable<FoodPortion>(); // Создаем таблицу продукт-порция
            hedb.CreateTable<Eating>();      // Создаем таблицу приём пищи

		}

        // Методы для сущности "Профиль" ///////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавляет новый "профиль пользователя" в базу.
		/// </summary>
		public Profile addProfile(float _current_wight, float _desired_width, 
                                  int _growth, int _age, bool _show_recommends)
        {
			// Добавляем новый профиль
			var _profile = new Profile {
				current_weight = _current_wight,
				desired_weight = _desired_width,
				growth = _growth,
				age = _age,
				show_recommends = _show_recommends
			};
			hedb.Insert (_profile);
			return _profile;
			// Show the automatically set ID and message.
			// Console.WriteLine ("{0}: {1}", _profile.ID, _profile.Message);
		}

        public Profile getProfile(int ID)
        {
            var query = hedb.Table<Profile>().Where(x => x.ID == ID);
            var result = query.ElementAt(0);

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

        // Методы для сущности "Профессия" ////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Добавляет новую "профессию" в базу по заданному "профилю"(Profile).
		/// </summary>
		public Profession addProfession(Profile _profile, string _name) {
			// Добавляем новый профиль
			var _profession = new Profession {
				ProfileID = _profile.ID,
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

        // Методы для сущности "Образ жизни" ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Adds the lifestyle.
        /// </summary>
        /// <returns>The lifestyle.</returns>
        /// <param name="_ID">I.</param>
        /// <param name="_name">Name.</param>
        public Lifestyle addLifestyle(int _ID, string _name)
        {
            // Добавляем новый профиль
            var _lifestyle = new Lifestyle {
                ID = _ID,
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

        // Методы для сущности "Продукты" /////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Adds the food.
        /// </summary>
        /// <returns>The food.</returns>
        /// <param name="_ID">I.</param>
        /// <param name="_name">Name.</param>
        /// <param name="_proteins">Proteins.</param>
        /// <param name="_fats">Fats.</param>
        /// <param name="_carbs">Carbs.</param>
        /// <param name="_calories">Calories.</param>
        /// <param name="_ProfileID">Profile I.</param>
        public Food addFood(int _ID, string _name, int _proteins, int _fats, int _carbs, int _calories, int _ProfileID)
        {
            // Добавляем новый профиль
            var _food = new Food {
                 ID        = _ID,
                 name      = _name, 
                 proteins  = _proteins,  
                 fats      = _fats, 
                 carbs     = _carbs,
                 calories  = _calories, 
                 ProfileID = _ProfileID
            };
            hedb.Insert (_food);
            return _food;
        }

        public Food getFood(int ID)
        {
            var query = hedb.Table<Food>().Where(x => x.ID == ID);
            var result = query.ElementAt(0);

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

	}

    // Сущности ///////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Профиль пользователя
	/// </summary>
	public class Profile
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
        public string name { get; set; }
		public float current_weight { get; set; }
		public float desired_weight { get; set; }
		public int growth { get; set; }
		public int age { get; set; }
		public bool show_recommends { get; set; }
		public string Message { get; set; }
	}

	/// <summary>
	/// Профессия
	/// </summary>
	public class Profession
	{
		[PrimaryKey]
		public int ProfileID { get; set; }
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
    /// Продукт питания
    /// </summary>
    public class Food
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string name { get; set; }
        public int proteins { get; set; }
        public int fats { get; set; }
        public int carbs { get; set; } // Сокращение от carbohydrates (используется в языке)
        public int calories { get; set; }
        public int ProfileID { get; set; }
    }

    /// <summary>
    /// Продукт-порция
    /// </summary>
    public class FoodPortion
    {
        [PrimaryKey]
        public int FoodID { get; set; }
        public int count { get; set; } 
    }

    /// <summary>
    /// Приём пищи
    /// </summary>
    public class Eating
    {
        [PrimaryKey]
        public int FoodPortionID { get; set; }
        public bool eaten { get; set; } 
        public DateTime time { get; set; }
    }

    /// <summary>
    /// Шаблон приёма пищи
    /// </summary>
    public class EatingTemplate
    {
        [PrimaryKey]
        public int FoodPortionID { get; set; } // ТУТ ДОЛЖЕН БЫТЬ СПИСОК!
    }

    // Ещё таблички "План питания" и "День питаний"

}

