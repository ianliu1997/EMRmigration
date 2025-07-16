using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.ANC;

namespace PalashDynamics.ValueObjects.ANC
{
    public class clsGetANCGeneralDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsGetANCGeneralDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        private clsANCVO _ANCGeneralDetails = new clsANCVO();
        public clsANCVO ANCGeneralDetails
        {
            get
            {
                return _ANCGeneralDetails;
            }
            set
            {
                _ANCGeneralDetails = value;
            }
        }
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

        public long TabID { get; set; }
    }
}
