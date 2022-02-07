using TaskManager.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
namespace TaskManager.iOS
{
    class CustomEditorRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.CornerRadius = 10;
                Control.TextColor = UIColor.Black;
            }
        }
    }