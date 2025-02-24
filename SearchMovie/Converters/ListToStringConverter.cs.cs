using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchMovie.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable list)
            {
                var items = list.Cast<object>().Select(item => item.ToString());
                var separator = parameter as string ?? ", "; // Разделитель можно передавать через XAML
                return string.Join(separator, items);
            }
            return "Нет данных";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Обычно ConvertBack не нужен
        }
    }
}
