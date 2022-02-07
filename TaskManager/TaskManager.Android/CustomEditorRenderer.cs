using Android.Content;
using TaskManager.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
namespace TaskManager.Droid
{
    class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}