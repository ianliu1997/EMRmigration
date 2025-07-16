using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
    public class clsAddUpdateUSGdetailsBizActionVO :IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsAddUpdateUSGdetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

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
