using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
    public class clsGetANCUSGListBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsGetANCUSGListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        private List<clsANCUSGDetailsVO> _ANCUSGDetailsList = new List<clsANCUSGDetailsVO>();
        public List<clsANCUSGDetailsVO> ANCUSGDetailsList
        {
            get
            {
                return _ANCUSGDetailsList;
            }
            set
            {
                _ANCUSGDetailsList = value;
            }
        }
        private clsANCUSGDetailsVO _ANCUSGDetails = new clsANCUSGDetailsVO();
        public clsANCUSGDetailsVO ANCUSGDetails
        {
            get
            {
                return _ANCUSGDetails;
            }
            set
            {
                _ANCUSGDetails = value;
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
