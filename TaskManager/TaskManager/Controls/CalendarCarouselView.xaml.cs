using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskManager.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarCarouselView : ContentView
    {
        public ObservableCollection<Date> ItemsSource { get; set; }

        public static readonly BindableProperty ItemsSourceProperty =
           BindableProperty.Create(
               propertyName: "ItemsSource",
               returnType: typeof(ObservableCollection<Date>),
               declaringType: typeof(CalendarCarouselView),
               defaultValue: null,
               defaultBindingMode: BindingMode.TwoWay,
               propertyChanged: ItemsSourcePropertyChanged);

        public CalendarCarouselView()
        {
            InitializeComponent();
        }

        private static async void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var items = newValue as ObservableCollection<Date>;
            var control = (CalendarCarouselView)bindable;

            var index = items.ToList().FindIndex(v => v.Selected);

            await Task.Delay(250);

            if (index > -1)
            {
                control.listDates.ScrollTo(index, -1, ScrollToPosition.MakeVisible, true);
            }
        }

    }
}