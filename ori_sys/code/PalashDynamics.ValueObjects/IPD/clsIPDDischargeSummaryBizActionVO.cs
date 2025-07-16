using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsGetIPDDischargeSummaryBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsGetIPDDischargeSummaryBizAction";
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

        private List<clsIPDDischargeSummaryVO> _DischargeSummaryList;
        public List<clsIPDDischargeSummaryVO> DischargeSummaryList
        {
            get { return _DischargeSummaryList; }
            set { _DischargeSummaryList = value; }
        }

        private clsIPDDischargeSummaryVO _DischargeSummaryDetails;
        public clsIPDDischargeSummaryVO DischargeSummaryDetails
        {
            get { return _DischargeSummaryDetails; }
            set { _DischargeSummaryDetails = value; }
        }

        private clsIPDDischargeSummaryVO _AdmPatientDetails;
        public clsIPDDischargeSummaryVO AdmPatientDetails
        {
            get { return _AdmPatientDetails; }
            set { _AdmPatientDetails = value; }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }
        public int DisAdm { get; set; }
        public string StrDisAdm { get; set; }
        public string MrNo { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long AdmID { get; set; }
        public long AdmUnitID { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class clsAddIPDDischargeSummaryBizActionVO : IBizActionValueObject
    {
        private clsIPDDischargeSummaryVO _DischargeSummary;
        public clsIPDDischargeSummaryVO DischargeSummary
        {
            get { return _DischargeSummary; }
            set { _DischargeSummary = value; }
        }

        private List<clsIPDDischargeSummaryVO> _DischargeSummaryList;
        public List<clsIPDDischargeSummaryVO> DischargeSummaryList
        {
            get { return _DischargeSummaryList; }
            set { _DischargeSummaryList = value; }
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

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsAddIPDDischargeSummaryBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsGetPatientsDischargeSummaryInfoInHTMLBizAction";
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

        private List<clsIPDDischargeSummaryVO> _DischargeSummaryList;
        public List<clsIPDDischargeSummaryVO> DischargeSummaryList
        {
            get { return _DischargeSummaryList; }
            set { _DischargeSummaryList = value; }
        }

        private clsIPDDischargeSummaryVO _DischargeSummaryDetails;
        public clsIPDDischargeSummaryVO DischargeSummaryDetails
        {
            get { return _DischargeSummaryDetails; }
            set { _DischargeSummaryDetails = value; }
        }

        private clsIPDDischargeSummaryVO _AdmPatientDetails;
        public clsIPDDischargeSummaryVO AdmPatientDetails
        {
            get { return _AdmPatientDetails; }
            set { _AdmPatientDetails = value; }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }
        public int DisAdm { get; set; }
        public string StrDisAdm { get; set; }
        public string MrNo { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long PrintID { get; set; }
        public long AdmID { get; set; }
        public long AdmUnitID { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }

    public class clsFillDataGridDischargeSummaryListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsFillDataGridDischargeSummaryListBizAction";
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

        private List<clsIPDDischargeSummaryVO> _DischargeSummaryList;
        public List<clsIPDDischargeSummaryVO> DischargeSummaryList
        {
            get { return _DischargeSummaryList; }
            set { _DischargeSummaryList = value; }
        }

        private clsIPDDischargeSummaryVO _DischargeSummaryDetails;
        public clsIPDDischargeSummaryVO DischargeSummaryDetails
        {
            get { return _DischargeSummaryDetails; }
            set { _DischargeSummaryDetails = value; }
        }

        private clsIPDDischargeSummaryVO _AdmPatientDetails;
        public clsIPDDischargeSummaryVO AdmPatientDetails
        {
            get { return _AdmPatientDetails; }
            set { _AdmPatientDetails = value; }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }
        public int DisAdm { get; set; }
        public string StrDisAdm { get; set; }
        public string MrNo { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long AdmID { get; set; }
        public long AdmUnitID { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }


}
