using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.Administration.DiagnosisServiceDrugLink
{
    public class clsGetDiagnosisListBizActionVO : IBizActionValueObject
    {
        #region IBizActionVO Member
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DiagnosisServiceDrugLink.clsGetDiagnosisListBizAction";
        }
        #endregion


        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
        private List<clsEMRAddDiagnosisVO> _DiagnosisList = new List<clsEMRAddDiagnosisVO>();
        public List<clsEMRAddDiagnosisVO> DiagnosisList
        {
            get
            {
                return _DiagnosisList;
            }
            set
            {
                _DiagnosisList = value;
            }
        }

        private clsEMRAddDiagnosisVO _DiagnosisDetails;
        public clsEMRAddDiagnosisVO DiagnosisDetails
        {
            get
            {
                return _DiagnosisDetails;
            }
            set
            {
                _DiagnosisDetails = value;
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
