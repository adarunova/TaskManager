using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.ViewModels
{
    public class CalendarCarouselViewModel : BaseViewModel
    {
        private string _backgroundColor;

        private string _textColor;

        public string Month { get; set; }

        public string Day { get; set; }

        public string DayOfWeek { get; set; }

        public bool Selected { get; set; }

        public string BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        public string TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }
    }
}
