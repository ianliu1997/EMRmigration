using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.ValueObjects.Administration.DiagnosisServiceDrugLink
{
    public class clsGetDiagnosisByIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionVO Member
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DiagnosisServiceDrugLink.clsGetDiagnosisByIDBizAction";
        }
        #endregion


        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
        public string ServiceList { get; set; }
        public string DrugList { get; set; }

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

        private List<clsDoctorSuggestedServiceDetailVO> _DiagnosisServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
        public List<clsDoctorSuggestedServiceDetailVO> DiagnosisServiceList
        {
            get
            {
                return _DiagnosisServiceList;
            }
            set
            {
                _DiagnosisServiceList = value;
            }
        }

        private clsDoctorSuggestedServiceDetailVO _DiagnosisServiceDetails;
        public clsDoctorSuggestedServiceDetailVO DiagnosisServiceDetails
        {
            get
            {
                return _DiagnosisServiceDetails;
            }
            set
            {
                _DiagnosisServiceDetails = value;
            }
        }

        private List<clsItemMasterVO> _DiagnosisDrugList = new List<clsItemMasterVO>();
        public List<clsItemMasterVO> DiagnosisDrugList
        {
            get
            {
                return _DiagnosisDrugList;
            }
            set
            {
                _DiagnosisDrugList = value;
            }
        }

        private clsItemMasterVO _DiagnosisDrugDetails;
        public clsItemMasterVO DiagnosisDrugDetails
        {
            get
            {
                return _DiagnosisDrugDetails;
            }
            set
            {
                _DiagnosisDrugDetails = value;
            }
        }
        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }

        }
    }
}
