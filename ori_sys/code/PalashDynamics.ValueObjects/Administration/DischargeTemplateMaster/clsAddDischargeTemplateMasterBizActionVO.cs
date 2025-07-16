using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PalashDynamics.ValueObjects.Administration.DischargeTemplateMaster
{
    public class clsAddDischargeTemplateMasterBizActionVO : IBizActionValueObject
    {
        private clsDischargeTemplateMasterVO _DischargeTemplateMaster;
        public clsDischargeTemplateMasterVO DischargeTemplateMaster
        {
            get { return _DischargeTemplateMaster; }
            set { _DischargeTemplateMaster = value; }
        }

        private List<clsDischargeTemplateDetailsVO> _DischargeTemplateDetails;
        public List<clsDischargeTemplateDetailsVO> DischargeTemplateDetailsList
        {
            get { return _DischargeTemplateDetails; }
            set { _DischargeTemplateDetails = value; }
        }
        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

        public bool IsCheckBox { get; set; }

        public bool IsModify { get; set; }

        public long ResultStatus { get; set; }

        private long _SuccessStatus;
        public long SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DischargeTemplateMaster.clsAddDischargeTemplateMasterBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsUpdateDischargeTemplateMasterBizActionVO : IBizActionValueObject
    {
        private clsDischargeTemplateMasterVO _DischargeTemplateMaster;
        public clsDischargeTemplateMasterVO DischargeTemplateMaster
        {
            get { return _DischargeTemplateMaster; }
            set { _DischargeTemplateMaster = value; }
        }
        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DischargeTemplateMaster.clsUpdateDischargeTemplateMasterBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsAddDischargeTemplateDetailsBizActionVO : IBizActionValueObject
    {
        private clsDischargeTemplateMasterVO _DischargeTemplateDetails;
        public clsDischargeTemplateMasterVO DischargeTemplateDetails
        {
            get { return _DischargeTemplateDetails; }
            set { _DischargeTemplateDetails = value; }
        }
        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DischargeTemplateMaster.clsAddDischargeTemplateDetailsBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsUpdateDischargeTemplateDetailsBizActionVO : IBizActionValueObject
    {
        private clsDischargeTemplateMasterVO _DischargeTemplateDetails;
        public clsDischargeTemplateMasterVO DischargeTemplateDetails
        {
            get { return _DischargeTemplateDetails; }
            set { _DischargeTemplateDetails = value; }
        }

        //private ObservableCollection<clsDischargeTemplateMasterVO> _StaffExpDetailsList;
        //public ObservableCollection<clsDischargeTemplateMasterVO> StaffExpDetailsList
        //{
        //    get { return _StaffExpDetailsList; }
        //    set { _StaffExpDetailsList = value; }
        //}

        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DischargeTemplateMaster.clsUpdateDischargeTemplateDetailsBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsGetDischargeTemplateMasterListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DischargeTemplateMaster.clsGetDischargeTemplateMasterListBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
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
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public string SearchExpression { get; set; }
        public bool PagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public string MRNo { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long ID { get; set; }
        public long Count { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        private bool _IsViewClick = false;
        public bool IsViewClick
        {
            get { return _IsViewClick; }
            set { _IsViewClick = value; }
        }

        private clsDischargeTemplateMasterVO _DischargeTemplateMaster;
        public clsDischargeTemplateMasterVO DischargeTemplateMaster
        {
            get { return _DischargeTemplateMaster; }
            set { _DischargeTemplateMaster = value; }
        }

        private List<clsDischargeTemplateMasterVO> _DischargeTemplateMasterList;
        public List<clsDischargeTemplateMasterVO> DischargeTemplateMasterList
        {
            get { return _DischargeTemplateMasterList; }
            set { _DischargeTemplateMasterList = value; }
        }

        private List<clsDischargeTemplateDetailsVO> _DischargeTemplateDetails;
        public List<clsDischargeTemplateDetailsVO> DischargeTemplateDetailsList
        {
            get { return _DischargeTemplateDetails; }
            set { _DischargeTemplateDetails = value; }
        }
    }
}
