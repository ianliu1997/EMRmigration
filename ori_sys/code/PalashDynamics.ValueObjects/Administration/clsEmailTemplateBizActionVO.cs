using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    class clsEmailTemplateBizActionVO
    {
    }

    public class clsGetEmailTemplateListBizActionVO:IBizActionValueObject 
    {
        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

       // List<clsEmailAttachmentVO> _AttachmentDetails = new List<clsEmailAttachmentVO>();

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

        List<clsEmailTemplateVO> _TempList = new List<clsEmailTemplateVO>();

        /// <summary>
        /// Output Property.
        /// Get Property To Access And Modify RoleList
        /// </summary>
        public List<clsEmailTemplateVO> EmailList
        {
            get { return _TempList; }
            set { _TempList = value; }
        }


        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsEmailTemplateListBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsGetEmailTemplateBizActionVO:IBizActionValueObject
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

        private clsEmailTemplateVO objEmailDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsEmailTemplateVO EmailDetails
        {
            get { return objEmailDetails; }
            set { objEmailDetails = value; }
        }


        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetEmailTemplateDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsUpdateEmailTemplateStatusBizActionVO : IBizActionValueObject 
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
        private clsEmailTemplateVO objTempStatus = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsEmailTemplateVO EmailTempStatus
        {
            get { return objTempStatus; }
            set { objTempStatus = value; }
        }


        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsUpdateEmailTemplateStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsAddEmailTemplateBizActionVO :IBizActionValueObject 
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

        private clsEmailTemplateVO objEmailTemplate = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Password Configuration Details Which is Added.
        /// </summary>

        public clsEmailTemplateVO EmailTemplate
        {
            get { return objEmailTemplate; }
            set { objEmailTemplate = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddEmailTemplateBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }
}
