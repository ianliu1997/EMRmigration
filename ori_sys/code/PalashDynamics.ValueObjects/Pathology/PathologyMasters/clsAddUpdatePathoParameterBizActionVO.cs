using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
{
    public class clsAddUpdatePathoParameterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.PathologyMaster.clsAddUpdatePathoParameterBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        clsPathoParameterMasterVO objPathoParameter = new clsPathoParameterMasterVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>

        public clsPathoParameterMasterVO PathologyParameter
        {
            get { return objPathoParameter; }
            set { objPathoParameter = value; }
        }
    }

    public class clsUpdatePathoParameterStatusBizActionVO : IBizActionValueObject
    {
        private int _SuccessStatus;

        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        //   public long ID { get; set; }
        private clsPathoParameterMasterVO objTempStatus = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsPathoParameterMasterVO PathoParameterStatus
        {
            get { return objTempStatus; }
            set { objTempStatus = value; }
        }


        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.PathologyMaster.clsUpdatePathoParameterStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

  


}
