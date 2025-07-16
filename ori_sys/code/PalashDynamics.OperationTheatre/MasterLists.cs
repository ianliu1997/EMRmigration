using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using PalashDynamics.ValueObjects;
using System.ComponentModel.DataAnnotations;
using PalashDynamics.OperationTheatre;

namespace PalashDynamics.OperationTheatre
{
    public interface IGetList
    {
        List<DynamicListItem> GetList();
    }

    public class Source1 : IGetList
    {


        #region IGetList Members

        public List<DynamicListItem> GetList()
        {
            List<DynamicListItem> list = new List<DynamicListItem>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add(new DynamicListItem() { Id = 1, Title = "Item"+i.ToString() });
            //}
            list.Add(new DynamicListItem() { Id = 1, Title = "50 to 100 ml after each loose stool =/> 2 years" });
            list.Add(new DynamicListItem() { Id = 2, Title = "100 to 200 ml after each loose stool =/<2 years" });
            return list;
        }

        #endregion
    }

    public class Source2 : IGetList
    {


        #region IGetList Members

        public List<DynamicListItem> GetList()
        {
            List<DynamicListItem> list = new List<DynamicListItem>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add(new DynamicListItem() { Id = 1, Title = "Item" + i.ToString() });
            //}
            list.Add(new DynamicListItem() { Id = 1, Title = "200-400ml (<6kg OR up to 4 months)" });
            list.Add(new DynamicListItem() { Id = 2, Title = "400-700ml (6kg to 10kg OR 4 to 12 months)" });
            list.Add(new DynamicListItem() { Id = 3, Title = "700-900ml (10kg to 12kg OR 12months to 2 yrs)" });
            list.Add(new DynamicListItem() { Id = 4, Title = "900-1400ml (12kg to 19kg OR 2yrs to 5yrs)" });
            return list;
        }

        #endregion
    }

    public class Source3 : IGetList
    {


        #region IGetList Members

        public List<DynamicListItem> GetList()
        {
            List<DynamicListItem> list = new List<DynamicListItem>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add(new DynamicListItem() { Id = 1, Title = "Item" + i.ToString() });
            //}
            list.Add(new DynamicListItem() { Id = 1, Title = "Ringers lactate, 30ml/kg in first 1hr, then 70ml/kg in next 5 hours" });
            list.Add(new DynamicListItem() { Id = 2, Title = "Ringers lactate, 30ml/kg in first 30 minutes, then 70ml/kg in next 2.5 hrs" });
            list.Add(new DynamicListItem() { Id = 3, Title = "Normal saline, 0.9% 30ml/kg in first 1hr, then 70ml/kg in next 5 hours" });
            list.Add(new DynamicListItem() { Id = 4, Title = "Normal saline, 0.9% 30ml/kg in first 30 minutes, then 70ml/kg in next 2.5 hrs" });
            return list;
        }

        #endregion
    }

    public class Source4 : IGetList
    {


        #region IGetList Members

        public List<DynamicListItem> GetList()
        {
            List<DynamicListItem> list = new List<DynamicListItem>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add(new DynamicListItem() { Id = 1, Title = "Item" + i.ToString() });
            //}
            list.Add(new DynamicListItem() { Id = 1, Title = "1/2 tablets (10mg) per day for 12 days" });

            return list;
        }

        #endregion
    }

    public class Source5 : IGetList
    {

        #region IGetList Members

        public List<DynamicListItem> GetList()
        {
            List<DynamicListItem> list = new List<DynamicListItem>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add(new DynamicListItem() { Id = 1, Title = "Item" + i.ToString() });
            //}
            list.Add(new DynamicListItem() { Id = 1, Title = "1/2 tablets (10mg) per day for 12 days" });

            return list;
        }

        #endregion
    }
    
    
    public class CustomValidation
    {
        #region Private Members
        private string message;
        #endregion

        #region Properties
        public bool ShowErrorMessage
        {
            get;
            set;
        }



        public object ValidationError
        {
            get
            {
                return null;
            }
            set
            {
                if (ShowErrorMessage)
                {
                    throw new ValidationException(message);
                }
            }
        }
        #endregion

        #region Constructor
        public CustomValidation(string message)
        {
            this.message = message;
        }
        #endregion
    }

    public static class Validations
    {
        public static void SetValidation(this FrameworkElement frameworkElement, string message)
        {
            CustomValidation customValidation = new CustomValidation(message);

            Binding binding = new Binding("ValidationError")
            {
                Mode = System.Windows.Data.BindingMode.TwoWay,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
                Source = customValidation
            };
            frameworkElement.SetBinding(Control.TagProperty, binding);
        }

        public static void RaiseValidationError(this FrameworkElement frameworkElement)
        {
            BindingExpression b = frameworkElement.GetBindingExpression(Control.TagProperty);

            if (b != null)
            {
                ((CustomValidation)b.DataItem).ShowErrorMessage = true;
                b.UpdateSource();
            }
        }

        public static void ClearValidationError(this FrameworkElement frameworkElement)
        {
            BindingExpression b = frameworkElement.GetBindingExpression(Control.TagProperty);

            if (b != null)
            {
                ((CustomValidation)b.DataItem).ShowErrorMessage = false;
                b.UpdateSource();
            }
        }

        public static bool IsTextValid(this string inputText)
        {
            bool isTextValid = true;

            foreach (char character in inputText)
            {
                if (char.IsWhiteSpace(character) == false)
                {
                    if (char.IsLetterOrDigit(character) == false)
                    {
                        if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                        {
                            isTextValid = false;
                            break;
                        }
                    }
                }
            }
            return isTextValid;
        }

        public static bool IsNumberValid(this string inputNumber)
        {
            bool isNumberValid = true;
            int number = -1;
            if (!Int32.TryParse(inputNumber, out number))
            {
                isNumberValid = false;
            }
            return isNumberValid;
        }

        public static bool IsPositiveNumberValid(this string inputNumber)
        {
            bool isNumberValid = true;
            int number = -1;
            if (!Int32.TryParse(inputNumber, out number))
            {
                isNumberValid = false;
            }
            if (isNumberValid == true)
            {
                int test = Convert.ToInt32(inputNumber);

                if (test < 0)
                    isNumberValid = false;
            }
            return isNumberValid;
        }

        public static bool IsItDecimal(this string inputNumber)
        {
            //bool IsItNumber = true;

            //string NumberExpression = @"^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$";
            ////string NumberExpression = @"^[-+]?[0-9]\d{0,2}(\.\d{1,2})?%?$";
            ////string NumberExpression=@"[^[-]?([1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|\.[0-9]{1,2})$]";
            //Regex isNumber = new Regex(NumberExpression);
            //if (!isNumber.IsMatch(inputNumber))
            //{
            //    IsItNumber = false;
            //}

            //return IsItNumber;
            bool isNumberValid = true;
            Decimal number = -1;
            if (!Decimal.TryParse(inputNumber, out number))
            {
                isNumberValid = false;
            }
            return isNumberValid;
        }

        public static bool IsItNumber(this string inputNumber)
        {
            bool IsItNumber = true;
            string NumberExpression = "[^0-9]";
            Regex isNumber = new Regex(NumberExpression);
            if (isNumber.IsMatch(inputNumber))
            {
                IsItNumber = false;
            }
            return IsItNumber;
        }

        public static bool IsEmailValid(this string inputEmail)
        {
            bool isEmailValid = true;
            string emailExpression = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            Regex re = new Regex(emailExpression);
            if (!re.IsMatch(inputEmail))
            {
                isEmailValid = false;
            }
            return isEmailValid;
        }

    }
}
