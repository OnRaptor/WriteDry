using System;
using System.Globalization;
using System.Windows.Data;

namespace WriteDry.Convertor
{
    public class FileNameToResourcePathConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string)
                throw new NotImplementedException();
            string filename = (string)value;
            if (string.IsNullOrWhiteSpace(filename))
                return "/Assets/picture.png";
            else
                return "/Assets/DB/" + filename;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
