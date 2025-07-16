using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
    public class clsGetANCInvestigationListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsGetANCInvestigationListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        private List<clsANCInvestigationDetailsVO> _ANCInvestigationDetailsList = new List<clsANCInvestigationDetailsVO>();
        public List<clsANCInvestigationDetailsVO> ANCInvestigationDetailsList
        {
            get
            {
                return _ANCInvestigationDetailsList;
            }
            set
            {
                _ANCInvestigationDetailsList = value;
            }
        }
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

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
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

    }
}
