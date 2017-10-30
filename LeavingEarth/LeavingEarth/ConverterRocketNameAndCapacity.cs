using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LeavingEarth
{
    class ConverterRocketNameAndCapacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string retval = "null";
            if (value != null)
            {
                if (value is Rocket)
                {
                    Rocket r = (Rocket)value;
                    retval = r.Name;
                    if (parameter != null)
                    {
                        DifficultyLevel d = (DifficultyLevel)parameter;
                        retval += "(" + r.GetMaxPayload(d) + "T)";
                    }
                }
                else
                {
                    retval = value.GetType().ToString();
                }
            }
            return retval;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
