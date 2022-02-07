using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using TaskManager.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace TaskManager.iOS
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.CornerRadius = 10;
                Control.TextColor = UIColor.White;
            }
        }
    }
}