using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.ANC;

namespace PalashDynamics.ValueObjects.ANC
{
   public class clsANCGetGeneralDetailsListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsANCGetGeneralDetailsListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        private List<clsANCVO> _ANCGeneralDetailsList = new List<clsANCVO>();
        public List<clsANCVO> ANCGeneralDetailsList
        {
            get
            {
                return _ANCGeneralDetailsList;
            }
            set
            {
                _ANCGeneralDetailsList = value;
            }
        }
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
