using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetEMRTemplateListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetEMRTemplateListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


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
        public Boolean IsProcedureTemplate { get; set; }  
        public long TemplateID { get; set; }
        public Boolean IsphysicalExam { get; set; }
       

        private List<clsEMRTemplateVO> objEMRTemplate = new List<clsEMRTemplateVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsEMRTemplateVO> objEMRTemplateList
        {
            get { return objEMRTemplate; }
            set { objEMRTemplate = value; }
        }

        private List<MasterListItem> objMaster = new List<MasterListItem>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<MasterListItem> objMasterList
        {
            get { return objMaster; }
            set { objMaster = value; }
        }
    }

    public class clsGetEMRTemplateListForOTBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetEMRTemplateListForOTBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


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

        public long TemplateID { get; set; }
        public Boolean IsphysicalExam { get; set; }
        public string ProcedureTemplateID { get; set; }

        private List<clsEMRTemplateVO> objEMRTemplate = new List<clsEMRTemplateVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsEMRTemplateVO> objEMRTemplateList
        {
            get { return objEMRTemplate; }
            set { objEMRTemplate = value; }
        }

        private List<MasterListItem> objMaster = new List<MasterListItem>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<MasterListItem> objMasterList
        {
            get { return objMaster; }
            set { objMaster = value; }
        }
    }

    public class clsGetEMR_PCR_FieldListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetEMR_PCR_FieldListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


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

        public long SectionID { get; set; }
        public bool Status { get; set; }

        private List<MasterListItem> objFieldMasterList = new List<MasterListItem>();
        /// <summary>
        /// Output Property.
        /// This Property Contains PCR Field List Which is fetched.
        /// </summary>
        public List<MasterListItem> PCR_FieldMasterList
        {
            get { return objFieldMasterList; }
            set { objFieldMasterList = value; }
        }
    }

    public class clsGetEMR_CaseReferral_FieldListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetEMR_CaseReferral_FieldListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


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

        public long SectionID { get; set; }
        public bool Status { get; set; }

        private List<MasterListItem> objFieldMasterList = new List<MasterListItem>();
        /// <summary>
        /// Output Property.
        /// This Property Contains PCR Field List Which is fetched.
        /// </summary>
        public List<MasterListItem> CaseReferral_FieldMasterList
        {
            get { return objFieldMasterList; }
            set { objFieldMasterList = value; }
        }
    }
}
