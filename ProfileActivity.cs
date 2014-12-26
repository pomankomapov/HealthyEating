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
        protected Button       btn_apply;         // Кнопка применения
        protected Button       btn_cancel;        // Кнопка отмены

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

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
            btn_apply         = FindViewById <Button>      (Resource.Id.button_apply);
            btn_cancel        = FindViewById <Button>      (Resource.Id.button_cancel);

			// Назначаем действия /////////////////////////////////////////////////

			skb_length.ProgressChanged += (sender, e) => {
				edt_length.Text = string.Format("{0}", skb_length.Progress);
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

            skb_weight.ProgressChanged += (sender, e) => {
                edt_weight.Text = string.Format("{0}", skb_weight.Progress);
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

            skb_target_weight.ProgressChanged += (sender, e) => {
                edt_target_weight.Text = string.Format("{0}", skb_target_weight.Progress);
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
            spn_allergic.ItemSelected += (sender, e) => {
            };

            adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.allergic, 
                Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spn_allergic.Adapter = adapter;

            // Кнопки применения и отмены
            btn_apply.Click += (sender, e) => {
                save_data();
            };

            btn_cancel.Click += (sender, e) => {
                base.OnBackPressed();
            };

			// Задачи во время запуска активности /////////////////////////////////
		}

        protected void load_data()
        {

        }

        protected void save_data()
        {
        }
	}
}

