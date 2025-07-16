using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
    public class clsDeleteANCInvestigationBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsDeleteANCInvestigationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        private clsANCInvestigationDetailsVO _ANCInvestigationDetails = new clsANCInvestigationDetailsVO();
        public clsANCInvestigationDetailsVO ANCInvestigationDetails
        {
            get
            {
                return _ANCInvestigationDetails;
            }
            set
            {
                _ANCInvestigationDetails = value;
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

        public bool IsUpdate { get; set; }
    }
}
