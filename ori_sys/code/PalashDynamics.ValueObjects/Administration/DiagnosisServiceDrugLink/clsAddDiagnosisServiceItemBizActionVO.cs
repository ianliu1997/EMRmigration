using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.ValueObjects.Administration.DiagnosisServiceDrugLink
{
    public class clsAddDiagnosisServiceItemBizActionVO : IBizActionValueObject
    {
        #region IBizActionVO Member
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DiagnosisServiceDrugLink.clsAddDiagnosisServiceDrugBizAction";
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
        public string DrugCodeList { get; set; }
        public string DrugNameList { get; set; }
        public string ServiceCodeList { get; set; }
        public string ServiceNameList { get; set; }
        public string ServiceGroupList { get; set; }
        public string ServiceTypeList { get; set; }
        public string ServiceSpecliztionList { get; set; }
        public long TemplateID { get; set; }
        public List<clsEMRAddDiagnosisVO> DiagnosisList { get; set; }
        public List<clsDoctorSuggestedServiceDetailVO> DiagnosisServiceList { get; set; }
        public List<clsItemMasterVO> DiagnosisDrugList { get; set; }
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

        public Boolean IsModify { get; set; }
        public Boolean IsCheckStatus { get; set; }
        public Boolean Status { get; set; }
        public long? CreatedUnitID { get; set; }
        public long? UpdatedUnitID { get; set; }
        public long? AddedBy { get; set; }
        public string AddedOn { get; set; }
        public DateTime? AddedDateTime { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string AddedWindowsLoginName { get; set; }
        public string UpdateWindowsLoginName { get; set; }
    }
}
