using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    //class clsSMSTemplateBizActionVO
    //{
    //}

    public class clsGetSMSTemplateDetailsBizActionVO :IBizActionValueObject 
    {
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

        public long ID { get; set; }
        public int UserType { get; set; }

        private clsSMSTemplateVO objSMSDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsSMSTemplateVO SMSDetails
        {
            get { return objSMSDetails; }
            set { objSMSDetails = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetSMSTemplateDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsGetListSMSTemplateListBizActionVO : IBizActionValueObject 
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetListSMSTemplateBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

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

        public long ID { get; set; }

        List<clsSMSTemplateVO> _TempList = new List<clsSMSTemplateVO>();

        /// <summary>
        /// Output Property.
        /// Get Property To Access And Modify RoleList
        /// </summary>
        public List<clsSMSTemplateVO> SMSList
        {
            get { return _TempList; }
            set { _TempList = value; }
        }

      // public long ID { get; set; }

        //private List<clsSMSTemplateVO> objDetails = null;
        ///// <summary>
        ///// Output Property.
        ///// This Property Contains OPDPatient Details Which is Added.
        ///// </summary>
        //public List<clsSMSTemplateVO> Details
        //{
        //    get { return objDetails; }
        //    set { objDetails = value; }
       // }
    }

    public class clsUpdateTemplatestatusBizActionVO : IBizActionValueObject
    {
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

        //   public long ID { get; set; }
        private clsSMSTemplateVO objTempStatus = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsSMSTemplateVO SMSTempStatus
        {
            get { return objTempStatus; }
            set { objTempStatus = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsUpdateSMSTemplateStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }
}
