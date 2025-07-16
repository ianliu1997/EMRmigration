using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.IO;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Text;
using System.Windows.Media.Imaging;

//
// SilverlightTips.com
//

namespace CIMS
{

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

    public static class Extensions
    {
        //By anjali..............
        public static byte[] ToByteArray(this WriteableBitmap bmp)
        {
            int[] p = bmp.Pixels;
            int len = p.Length << 2;
            byte[] result = new byte[len];
            Buffer.BlockCopy(p, 0, result, 0, len);
            return result;
            //// Init buffer
            //int w = bmp.PixelWidth;
            //int h = bmp.PixelHeight;
            //int[] p = bmp.Pixels;
            //int len = p.Length;
            //byte[] result = new byte[4 * w * h];

            //// Copy pixels to buffer
            //for (int i = 0, j = 0; i < len; i++, j += 4)
            //{
            //    int color = p[i];
            //    result[j + 0] = (byte)(color >> 24); // A
            //    result[j + 1] = (byte)(color >> 16); // R
            //    result[j + 2] = (byte)(color >> 8);  // G
            //    result[j + 3] = (byte)(color);       // B
            //}

            //return result;
        }
        //.........................
        public static void FromByteArray(this WriteableBitmap bmp, byte[] buffer)
        {
            Buffer.BlockCopy(buffer, 0, bmp.Pixels, 0, buffer.Length);
        }

        public static D DeepCopyd<D>(this D oSource)
        {
            D oClone;

            DataContractSerializer dcs = new DataContractSerializer(typeof(D));
            using (MemoryStream ms = new MemoryStream())
            {
                dcs.WriteObject(ms, oSource);
                ms.Position = 0;
                oClone = (D)dcs.ReadObject(ms);
            }

            return oClone;
        }

        public static T DeepCopy<T>(this T oSource)
        {

            T oClone;
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                dcs.WriteObject(ms, oSource);
                ms.Position = 0;
                oClone = (T)dcs.ReadObject(ms);
            }
            return oClone;
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
        public static bool AllowSpecialChar(this string inputCharacter)
        {
            bool AllowSpecialChar = true;
            string CharacterExpression = "[/:@#$%^+=)(?]";
            Regex IsSpecial = new Regex(CharacterExpression);
            if (IsSpecial.IsMatch(inputCharacter))
            {
                AllowSpecialChar = false;
            }
            return AllowSpecialChar;
        }
        public static bool IsPhoneNumberValid(this string inputPhNumber)
        {
            bool isPhoneNumberValid = true;
            string PhNumberExpression = "^[0-9]{2,3}-? ?[0-9]{6,7}$";
            Regex re = new Regex(PhNumberExpression);
            if (!re.IsMatch(inputPhNumber))
            {
                isPhoneNumberValid = false;
            }
            return isPhoneNumberValid;
        }
        //rohinee
        public static bool IsNameValid(this string inputName)
        {
            bool isNameValid = true;
            //string PhNameExpression = @"^[a-zA-Z0-9\s.\?\,\'\;\:\!\-]+$";
            //Regex re = new Regex(PhNameExpression);
            //if (!re.IsMatch(inputName))
            //{
            //    isNameValid = false;
            //}

            string PhNameExpression ="^[a-zA-Z ]*$";
            Regex re = new Regex(PhNameExpression);
            if (!re.IsMatch(inputName))
            {
                isNameValid = false;
            }
            return isNameValid;
        }
        //by rohini for no operator allowded  dated 8/12/2016
        public static bool IsOperatorNameInValid(this string inputName)
        {
            bool isNameValid = true;
           // string PhNameExpression = "^[a-zA-Z0-9:@#$%^&=!,.?]"; 
            string PhNameExpression = "[^a-zA-Z0-9:@#$%^%&=!,?\\w\\s]"; 
                //"^[a-zA-Z0-9:@#$%^.&_!,><}{?]$";
          
            Regex re = new Regex(PhNameExpression);
            if (!re.IsMatch(inputName))
            {
                isNameValid = false;
            }
            return isNameValid;
        }
        //
             //by rohini for result netry
        public static bool IsValidPositiveNumberWithDecimal(this string inputName)
        {
            bool isNameValid = true;       
            string PhNameExpression = @"^[+]?([0-9]+(?:[\.][0-9]*)?|\.[0-9]+)$";  
            Regex re = new Regex(PhNameExpression);
            if (!re.IsMatch(inputName))
            {
                isNameValid = false;
            }
            return isNameValid;
        }
        //
        //
       
        public static bool IsNumberValid(this string inputNumber)
        {
            bool isNumberValid = true;
            long number = -1;
            if (!Int64.TryParse(inputNumber, out number))
            {
                isNumberValid = false;
            }
            return isNumberValid;
        }

        public static bool IsPositiveNumber(this string inputNumber)
        {
            
            bool isNumberValid = true;
            long number = -1;
            if (!Int64.TryParse(inputNumber, out number))
            {

                isNumberValid = false;
            }
            else if (Convert.ToInt64(inputNumber) < 0)
                isNumberValid = false;

            return isNumberValid;
        }

        public static bool IsValueDouble(this string inputNumber)
        {
            bool isNumberValid = true;
            double number = -1;
            if (!Double.TryParse(inputNumber, out number))
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

        public static bool IsValueNotZero(this string inputNumber)
        {
            bool isNumberValid = true;
            string NumberExpression = "[0]";
            Regex isNumber = new Regex(NumberExpression);
            if (isNumber.IsMatch(inputNumber))
            {
                isNumberValid = false;
            }
            return isNumberValid;
        }

        public static bool IsItValidQuantity(this string inputNumber)
        {
            bool IsItNumber = true;
            string NumberExpression = @"^([0-9]{0,5})$";
            Regex isNumber = new Regex(NumberExpression);
            if (!isNumber.IsMatch(inputNumber))
            {
                IsItNumber = false;
            }
            return IsItNumber;
        }

        public static bool IsItDecimal(this string inputNumber)
        {
            bool IsItNumber = true;

            string NumberExpression = @"^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$";
            //string NumberExpression = @"^[-+]?[0-9]\d{0,2}(\.\d{1,2})?%?$";
            //string NumberExpression=@"[^[-]?([1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|\.[0-9]{1,2})$]";
            Regex isNumber = new Regex(NumberExpression);
            if (isNumber.IsMatch(inputNumber))
            {
                IsItNumber = false;
            }
            return IsItNumber;
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

        public static bool IsItCharacter(this string inputCharacter)
        {
            bool IsItCharacter = true;
            string CharacterExpression = "[^a-zA-Z]";
            Regex isCharacter = new Regex(CharacterExpression);
            if (isCharacter.IsMatch(inputCharacter))
            {
                IsItCharacter = false;
            }
            return IsItCharacter;

        }
        
        public static bool IsItCharacterWithHyphan(this string inputCharacter)
        {
            bool IsItCharacter = true;
            string CharacterExpression = "[^a-zA-Z-]";
            Regex isCharacter = new Regex(CharacterExpression);
            if (isCharacter.IsMatch(inputCharacter))
            {
                IsItCharacter = false;
            }
            return IsItCharacter;

        }

        public static bool IsItUpperCase(this string inputCharacter)
        {
            bool IsItUpperCase = true;
            string CharacterExpression = "[^A-Z]";
            Regex IsUpperCase = new Regex(CharacterExpression);
            if (IsUpperCase.IsMatch(inputCharacter))
            {
                IsItUpperCase = false;
            }
            return IsItUpperCase;
        }

        public static bool IsItLowerCase(this string inputCharacter)
        {
            bool IsItLowerCase = true;
            string CharacterExpression = "[^a-z]";
            Regex IsLower = new Regex(CharacterExpression);
            if (IsLower.IsMatch(inputCharacter))
            {
                IsItLowerCase = false;
            }
            return IsItLowerCase;
        }

        public static bool IsItSpecialChar(this string inputCharacter)
        {
            bool IsItSpecialChar = true;
            string CharacterExpression = "[@#$%^&+=]";
            Regex IsSpecial = new Regex(CharacterExpression);
            if (IsSpecial.IsMatch(inputCharacter))
            {
                IsItSpecialChar = false;
            }
            return IsItSpecialChar;
        }

        public static bool IsItNEFTRTGSNumber(this string inputCharacter)
        {
            bool IsItNEFTRTGSNumber = true;
            //string CharacterExpression1 = @"(?<![A-Z])[A-Z]{5}(?![A-Z][0-9])[0-9]{11}(?![0-9])";
            //Regex IsSpecial1 = new Regex(CharacterExpression1);
            //if (IsSpecial1.IsMatch(inputCharacter))
            //{
            //    IsItNEFTRTGSNumber = false;
            //}
            return IsItNEFTRTGSNumber;
        }

        //by Anjali........................

        public static bool IsPositiveDoubleValid(this string inputNumber)
        {
            bool isNumberValid = true;
            double number = -1;
            if (!Double.TryParse(inputNumber, out number))
            {
                isNumberValid = false;
            }
            if (isNumberValid == true)
            {
                double test = Convert.ToDouble(inputNumber);

                if (test < 0)
                    isNumberValid = false;
            }
            return isNumberValid;
        }

        public static bool IsValidCountryCode(this string inputPhNumber)
        {
            bool isCountryCodeValid = true;
            //string CountryCodeExpression = @"^([0|\+[0-9]{1,4})$";

           // string CountryCodeExpression = @"^[\+]([0-9]{1,3})$";
            string CountryCodeExpression = @"^\+[0-9]{0,3}$";
            Regex re = new Regex(CountryCodeExpression);
            if (!re.IsMatch(inputPhNumber))
            {
                isCountryCodeValid = false;
            }
            return isCountryCodeValid;
        }

        public static bool IsValidPositiveNegative(this string inputPhNumber)
        {
            bool isGRNRoundOffValid = true;
            //string CountryCodeExpression = @"^([0|\+[0-9]{1,4})$";

            // string CountryCodeExpression = @"^[\+]([0-9]{1,3})$";
            string GRNRoundOffExpression = @"^(\+|-)?[0-9]\d*(\.\d+)?$";
            Regex re = new Regex(GRNRoundOffExpression);
            if (!re.IsMatch(inputPhNumber))
            {
                isGRNRoundOffValid = false;
            }
            return isGRNRoundOffValid;
        }

        public static bool IsValidDigintWithTwoDecimalPlaces(this string inputNumber)
        {
            bool isDigitValid = true;


            //string CodeExpression = @"([0-9]{1,16})[\.]([0-9]{1,2})|([0-9]{1,16})$";
           //string CodeExpression = @"^[0-9]{1,16}(\.[0-9]{1,2})?$";
           //string CodeExpression = @"^([0-9]{0,16}+(?:[\.][0-9]{0,2})?|\.[0-9]{0,2}+)$";
           string CodeExpression = @"^([0-9]{0,16}(?:[\.][0-9]{0,2})?|\.[0-9]{0,2})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        public static bool IsValidDigintWithOneDecimalPlaces(this string inputNumber)
        {
            bool isDigitValid = true;


            //string CodeExpression = @"([0-9]{1,16})[\.]([0-9]{1,2})|([0-9]{1,16})$";
            //string CodeExpression = @"^[0-9]{1,16}(\.[0-9]{1,2})?$";
            //string CodeExpression = @"^([0-9]{0,16}+(?:[\.][0-9]{0,2})?|\.[0-9]{0,2}+)$";
            string CodeExpression = @"^([0-9]{0,16}(?:[\.][0-9]{0,1})?|\.[0-9]{0,1})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        public static bool IsValidOneDigitWithOneDecimal(this string inputNumber)
        {
            bool isDigitValid = true;
            // string CodeExpression = @"^[0-9]([.,][0-9]{1,3})?$";
            string CodeExpression = @"^([0-9]{0,1}(?:[\.][0-9]{0,1})?|\.[0-9]{0,1})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        public static bool IsValidOneDigitWithTwoDecimal(this string inputNumber)
        {
            bool isDigitValid = true;
           // string CodeExpression = @"^[0-9]([.,][0-9]{1,3})?$";
            string CodeExpression = @"^([0-9]{0,1}(?:[\.][0-9]{0,2})?|\.[0-9]{0,2})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        public static bool IsValidTwoDigitWithTwoDecimal(this string inputNumber)
        {
            bool isDigitValid = true;
            // string CodeExpression = @"^[0-9]([.,][0-9]{1,3})?$";
            string CodeExpression = @"^([0-9]{0,2}(?:[\.][0-9]{0,2})?|\.[0-9]{0,2})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        public static bool IsValidTwoDigitWithOneDecimal(this string inputNumber)
        {
            bool isDigitValid = true;
            // string CodeExpression = @"^[0-9]([.,][0-9]{1,3})?$";
            string CodeExpression = @"^([0-9]{0,2}(?:[\.][0-9]{0,1})?|\.[0-9]{0,1})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        public static bool IsValidFourDigitWithTwoDecimal(this string inputNumber)
        {
            bool isDigitValid = true;
            // string CodeExpression = @"^[0-9]([.,][0-9]{1,3})?$";
            string CodeExpression = @"^([0-9]{0,4}(?:[\.][0-9]{0,2})?|\.[0-9]{0,2})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        public static bool IsValidThreeDigit(this string inputNumber)
        {
            bool isDigitValid = true;
            // string CodeExpression = @"^[0-9]([.,][0-9]{1,3})?$";
            string CodeExpression = @"^([0-9]{0,3})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        public static bool IsValidTwoDigit(this string inputNumber)
        {
            bool isDigitValid = true;
            // string CodeExpression = @"^[0-9]([.,][0-9]{1,3})?$";
            string CodeExpression = @"^([0-9]{0,2})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        public static bool IsValidThreeDigitWithTwoDecimal(this string inputNumber)
        {
            bool isDigitValid = true;
            // string CodeExpression = @"^[0-9]([.,][0-9]{1,3})?$";
            string CodeExpression = @"^([0-9]{0,3}(?:[\.][0-9]{0,2})?|\.[0-9]{0,2})$";
            Regex re = new Regex(CodeExpression);
            if (!re.IsMatch(inputNumber))
            {
                isDigitValid = false;
            }
            return isDigitValid;
        }

        //public static bool IsValidDigintWithTwoDecimalPlaces(this string inputNumber)
        //{
        //    bool isDigitValid = true;


        //    //string CodeExpression = @"([0-9]{1,16})[\.]([0-9]{1,2})|([0-9]{1,16})$";
        //    string CodeExpression = @"^\d{0,16}(\.\d{1,2}) |{0,16}?$";
        //    Regex re = new Regex(CodeExpression);
        //    if (!re.IsMatch(inputNumber))
        //    {
        //        isDigitValid = false;
        //    }
        //    return isDigitValid;
        //}


        //.................................

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
        //public static void RaiseValidationError(this FrameworkElement frameworkElement)
        //{
        //    //if(frameworkElement ==null)
        //       //  FrameworkElement result = GetParent(fe, typeof(DataGridCell));
        //    BindingExpression b = TextBox.GetBindingExpression(Control.TagProperty);

        //    if (b != null)
        //    {
        //        ((CustomValidation)b.DataItem).ShowErrorMessage = true;
        //        b.UpdateSource();
        //    }
        //}
       

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
        /// <summary>
        /// Finds an ancestor of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the ancestor</typeparam>
        /// <param name="obj">The DependencyObject whole ancestor we want to find.</param>
        /// <returns>The ancestor of the specified type.</returns>
        public static T FindAncestor<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                T o = obj as T;
                if (o != null)
                    return o;
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }
        /// <summary>
        /// Finds an ancestor of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the ancestor</typeparam>
        /// <param name="obj">The UIElement whole ancestor we want to find.</param>
        /// <returns>The ancestor of the specified type.</returns>
        public static T FindAncestor<T>(this UIElement obj) where T : UIElement
        {
            return FindAncestor<T>((DependencyObject)obj);
        }
        /// <summary>
        /// Gets children of a specific Type.
        /// </summary>
        /// <typeparam name="T">The type of children.</typeparam>
        /// <param name="parent">The parent whose children we want to find.</param>
        /// <param name="children">The list which will contain the matched children.</param>
        /// <param name="recurseThroughMatches">Specifies if the we should continue searching inside a matched element.</param>
        /// <param name="onlyVisible">true to only find visible elements.</param>
        public static void GetChildren<T>(DependencyObject parent, ref List<T> children, bool recurseThroughMatches, bool onlyVisible)
            where T : UIElement
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T)
                    {
                        T uiElement = child as T;
                        if (onlyVisible == false || uiElement.Visibility == Visibility.Visible)
                        {
                            children.Add(uiElement);
                            if (recurseThroughMatches == false)
                                continue;
                        }
                    }
                    GetChildren<T>(child, ref children, recurseThroughMatches, onlyVisible);
                }
            }
        }

        public static string ToTitleCase(this string thePhrase)
        {
            if (!string.IsNullOrEmpty(thePhrase))
            {
                StringBuilder newString = new StringBuilder();
                StringBuilder nextString = new StringBuilder();
                string[] phraseArray;
                string theWord;
                string returnValue;
                phraseArray = thePhrase.Split(null);
                for (int i = 0; i < phraseArray.Length; i++)
                {
                    theWord = phraseArray[i].ToLower();
                    if (theWord.Length > 1)
                    {
                        if (theWord.Substring(1, 1) == "'")
                        {
                            //Process word with apostrophe at position 1 in 0 based string.
                            if (nextString.Length > 0)
                                nextString.Replace(nextString.ToString(), null);
                            nextString.Append(theWord.Substring(0, 1).ToUpper());
                            nextString.Append("'");
                            if (theWord.Length > 2)
                            {
                                nextString.Append(theWord.Substring(2, 1).ToUpper());
                                nextString.Append(theWord.Substring(3).ToLower());
                            }
                            nextString.Append(" ");
                        }
                        else
                        {
                            //Process normal word (possible apostrophe near end of word.
                            if (nextString.Length > 0)
                                nextString.Replace(nextString.ToString(), null);
                            nextString.Append(theWord.Substring(0, 1).ToUpper());
                            nextString.Append(theWord.Substring(1).ToLower());
                            nextString.Append(" ");
                        }
                    }
                    else
                    {
                        //Process normal single character length word.
                        if (nextString.Length > 0)
                            nextString.Replace(nextString.ToString(), null);
                        nextString.Append(theWord.ToUpper());
                        nextString.Append(" ");
                    }
                    newString.Append(nextString);
                }
                returnValue = newString.ToString();
                return returnValue.Trim();
            }
            else
                return thePhrase;
        }

        //For Name (Character , ' ,' ') are allowed
        public static int CharactersWithBlankDotQuote(this int keyascii)
        {

            // if (!(((keyascii >= 97) && (keyascii <= 122)) || ((keyascii >= 65) && (keyascii <= 90)) || (keyascii == 8) || (keyascii == 32) || (keyascii == 46) || (keyascii == 39) || (keyascii == 222)))
            if (!((keyascii >= 97 & keyascii <= 122) | (keyascii >= 65 & keyascii <= 90) | keyascii == 8 | keyascii == 32 | keyascii == 46 | keyascii == 39 | keyascii == 222))
                return 0;
            else
                return keyascii;

        }

        public static bool IsPersonNameValid(this string inputName)
        {
            bool IsNameValid = true;
            string nameExpression = "^[a-zA-Z' ]*$";
            Regex re = new Regex(nameExpression);
            if (!re.IsMatch(inputName))
            {
                IsNameValid = false;
            }
            return IsNameValid;

        }

        public static bool IsIFSCCodeValid(this string inputName)
        {
            bool IsNameValid = true;
            string nameExpression = "^[a-zA-Z0-9]+$";
            Regex re = new Regex(nameExpression);
            if (!re.IsMatch(inputName))
            {
                IsNameValid = false;
            }
            return IsNameValid;

        }

        public static bool IsOnlyCharacters(this string inputName)
        {
            bool IsValid = true;

            string myExpression = "^[a-zA-Z ]*$";
            Regex re = new Regex(myExpression);
            if (!re.IsMatch(inputName))
            {
                IsValid = false;
            }

            return IsValid;
        }

        public static bool IsItSpecialCharAndMinus(this string inputCharacter)
        {
            bool IsItSpecialMinus = true;
            string CharacterExpression = "[/:@#$%^&+=)(?]";
            Regex IsSpecial = new Regex(CharacterExpression);
            if (IsSpecial.IsMatch(inputCharacter))
            {
                IsItSpecialMinus = false;
            }
            return IsItSpecialMinus;
        }

        ///////////////Added By YK/////////
        public static bool IsMobileNumberValid(this string inputPhNumber)
        {
           

            bool IsItNumber = true;
          
            //string NumberExpression = @"^[7-9]{1}[0-9]{8}$";
         //   string NumberExpression = @"^7|8|9|0{9}$";
            string NumberExpression = @"[7-9]+[0-9]{0,9}$";
            Regex isNumber = new Regex(NumberExpression);

            if (isNumber.IsMatch(inputPhNumber))
            {

                IsItNumber = true;
            }
            else
            {
                IsItNumber = false;
            }
          
            
            return IsItNumber;
        }
        //////////////END/////////////////



    }
        public class ScrollValueTracker : FrameworkElement, IDisposable
    {
        public ScrollValueTracker(ScrollBar vertScrollBar)
        {
            Binding binding = new Binding("Value");
            binding.Source = vertScrollBar;
            this.SetBinding(ValueProperty, binding);
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ScrollValueTracker), new PropertyMetadata(0.0, BoundValueChangedCallback));

        private static void BoundValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ScrollValueTracker)d).OnValueChanged();
        }

        internal void OnValueChanged()
        {
            if (this.ValueChanged != null)
                this.ValueChanged(this, EventArgs.Empty);
        }

        public event EventHandler ValueChanged;

        #region IDisposable Members

        public void Dispose()
        {
            this.ClearValue(ValueProperty);
        }

        #endregion

    }

}


