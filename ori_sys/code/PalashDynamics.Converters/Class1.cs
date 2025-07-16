using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;
using System.Threading;
using CIMS;
using PalashDynamics;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;

namespace PalashDynamics.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value != null && parameter != null && value.ToString() != String.Empty)
            {
                DateTime date = DateTime.Parse(value.ToString());
                //return date.ToShortDateString();

                return date.ToString(parameter.ToString());

            }
            else
                return string.Empty;

            //if (value != null && parameter != null)
            //{
            //    DateTime date = (DateTime)value;
            //    //return date.ToShortDateString();
                
            //        return date.ToString(parameter.ToString());
           
            //}
            //else
            //    return string.Empty;

        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            if (value != null)
            {
                string strValue = value.ToString();
                DateTime resultDateTime;
                if (DateTime.TryParse(strValue, out resultDateTime))
                {
                    return resultDateTime;
                }
            }
            return value;
        }
    }

    public class ToggleBooleanValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool val = (bool)value;

                return !val; // Math.Round(val, 2).ToString();
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisiblilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility isVisible = Visibility.Collapsed;

            if (parameter != null && parameter.ToString().ToUpper() == "AMT")
            {
                if (value == null)
                    return isVisible; // Math.Round(val, 2).ToString();
                else
                {
                    double mValue = (double)value;
                    if (mValue > 0)
                        isVisible = Visibility.Visible;
                    else
                        isVisible = Visibility.Collapsed;
                    return isVisible;
                }
            }
            else
            {
                if (value == null)
                    return isVisible; // Math.Round(val, 2).ToString();
                else
                {
                    bool condition = (bool)value;
                    isVisible = condition == true ? Visibility.Visible : Visibility.Collapsed;
                    return isVisible;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class DateTimeToAgePart : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    DateTime BirthDate = (DateTime)value;
                    TimeSpan difference = DateTime.Now.Subtract(BirthDate);
                    
                    //return date.ToString(parameter.ToString());
                    // This is to convert the timespan to datetime object
                    DateTime age = DateTime.MinValue + difference;

                    // Min value is 01/01/0001
                    // Actual age is say 24 yrs, 9 months and 3 days represented as timespan
                    // Min Valye + actual age = 25 yrs , 10 months and 4 days.
                    // subtract our addition or 1 on all components to get the actual date.
                    string result = "";
                    switch (parameter.ToString().ToUpper())
                    {
                        case "YY":
                            result = (age.Year - 1).ToString();
                            break;
                        case "MM":
                            result = (age.Month - 1).ToString();
                            break;
                        case "DD":
                            result = (age.Day - 1).ToString();
                            break;
                        default:
                            result = (age.Year - 1).ToString();
                            break;

                    }
                    return result;
          
                }
               catch (Exception ex)
                {
                   return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    // DateTime BirthDate = (DateTime)value;
                    DateTime BirthDate = DateTime.Now;

                    // TimeSpan difference = DateTime.Now.Subtract(BirthDate);
                    //return date.ToString(parameter.ToString());
                    // This is to convert the timespan to datetime object
                    //DateTime age = DateTime.MinValue + difference;

                    // Min value is 01/01/0001
                    // Actual age is say 24 yrs, 9 months and 3 days represented as timespan
                    // Min Valye + actual age = 25 yrs , 10 months and 4 days.
                    // subtract our addition or 1 on all components to get the actual date.
                    int mValue = Int32.Parse(value.ToString());


                    switch (parameter.ToString().ToUpper())
                    {
                        case "YY":
                            BirthDate = BirthDate.AddYears(-mValue);
                            break;
                        case "MM":
                            BirthDate = BirthDate.AddMonths(-mValue);
                            // result = (age.Month - 1).ToString();
                            break;
                        case "DD":
                            //result = (age.Day - 1).ToString();
                            BirthDate = BirthDate.AddDays(-mValue);
                            break;
                        default:
                            BirthDate = BirthDate.AddYears(-mValue);
                            break;
                    }
                    return BirthDate;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            else
                return string.Empty;
            //return "";
        }
    }

    public class IDToDescriptionConvertor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string HPLevel="";
            if (value!=null)
            {
                switch (value.ToString())
                {
                    case "1":
                        HPLevel = "Level I";
                        break;
                    case "2":
                        HPLevel = "Level II";
                        break;
                    case "3":
                        HPLevel = "Level III";
                        break;
                    default:
                          HPLevel = "Select";
                        break;
                }
               
            }
            return HPLevel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class DecimalRoundConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value != null)
            {
                Decimal val = (Decimal)value;

                return Math.Round(val,2).ToString(); 
            }
            else
                return string.Empty;

        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ApplicableOnIDToDescriptionConvertor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ApplicableOn = "";
            if (value != null)
            {
                switch (value.ToString())
                {
                    case "1":
                        ApplicableOn = "Purchase Rate";
                        break;
                    case "2":
                        ApplicableOn = "MRP";
                        break;
                  
                    default:
                        ApplicableOn = "Select";
                        break;
                }

            }
            return ApplicableOn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class ApplicableForIDToDescriptionConvertor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ApplicableFor = "";
            if (value != null)
            {
                switch (value.ToString())
                {
                    case "1":
                        ApplicableFor = "Purchase";
                        break;
                    case "2":
                        ApplicableFor = "Sale";
                        break;
                  
                    default:
                        ApplicableFor = "Select";
                        break;
                }

            }
            return ApplicableFor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PieColorConverter : IValueConverter
    {
        private SolidColorBrush Default = new SolidColorBrush(Colors.Black);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (!(value is string))) { return Default; }

            switch (value as string)
            {
                case "Cash":
                    return new SolidColorBrush(Colors.Brown);
                case "CreditCard":
                    return new SolidColorBrush(Colors.Cyan);
                case "DD":
                    return new SolidColorBrush(Colors.DarkGray);
                case "Cheque":
                    return new SolidColorBrush(Colors.LightGray);
                case "DebitCard":
                    return new SolidColorBrush(Colors.Purple);
                case "StaffFree":
                    return new SolidColorBrush(Colors.Transparent);
                case "Credit":
                    return new SolidColorBrush(Colors.Yellow);
                case "CompanyAdvance":
                    return new SolidColorBrush(Colors.Blue);
                case "PatientAdvance":
                    return new SolidColorBrush(Colors.Orange);
                default:
                    return Default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class clsColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush color;
            if (value != null)
            {
                if (value.Equals(true))
                {
                    color = new SolidColorBrush(Colors.Red);
                    return color;
                }
                else
                {
                    color = new SolidColorBrush(Colors.Black);
                    return color;
                }
            }
            else
            {
                color = new SolidColorBrush(Colors.Black);
                return color;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = "dd/MM/yyyy";
           // parameter = (((IApplicationConfiguration)App.Current).ApplicationConfigurations.DateFormat);
         
            if (value != null && parameter != null)
            {
                DateTime date = (DateTime)value;
                //return date.ToShortDateString();

                return date.ToString(parameter.ToString());

            }
            else
                return string.Empty;

        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            if (value != null)
            {
                string strValue = value.ToString();
                DateTime resultDateTime;
                if (DateTime.TryParse(strValue, out resultDateTime))
                {
                    return resultDateTime;
                }
            }
            return value;
        }
    }

    public class TimeConverter : IValueConverter
    {

        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                DateTime date = (DateTime)value;
                //return date.ToShortDateString();

                return date.ToString(parameter.ToString());


            }
            else
                return string.Empty;

        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            if (value != null)
            {
                string strValue = value.ToString();
                DateTime resultDateTime;
                if (DateTime.TryParse(strValue, out resultDateTime))
                {
                    return resultDateTime;
                }
            }
            return value;
        }
    }

    public class DateTimeConverterGlobal : IValueConverter
    {

        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                DateTime date = (DateTime)value;
                //return date.ToShortDateString();

                //return date.ToString(parameter.ToString());
                return date.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToString());

            }
            else
                return string.Empty;

        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            if (value != null)
            {
                string strValue = value.ToString();
                DateTime resultDateTime;
                if (DateTime.TryParse(strValue, out resultDateTime))
                {
                    return resultDateTime;
                }
            }
            return value;
        }
    }
    public class camelcase1 : IValueConverter
    {
        private string toTitleCase(string value)
        {
            if (value == null)
                return null;
            if (value.Length == 0)
                return value;
            StringBuilder result = new StringBuilder(value);
            result[0] = char.ToUpper(result[0]);
            for (int i = 1; i < result.Length; ++i)
            {
                if (char.IsWhiteSpace(result[i - 1]))
                    result[i] = char.ToUpper(result[i]);
                else
                    result[i] = char.ToLower(result[i]);
            }
            return result.ToString();
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result=null;
            if (value != null)
            {
                result = toTitleCase(value as string);
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ByteToImageConverter : IValueConverter
    {

        public BitmapImage ConvertByteArrayToBitMapImage(byte[] imageByteArray)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(imageByteArray))
            {
                img.SetSource(memStream);
            }
            return img;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage img = new BitmapImage();
            if (value != null)
            {
                img = this.ConvertByteArrayToBitMapImage(value as byte[]);
            }
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
