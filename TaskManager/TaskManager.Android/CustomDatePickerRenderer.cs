using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TaskManager.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.DatePicker), typeof(CustomDatePickerRenderer))]
namespace TaskManager.Droid
{
    class CustomDatePickerRenderer : DatePickerRenderer
    {
        public CustomDatePickerRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}