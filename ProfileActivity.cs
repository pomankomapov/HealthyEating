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
	[Activity (Label = "Профиль")]
	public class ProfileActivity : Activity
	{
        static HEDB database = new HEDB();
		protected EditText     edt_name;          // Имя
		protected RadioButton  rad_man;           // Пол мужской
		protected RadioButton  rad_woman;         // Пол женский
		protected EditText     edt_length;        // Рост
		protected SeekBar      skb_length;        // Прокрутка роста
        protected EditText     edt_weight;        // Вес
        protected SeekBar      skb_weight;        // Прокрутка веса
        protected EditText     edt_target_weight; // Целевой вес
        protected SeekBar      skb_target_weight; // Прокрутка целевого веса
        protected Spinner      spn_lifestyle;     // Образ жизни
        protected Spinner      spn_allergic;      // Группы аллегренных продуктов
        protected Button       btn_choose;        // Выбрать аллергенный продукт
        protected Button       btn_delete;        // Удалить продукт из списка аллергенных
        protected Button       btn_apply;         // Кнопка применения
        protected Button       btn_cancel;        // Кнопка отмены
        private const int      min_length = 100;  // Минимальный рост
        private const int      min_weight = 35;   // Минимальный вес
        private const int      min_target_weight = 35;    // Минимальный целевой вес

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Убираем автофокус
            this.Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

			// Ставим нужный layout ///////////////////////////////////////////////

			SetContentView (Resource.Layout.profile);

			// Получаем все контролы //////////////////////////////////////////////

			edt_name          = FindViewById <EditText>    (Resource.Id.edit_name);
			rad_man           = FindViewById <RadioButton> (Resource.Id.radio_man);
			rad_woman         = FindViewById <RadioButton> (Resource.Id.radio_woman);
			edt_length        = FindViewById <EditText>    (Resource.Id.edit_length);
			skb_length        = FindViewById <SeekBar>     (Resource.Id.seekbar_length);
            edt_weight        = FindViewById <EditText>    (Resource.Id.edit_weight);
            skb_weight        = FindViewById <SeekBar>     (Resource.Id.seekbar_weight);
            edt_target_weight = FindViewById <EditText>    (Resource.Id.edit_target_weight);
            skb_target_weight = FindViewById <SeekBar>     (Resource.Id.seekbar_target_weight);
            spn_lifestyle     = FindViewById <Spinner>     (Resource.Id.spinner_lifestyle);
            spn_allergic      = FindViewById <Spinner>     (Resource.Id.spinner_allergic);
            btn_choose        = FindViewById <Button>      (Resource.Id.button_choose_food);
            btn_delete        = FindViewById <Button>      (Resource.Id.button_delete);
            btn_apply         = FindViewById <Button>      (Resource.Id.button_apply);
            btn_cancel        = FindViewById <Button>      (Resource.Id.button_cancel);

			// Назначаем действия /////////////////////////////////////////////////

            //Контролы роста
			skb_length.ProgressChanged += (sender, e) => {
                int new_length = min_length + skb_length.Progress;
                edt_length.Text = string.Format("{0}", new_length);
			};

			edt_length.KeyPress += (sender, e) => {
				e.Handled = false;
				if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
				{
					int value;
					if (Int32.TryParse(edt_length.Text, out value))
					{
						if (value > skb_length.Max) edt_length.Text = string.Format("{0}", skb_length.Max);
						else if (value < 0)         edt_length.Text = "0";
						skb_length.Progress = value;
					}
					e.Handled = true;
				}
			};

            //Контролы веса
            skb_weight.ProgressChanged += (sender, e) => {
                int new_weight = min_weight + skb_weight.Progress;
                edt_weight.Text = string.Format("{0}", new_weight);
            };

            edt_weight.KeyPress += (sender, e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    int value;
                    if (Int32.TryParse(edt_weight.Text, out value))
                    {
                        if (value > skb_weight.Max) edt_weight.Text = string.Format("{0}", skb_weight.Max);
                        else if (value < 0)         edt_weight.Text = "0";
                        skb_weight.Progress = value;
                    }
                    e.Handled = true;
                }
            };

            //Контролы желаемого веса
            skb_target_weight.ProgressChanged += (sender, e) => {
                int new_target_weight = min_target_weight + skb_target_weight.Progress;
                edt_target_weight.Text = string.Format("{0}", new_target_weight);
            };

            edt_target_weight.KeyPress += (sender, e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    int value;
                    if (Int32.TryParse(edt_target_weight.Text, out value))
                    {
                        if (value > skb_target_weight.Max) edt_target_weight.Text = string.Format("{0}", skb_target_weight.Max);
                        else if (value < 0)                edt_target_weight.Text = "0";
                        skb_target_weight.Progress = value;
                    }
                    e.Handled = true;
                }
            };

            // Образ жизни
            spn_lifestyle.ItemSelected += (sender, e) => {
            };

            var adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.lifestyles, 
                                                           Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spn_lifestyle.Adapter = adapter;

            // Аллергенные продукты
            btn_choose.Click += (sender, e) => {
                var activity = new Intent (this, typeof(ListFood));
                activity.PutExtra("mode", "Allergic");
                StartActivity(activity);
            };

            btn_delete.Click += (sender, e) => {
                // Если аллергенные продукты существуют для данного пользователя
                if (database.getAllergic(Global.userID) != null)
                {
                    string name = spn_allergic.SelectedItem.ToString();
                    Food food = database.findFood(name);

                    // Найден продукт
                    if (food != null)
                    {
                        database.delAllergic(Global.userID, food.ID);
                        update_allergic();
                    }
                }
            };

            // Кнопки применения и отмены
            btn_apply.Click += (sender, e) => {
                if (save_data())
                    base.OnBackPressed();
            };

            btn_cancel.Click += (sender, e) => {
                base.OnBackPressed();
            };

			// Задачи во время запуска активности /////////////////////////////////
		}

        protected override void OnResume()
        {
            // Always call the superclass first
            base.OnResume();

            load_data();
        }

        protected void load_data()
        {
            int profileID = Global.userID;

            // Если профиль был найден
            if (profileID != int.MaxValue)
            {
                Profile profile = database.getProfile(profileID);

                edt_name.Text = profile.name;
                rad_man.Checked = profile.man;
                rad_woman.Checked = !profile.man;
                edt_length.Text = string.Format("{0}", profile.growth);
                edt_weight.Text = string.Format("{0}", profile.current_weight);
                edt_target_weight.Text = string.Format("{0}", profile.desired_weight);
                skb_length.Progress = profile.growth - min_length;
                skb_weight.Progress = profile.current_weight - min_weight;
                skb_target_weight.Progress = profile.desired_weight - min_target_weight;

                update_allergic();
            }
        }

        protected void update_allergic()
        {
            // Добываем список аллергенных продуктов
            List<string> allergics = database.getAllAllergicNames(Global.userID);

            var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, allergics);
            adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spn_allergic.Adapter = adapter;
            spn_allergic.Selected = false;
        }

        protected bool save_data()
        {
            Profile profile;
            if (Global.userID == int.MaxValue) profile = new Profile();
            else
            {
                profile = database.getProfile(Global.userID);
            }

            // Имя /////////////////////////////////////////////////////////////////////////////////////////

            string name = edt_name.Text;
            if (string.IsNullOrEmpty(name))
            {
                Global.print(this, "Введите имя");
                return false;
            }

            // Пол /////////////////////////////////////////////////////////////////////////////////////////

            bool man = false;
            if (rad_man.Checked)
            {
                man = true;
            }
            else if (rad_woman.Checked)
            {
                man = false;
            }
            else
            {
                Global.print(this, "Выберите пол");
                return false;
            }

            // Рост ////////////////////////////////////////////////////////////////////////////////////////

            int growth = 0;
            int.TryParse(edt_length.Text, out growth);

            // Вес /////////////////////////////////////////////////////////////////////////////////////////

            int current_weight, desired_weight;
            current_weight = desired_weight = 0;

            int.TryParse(edt_weight.Text, out current_weight);
            int.TryParse(edt_target_weight.Text, out desired_weight);

            // Сохранение данных ///////////////////////////////////////////////////////////////////////////

            // DEBUG: вывод номера устройства
            //Android.Widget.Toast.MakeText(this, Global.deviceID, Android.Widget.ToastLength.Short).Show();

            // Запись существует
            if (Global.userID != int.MaxValue) database.delProfile(Global.userID);

            Global.userID = database.addProfile(Global.deviceID, name, current_weight, 
                                                desired_weight, growth, 0, man, false).ID;

            return true;
        }
	}
}

