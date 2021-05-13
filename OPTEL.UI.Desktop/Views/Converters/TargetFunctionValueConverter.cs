using EasyLocalization.Localization;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OPTEL.UI.Desktop.Views.Converters
{
    public class TargetFunctionValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            if (value is TimeSpan)
            {
                TimeSpan time = (TimeSpan)value;
                result = string.Format(LocalizationManager.Instance.GetValue("Window.Main.GanttChart.TimeTargetFunction"), time.ToString("dd"), time.ToString("hh\\:mm\\:ss"));
            }
            if (value is double)
            {
                result = string.Format(LocalizationManager.Instance.GetValue("Window.Main.GanttChart.CostTargetFunction"), (double)value);
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
