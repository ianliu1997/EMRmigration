using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IPD
{
    #region BizActionVO Added By Kiran For ADD,Ipdate,Get,UpdateStatus
    public class clsAddUpdateIntakeOutputChartAndDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsAddUpdateIntakeOutputChartAndDetailsBizAction";
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
        /// 

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsIPDIntakeOutputChartVO> _IntakeOutputList;
        public List<clsIPDIntakeOutputChartVO> IntakeOutputList
        {
            get { return _IntakeOutputList; }
            set { _IntakeOutputList = value; }
        }

        private clsIPDIntakeOutputChartVO __IntakeOutputDetails;
        public clsIPDIntakeOutputChartVO IntakeOutputDetails
        {
            get { return __IntakeOutputDetails; }
            set { __IntakeOutputDetails = value; }
        }

        private bool _IsAddUdtIntakeOutChart = false;
        public bool IsAddUdtIntakeOutChart
        {
            get { return _IsAddUdtIntakeOutChart; }
            set { _IsAddUdtIntakeOutChart = value; }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }

    }

    public class clsGetIntakeOutputChartDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIntakeOutputChartDetailsBizAction";
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
        /// 

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsIPDIntakeOutputChartVO> _GetIntakeOutputList;
        public List<clsIPDIntakeOutputChartVO> GetIntakeOutputList
        {
            get { return _GetIntakeOutputList; }
            set { _GetIntakeOutputList = value; }
        }

        private clsIPDIntakeOutputChartVO __GetIntakeOutputDetails;
        public clsIPDIntakeOutputChartVO GetIntakeOutputDetails
        {
            get { return __GetIntakeOutputDetails; }
            set { __GetIntakeOutputDetails = value; }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }

    }

    public class clsGetIntakeOutputChartDetailsByPatientIDBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIntakeOutputChartDetailsByPatientIDBizAction";
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
        /// 

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsIPDIntakeOutputChartVO> _GetIntakeOutputList;
        public List<clsIPDIntakeOutputChartVO> GetIntakeOutputList
        {
            get { return _GetIntakeOutputList; }
            set { _GetIntakeOutputList = value; }
        }

        private clsIPDIntakeOutputChartVO __GetIntakeOutputDetails;
        public clsIPDIntakeOutputChartVO GetIntakeOutputDetails
        {
            get { return __GetIntakeOutputDetails; }
            set { __GetIntakeOutputDetails = value; }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }

    }

    public class clsUpdateStatusIntakeOutputChartBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsUpdateStatusIntakeOutputChartBizAction";
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
        /// 

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsIPDIntakeOutputChartVO> _IntakeOutputList;
        public List<clsIPDIntakeOutputChartVO> IntakeOutputList
        {
            get { return _IntakeOutputList; }
            set { _IntakeOutputList = value; }
        }

        private clsIPDIntakeOutputChartVO __IntakeOutputDetails;
        public clsIPDIntakeOutputChartVO IntakeOutputDetails
        {
            get { return __IntakeOutputDetails; }
            set { __IntakeOutputDetails = value; }
        }

        private bool _IsCalledForFreeze=false;
        public bool IsCalledForFreeze
        {
            get { return _IsCalledForFreeze; }
            set { _IsCalledForFreeze = value; }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }

    }

    #endregion
}
