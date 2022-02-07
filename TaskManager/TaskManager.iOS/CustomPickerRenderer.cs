using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using TaskManager.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRender))]
namespace TaskManager.iOS
{
    class CustomPickerRender : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            var view = e.NewElement as Picker;
            this.Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}