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
using System.ComponentModel.DataAnnotations;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace DataDrivenApplication
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
    
    public static class Helpers
    {
        public static T DeepCopy<T>(this T oSource)
        {

            T oClone;
            XmlSerializer dcs = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                dcs.Serialize(ms, oSource);
                ms.Position = 0;
                oClone = (T)dcs.Deserialize(ms);
            }
            return oClone;
        }

        public static string XmlSerilze<T>(this T data)
        {
            System.Xml.Serialization.XmlSerializer MyXMLWriter = new System.Xml.Serialization.XmlSerializer(data.GetType());
            StringWriter MyStringWriter = new StringWriter();
            MyXMLWriter.Serialize(MyStringWriter, data);
            return MyStringWriter.ToString();

        }

        public static T XmlDeserialize<T>(this string xml)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(xml)))
            {
                var serializer = new XmlSerializer(typeof(T));
                T theObject = (T)serializer.Deserialize(stream);
                return theObject;
            }
        }

        public static string Serialize<T>(this T data)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memoryStream, data);

                memoryStream.Seek(0, SeekOrigin.Begin);

                var reader = new StreamReader(memoryStream);
                string content = reader.ReadToEnd();
                return content;
            }
        }

        public static T Deserialize<T>(this string xml)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(xml)))
            {
                var serializer = new DataContractSerializer(typeof(T));
                T theObject = (T)serializer.ReadObject(stream);
                return theObject;
            }
        }

        public static List<DynamicListItem> GetList()
        {
            List<DynamicListItem> list = new List<DynamicListItem>();
            for (int i = 1; i <= 3; i++)
            {
                list.Add(new DynamicListItem() { Id = 1, Title = "Source" + i.ToString(), Value = "DataDrivenApplication.Source" + i.ToString() });
            }
            return list;
        }

        public static List<string> GetDrugTypeList()
        {
            //Old Version
            List<string> list = new List<string>();
            list.Add("Antibiotics");
            list.Add("Antiemetics");
            list.Add("Antipyretic");
            list.Add("Antispasmodic");
            return list;

            //New Version

        }

        public static List<string> GetListOfCheckBoxesTypeList()
        {
            List<string> list = new List<string>();
            list.Add("Nutrition List");
            list.Add("Other Alarms");
            return list;
        }


        public static List<string> GetAntibioticsList()
        {
            List<string> list = new List<string>();
            list.Add("Cotrimoxazole");
            //list.Add("Trimethoprim");
            //list.Add("Sulfamethoxazole");
            list.Add("Ciprofloxacin");
            list.Add("Doxycycline");
            list.Add("Furazolidone");
            //list.Add("Cotrimoxazole");
            list.Add("Metronidazole");
            //list.Add("Metronidazole");
            list.Add("Ampicillin");
            list.Add("Tinidazole");
            list.Add("Diloxanide Furoate");
            return list;
        }

        public static List<string> GetAntiemeticsList()
        {
            List<string> list = new List<string>();
            list.Add("metoclopramide");

            return list;
        }

        public static List<string> GetAntipyreticList()
        {
            List<string> list = new List<string>();
            list.Add("Paracetamol");

            return list;
        }

        public static List<string> GetAntispasmodicList()
        {
            List<string> list = new List<string>();
            list.Add("Dicylomine");

            return list;
        }

        //Older Version
        public static List<string> GetDrugList(long ID)
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 30; i++)
            //{
            //    list.Add("Drug" + i.ToString());
            //}
            list.Add("Cotrimoxazole");
            list.Add("Trimethoprim");
            list.Add("Sulfamethoxazole");
            list.Add("Ciprofloxacin");
            list.Add("Doxycycline");
            list.Add("Furazolidone");
            list.Add("Cotrimoxazole");
            list.Add("Metronidazole");
            list.Add("Metronidazole");
            list.Add("Ampicillin");
            list.Add("metoclopramide");
            list.Add("Paracetamol");
            list.Add("Dicylomine");
            return list;


            //Newer Version


        }

        public static List<MasterListItem> GetDayList()
        {
            //List<int> list = new List<int>();
            //for (int i = 1; i <= 100; i++)
            //{
            //    list.Add(i);
            //}

            List<MasterListItem> list = new List<MasterListItem>();
            for (int i = 1; i <= 100; i++)
            {
                list.Add(new MasterListItem() { ID = i, Description = i.ToString(), Status = true });
            }

            return list;
        }


        public static List<string> GetDosageList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Dosage" + i.ToString());
            //}
            list.Add("10 mg TMP and 50 mg SMX/ kg/day in 2 divided doses for 5 days");
            list.Add("20mg/kg/d in 2 divided doses for 5 days");
            list.Add("Single dose of 5mg/kg maximum 200mg)");
            list.Add("5-8mg/kg/day in 4 divided doses for 3 days");
            //list.Add("15mg/kg/day in 3 divided doses for 5 days");
            list.Add("15-20 mg/kg/day in 3 divided doses for 5days");
            list.Add("100 mg/kg/d for 10 divided doses");
            list.Add("10-15 mg/kg/day for 3 divided doses x 5 days");
            list.Add("20 mg /kg / day for 10 days");
            //list.Add("0.1 mg/kg/day 6-8 hours");
            //list.Add("15 mg/kg oral");
            list.Add("5-10mg orally upto 3-4 times daily");
            return list;
        }

        public static List<MasterListItem> GetRouteList()
        {
            //List<string> list = new List<string>();
            ////for (int i = 1; i <= 10; i++)
            ////{
            ////    list.Add("Dosage" + i.ToString());
            ////}

            //list.Add("Oral");
            //list.Add("IM");
            //list.Add("IV");

            List<MasterListItem> list = new List<MasterListItem>();
            list.Add(new MasterListItem() { ID = 1, Description = "Oral", Status = true });
            list.Add(new MasterListItem() { ID = 2, Description = "IM", Status = true });
            list.Add(new MasterListItem() { ID = 3, Description = "IV", Status = true });

            return list;
        }

        public static List<string> GetFrequencyList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Frequency" + i.ToString());

            //}
            list.Add("Stat");
            list.Add("Every Hour");
            list.Add("Every 2 Hour");
            list.Add("Every 4 Hour");
            list.Add("Every 6 Hour");
            list.Add("Once Daily");
            list.Add("Twice Daily");
            list.Add("Thrice Daily");
            list.Add("as needed");
            list.Add("other");
            return list;
        }

        public static List<string> GetInvestigationList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Frequency" + i.ToString());

            //}
            list.Add("--Select--");
            list.Add("Stool Exam");
            list.Add("Clinitest or Benedict’s test");
            list.Add("Endoscopy");
            list.Add("Sigmoidoscopy");
            list.Add("Small bowel biopsy");
            list.Add("Colonoscopy");
            list.Add("Barium studies");
            list.Add("Other");
            return list;
        }

        public static List<string> GetNutritionList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Frequency" + i.ToString());

            //}
            //list.Add("--Select--");
            list.Add("encourage breast feeding");
            list.Add("ORS first 4 hours");
            list.Add("freshly prepared mashed / ground food every 3 hours");
            list.Add("energy rich food complements");
            list.Add("additional meals x 3 a day for breast fed children");
            list.Add("additional mealsx 5 a day for non breastfed infants");
            list.Add("meals x 6 times a day for older infants");
            list.Add("supplement additional leafy vegetables, legumes, carrot, banana, pumpkin etc");
            list.Add("Other");
            return list;
        }

        public static List<string> GetOtherAlertsList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Frequency" + i.ToString());

            //}
            //list.Add("--Select--");
            list.Add("Blood in Stools");
            list.Add("Nausea Vomiting");
            list.Add("Urine Absent");
            list.Add("Lathergic Unconcious");
            list.Add("Eyes Sunken");
            list.Add("Skin color cyanosed");
            list.Add("Drinks poorly (Not able to drink)");
            list.Add("Skin/turgor retracts very slowly");
            list.Add("Weight loss >=9%");
            list.Add("Severe dehydration");
            list.Add("Nutritional status severely impaired");
            list.Add("Abdominal Examination Distended");
            list.Add("Eyes Sunken");
            list.Add("Skin color cyanosed");
            list.Add("Drinks poorly (Not able to drink)");
            list.Add("Skin/turgor retracts very slowly");
            list.Add("Other");
            return list;
        }

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
