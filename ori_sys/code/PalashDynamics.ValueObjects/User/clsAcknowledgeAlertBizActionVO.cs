using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;


namespace PalashDynamics.ValueObjects
{
    /// <summary>
    /// This class contains the properties for acknowledge alert and implements IBizActionValueObject interface which implements IValueObject interface.
    /// </summary>
    public class clsChangePasswordBizActionVO : IBizActionValueObject
    {
        /////// <summary>
        /////// It is long Type. Gets or sets the UserId.
        /////// </summary>
        ////public long UserId { get; set; }
        /////// <summary>
        /////// It is string Type. Gets or sets the CurrentPassword
        /////// </summary>
        ////public string CurrentPassword { get; set; }
        /////// <summary>
        /////// It is string Type. Gets or sets the NewPassword.
        /////// </summary>
        ////public string NewPassword { get; set; }
        /////// <summary>
        /////// is Integer type. Gets or sets the Success Status of query execution.
        /////// </summary>
        ////public Boolean CheckCurrentPassword { get; set; } 
        ////public int SuccessStatus { get; set; }
     
        private clsUserVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains User Details Which is Added.
        /// </summary>

        public clsUserVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

     

        private int _SuccessStatus;

        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        /// 
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

       
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsChangePasswordBizAction";
        }
        
        #endregion

        #region IValueObject Members

        public string ToXml()
        {
           return this.ToString();
        }

        #endregion
    }
}
