using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
    public class clsDeleteANCExaminationBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsDeleteANCExaminationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        private clsANCExaminationDetailsVO _ANCExaminationDetails = new clsANCExaminationDetailsVO();
        public clsANCExaminationDetailsVO ANCExaminationDetails
        {
            get
            {
                return _ANCExaminationDetails;
            }
            set
            {
                _ANCExaminationDetails = value;
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

