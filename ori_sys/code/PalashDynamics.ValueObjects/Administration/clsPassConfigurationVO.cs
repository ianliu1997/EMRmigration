using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;


namespace PalashDynamics.ValueObjects.Administration
{
   public class clsPassConfigurationVO:IValueObject, INotifyPropertyChanged
    {
       #region IValueObject Members
        public string ToXml()
        {
            return this.ToXml();
        }
       #endregion


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
         }

        #endregion

        public long ID { get; set; }
        //public string Password { get; set; }
        public Int16 MinPasswordLength { get; set; }
        public Int16 MaxPasswordLength { get; set; }
        public bool AtLeastOneDigit { get; set; }
        public bool AtLeastOneLowerCaseChar { get; set; }
        public bool AtLeastOneUpperCaseChar { get; set; }
        public bool AtLeastOneSpecialChar { get; set; }

        public Int16 NoOfPasswordsToRemember { get; set; }
        public Int16 MinPasswordAge { get; set; }
        public Int16 MaxPasswordAge { get; set; }
        public Int16 AccountLockThreshold { get; set; }

        public float AccountLockDuration { get; set; }

        public long UpdatedUnitId { get; set; }
        public long UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string UpdateWindowsLoginName { get; set; }

      //  public List<clsPassConfigurationVO> PassConfigList { get; set; }
       //string inputEmail, int MinPasswordLength, int MaxPasswordLength, bool? AtLeastOneDigit, bool? AtLeastOneSpecialChar, bool? AtLeastOneLowerChar, bool? AtLeastOneUpperChar
        public long IsPasswordValid(string Password)
        {
            bool isPasswordValid = true;
            long ValidatePassword=0;
            // string emailExpression = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            string strDigitExpr = "";
            string strSpecialCharExpr = "";
            string strLowerCharExpr = "";
            string strUpperCharExpr = "";

            int PasswordLength = Password.Length;
            
            if (PasswordLength < MinPasswordLength)
            {
                ValidatePassword = 1;
                return ValidatePassword;
            }

            if (PasswordLength > MaxPasswordLength)
            {
                ValidatePassword = 2;
                return ValidatePassword;
            }
            string strLsngthExpr = "^.*(?=.{" + PasswordLength + ",})";

            string emailExpression;
            if (AtLeastOneDigit == true)
                strDigitExpr = @"(?=.*\d)";

            if (AtLeastOneLowerCaseChar == true)
                strLowerCharExpr = "(?=.*[a-z])";

            if (AtLeastOneUpperCaseChar == true)
                strUpperCharExpr = "(?=.*[A-Z])";

            if (AtLeastOneSpecialChar == true)
                strSpecialCharExpr = "(?=.*[@#$%^&+=]).*$";
            else
                strSpecialCharExpr = ".*$";

            emailExpression = strLsngthExpr + strDigitExpr + strLowerCharExpr + strUpperCharExpr + strSpecialCharExpr;

            if (AtLeastOneDigit == true)
            {
                Regex Digit = new Regex(strDigitExpr);
                if (!Digit.IsMatch(Password))
                {
                    ValidatePassword = 3;
                    return ValidatePassword;
                }
            }
            if (AtLeastOneLowerCaseChar == true)
            {
                Regex Lower = new Regex(strLowerCharExpr);
                if (!Lower.IsMatch(Password))
                {
                    ValidatePassword = 4;
                    return ValidatePassword;
                }
            }
            if (AtLeastOneUpperCaseChar == true)
            {
                Regex Upper = new Regex(strUpperCharExpr);
                if (!Upper.IsMatch(Password))
                {
                    ValidatePassword = 5;
                    return ValidatePassword;
                }
            }
            
            if (AtLeastOneSpecialChar == true)
            {
                Regex SpecialChar = new Regex(strSpecialCharExpr);
                if (!SpecialChar.IsMatch(Password))
                {
                    ValidatePassword = 6;
                    return ValidatePassword;
                }
            }
            return ValidatePassword;
        }
   }
}
