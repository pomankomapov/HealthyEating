using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.Res;

namespace healthy_eating
{
    // +------------------------------------------------------------------------------------------+
    // |                            Класс круговых диаграмм                                       |
    // +------------------------------------------------------------------------------------------+

    public class PieChart : View
    {
        protected float k1 = 0.94f;
        protected float k2 = 0.98f;
        protected float k3 = 0.70f;
        protected int padding = 5;
        protected float separator = 1.5f;
        protected float font_size = 18;

        public PieChart(Context context) : base (context)
        {
            init();
        }

        public PieChart(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            init();
        }

        public PieChart(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs)
        {
            init();
        }

        protected void init()
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            int minw = PaddingLeft + PaddingRight + SuggestedMinimumWidth;
            int w = ResolveSize(minw, widthMeasureSpec);

            // Whatever the width ends up being, ask for a height that would let the pie
            // get as big as it can
            int minh = MeasureSpec.GetSize(w) + PaddingBottom + PaddingTop;
            int h = ResolveSize(MeasureSpec.GetSize(w), heightMeasureSpec);

            SetMeasuredDimension(w, h);
        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            int x = this.Width;
            int y = this.Height;
            int ra = 100;

            Paint white = new Paint();
            white.AntiAlias = true;
            white.SetStyle(Paint.Style.Fill);
            white.Color = Color.White;
            canvas.DrawPaint(white);

            // Use Color.parseColor to define HTML colors
            Paint main1 = new Paint();
            main1.AntiAlias = true;
            main1.Color = Color.Rgb(166, 206, 57);

            Paint main2 = new Paint();
            main2.AntiAlias = true;
            main2.Color = Color.Rgb(43, 134, 213);

            Paint back = new Paint();
            back.AntiAlias = true;
            back.Color = Color.Rgb(224, 212, 187);

            draw_segment(canvas, ra + padding, ra + padding, ra, Global.percent1, main1, back, "Калории");
            draw_segment(canvas, 3*ra + 3*padding, ra + padding, ra, Global.percent2, main2, back, "Вода");

            Paint black = new Paint();
            black.AntiAlias = true;
            black.Color = Color.Black;
            black.TextSize = 25;

        }

        protected void draw_segment(Canvas canvas, float x, float y, float ra, float percents, Paint main, Paint back, string name)
        {
            RectF rect;
            float radius = ra;
            int angle = (int)(percents * 360);

            Paint gray = new Paint();
            gray.AntiAlias = true;
            gray.Color = Color.Gray;
            gray.TextSize = ra * k1 * k2 * k3 * 0.8f;

            Paint light_main = new Paint();
            light_main.AntiAlias = true;
            light_main.Color = main.Color;
            light_main.Alpha = (int)(0.6 * light_main.Alpha);

            Paint light_back = new Paint();
            light_back.AntiAlias = true;
            light_back.Color = back.Color;
            light_back.Alpha = (int)(0.6 * light_back.Alpha);

            Paint white = new Paint();
            white.AntiAlias = true;
            white.SetStyle(Paint.Style.Fill);
            white.Color = Color.White;

            rect = new RectF(x - ra, y - ra, x + ra, y + ra);
            canvas.DrawArc(rect, 0, 360 - angle, true, light_back);
            canvas.DrawArc(rect, 360 - angle, angle, true, light_main);
            ra *= k1;
            rect = new RectF(x - ra, y - ra, x + ra, y + ra);
            canvas.DrawArc(rect, 0, 360 - angle, true, white);
            canvas.DrawArc(rect, 360 - angle - 3, angle + 3, true, white);
            ra *= k2;
            rect = new RectF(x - ra, y - ra, x + ra, y + ra);
            canvas.DrawArc(rect, 0, 360 - angle, true, back);
            canvas.DrawArc(rect, 360 - angle, angle, true, main);
            ra *= k3;
            rect = new RectF(x - ra, y - ra, x + ra, y + ra);
            canvas.DrawArc(rect, 0, 360 - angle, true, white);
            canvas.DrawArc(rect, 360 - angle - 3, angle + 3, true, white);

            // Separator
            ra = radius;
            if ((int)(percents * 100 + .5) < 100)
            {
                rect = new RectF(x - ra, y - ra, x + ra, y + ra);
                canvas.DrawArc(rect, -separator / 2, separator, true, white);
                canvas.DrawArc(rect, 360 - angle - separator / 2, separator, true, white);
            }

            // Percents
            string text = string.Format("{0}%", (int)(percents * 100 + .5));
            canvas.DrawText(text,
                x - (int)(text.Length * gray.TextSize / 3.3),
                y + (int)(0.3 * gray.TextSize),
                gray);

            // Label
            float x0 = x - ra + padding;
            float y0 = y + ra + padding + font_size;
            rect = new RectF(x0, y0, x0 + font_size, y0 + font_size);
            canvas.DrawRect(rect, main);

            gray.TextSize = font_size;
            canvas.DrawText(name, x0 + font_size + padding, y0 + font_size - 2, gray);
        }
    }


    // +------------------------------------------------------------------------------------------+
    // |                                 Класс графиков                                           |
    // +------------------------------------------------------------------------------------------+

    public class Graph : View
    {
        public Graph(Context context) : base(context)
        {
            Initialize();
        }

        public Graph(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public Graph(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            Paint green = new Paint {
                AntiAlias = true,
                Color = Color.Rgb(0x99, 0xcc, 0),
            };
            green.SetStyle(Paint.Style.FillAndStroke);

            Paint red = new Paint {
                AntiAlias = true,
                Color = Color.Rgb(0xff, 0x44, 0x44)
            };
            red.SetStyle(Paint.Style.FillAndStroke);

            float middle = canvas.Width * 0.25f;
            canvas.DrawPaint(red);
            canvas.DrawRect(0, 0, middle, canvas.Height, green);
        }
    }

    // +------------------------------------------------------------------------------------------+
    // |                               Класс линейных чартов                                      |
    // +------------------------------------------------------------------------------------------+

    public class LinChart : View
    {
        protected int padding = 5;
        protected int margin = 10;
        protected int width = 15;
        protected int max_val = 1200;
        protected int max_len = 300;
        protected float effect3d = 0.2f;
        protected float separator = 1.5f;
        protected float font_size = 18;

        public LinChart(Context context) : base (context)
        {
            init();
        }

        public LinChart(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            init();
        }

        public LinChart(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs)
        {
            init();
        }

        protected void init()
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            int minw = PaddingLeft + PaddingRight + SuggestedMinimumWidth;
            int w = ResolveSize(minw, widthMeasureSpec);

            // Whatever the width ends up being, ask for a height that would let the pie
            // get as big as it can
            int minh = MeasureSpec.GetSize(w) + PaddingBottom + PaddingTop;
            int h = ResolveSize(MeasureSpec.GetSize(w), heightMeasureSpec);

            SetMeasuredDimension(w, h);
        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            int x = this.Width;
            int y = this.Height;
            int ra = 100;

            Paint white = new Paint();
            white.AntiAlias = true;
            white.SetStyle(Paint.Style.Fill);
            white.Color = Color.White;
            canvas.DrawPaint(white);

            Paint main1 = new Paint();
            main1.AntiAlias = true;
            main1.Color = Color.Rgb(166, 206, 57);
            main1.SetStyle(Paint.Style.Fill);

            Paint main2 = new Paint();
            main2.AntiAlias = true;
            main2.Color = Color.Rgb(43, 134, 213);
            main2.SetStyle(Paint.Style.Fill);

            Paint main3 = new Paint();
            main3.AntiAlias = true;
            main3.Color = Color.Rgb(255, 99, 71);
            main3.SetStyle(Paint.Style.Fill);

            Paint back = new Paint();
            back.AntiAlias = true;
            back.Color = Color.Rgb(224, 212, 187);

            draw_chart(canvas, padding, padding, Global.linechart1, main1, back, "Белки");
            draw_chart(canvas, padding, padding + width + margin, Global.linechart2, main2, back, "Жиры");
            draw_chart(canvas, padding, padding + 2*(width + margin), Global.linechart3, main3, back, "Углеводы");

            Paint black = new Paint();
            black.AntiAlias = true;
            black.Color = Color.Black;
            black.TextSize = 25;
        }

        protected void draw_chart(Canvas canvas, float x, float y, float val, Paint main, Paint back, string name)
        {
            RectF rect;

            Paint white = new Paint();
            white.AntiAlias = true;
            white.SetStyle(Paint.Style.Fill);
            white.Color = Color.White;

            Paint gray = new Paint();
            gray.AntiAlias = true;
            gray.Color = Color.Gray;
            gray.TextSize = font_size;

            Paint light_main = new Paint();
            light_main.AntiAlias = true;
            light_main.Color = main.Color;
            light_main.Alpha = (int)(0.6 * light_main.Alpha);

            Paint light_back = new Paint();
            light_back.AntiAlias = true;
            light_back.Color = back.Color;
            light_back.Alpha = (int)(0.6 * light_back.Alpha);

            ///////////////

            rect = new RectF(x, y + width * effect3d, x + max_len, y + width);
            canvas.DrawRect(rect, back);

            rect = new RectF(x, y, x + max_len, y + width * effect3d);
            canvas.DrawRect(rect, light_back);

            ///////////////

            rect = new RectF(x, y + width * effect3d, x + max_len * val / max_val  + separator, y + width);
            canvas.DrawRect(rect, white);

            rect = new RectF(x, y, x + max_len * val / max_val + separator, y + width * effect3d);
            canvas.DrawRect(rect, white);

            ///////////////

            rect = new RectF(x, y + width * effect3d, x + max_len * val / max_val, y + width);
            canvas.DrawRect(rect, main);

            rect = new RectF(x, y, x + max_len * val / max_val, y + width * effect3d);
            canvas.DrawRect(rect, light_main);

            ///////////////

            // Подписываем чарт
            float x0 = x + max_len + margin;
            float y0 = y + width;

            canvas.DrawText(name, x0, y0, gray);
        }
    }

}

