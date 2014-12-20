using System;
using SQLite;

namespace healthy_eating
{
	public class HEDB
	{
		private SQLiteConnection hedb;

		public HEDB ()
		{
			initializeDatabase();
		}

		private void initializeDatabase() {
			// Создание или подключение к базе HelthyEating.db
			string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			hedb = new SQLiteConnection (System.IO.Path.Combine (folder, "HelthyEating.db"));

			// Создаем таблицу прифилей
			hedb.CreateTable<Profile>();
			// Создаем таблицу прифилей
			hedb.CreateTable<Profession>();

		}

		/// <summary>
		/// Добавляет новый "профиль пользователя" в базу.
		/// </summary>
		public Profile addProfile() {
			// Добавляем новый профиль
			var _profile = new Profile { Message = "Test profile!!!" };
			hedb.Insert (_profile);
			return _profile;
			// Show the automatically set ID and message.
			// Console.WriteLine ("{0}: {1}", _profile.ID, _profile.Message);
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

	}

	/// <summary>
	/// Прифиль пользователя
	/// </summary>
	public class Profile
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int current_weight { get; set; }
		public int desired_weight { get; set; }
		public int growth { get; set; }
		public int age { get; set; }
		public int show_recommends { get; set; }
		public string Message { get; set; }
	}

	/// <summary>
	/// Прифиль пользователя
	/// </summary>
	public class Profession
	{
		[PrimaryKey]
		public int ProfileID { get; set; }
		public string name { get; set; }
	}

}

