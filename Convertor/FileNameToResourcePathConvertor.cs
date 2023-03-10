using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (string.IsNullOrEmpty(filename))
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
