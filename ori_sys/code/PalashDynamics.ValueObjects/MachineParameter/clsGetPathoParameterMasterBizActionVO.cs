using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;

namespace PalashDynamics.ValueObjects.MachineParameter
{
    public class clsGetPathoParameterMasterBizActionVO : IBizActionValueObject
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

        //List<clsPathoParameterMasterVO> _ParameterList = new List<clsPathoParameterMasterVO>();

        //public List<clsPathoParameterMasterVO> ParameterList
        //{
        //    get { return _ParameterList; }
        //    set { _ParameterList = value; }
        //}

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MachineParameter.clsGetPathoParameterBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsGetPathoParameterByIDBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MachineParameter.clsGetPathoParameterByIDBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long ID { get; set; }
        // public int UserType { get; set; }

        private clsPathoParameterMasterVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPathoParameterMasterVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }

    public class clsAddUpdatePathoMachineParameterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MachineParameter.clsAddUpdatePathoMachineParameterBizAction";
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

        public long ID { get; set; }
        public long UnitID { get; set; }

        private clsMachineParameterMasterVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsMachineParameterMasterVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    public class clsUpdateStatusMachineParameterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MachineParameter.clsUpdateStatusMachineParameterBizAction";
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

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long MachineID { get; set; }
        public bool Status { get; set; }
    }

    public class clsGetPathoMachineParameterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MachineParameter.clsGetPathoMachineParameterBizAction";
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

        public long ID { get; set; }
        public long UnitID { get; set; }
        public bool GetList { get; set; }

        public string Description { get; set; }
        public string MachineName { get; set; }
        public long MachineID { get; set; }
        // public int UserType { get; set; }

        private clsMachineParameterMasterVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsMachineParameterMasterVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<clsMachineParameterMasterVO> objDetailsList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsMachineParameterMasterVO> DetailsList
        {
            get { return objDetailsList; }
            set { objDetailsList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

    }

    public class clsGetParameterLinkingBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MachineParameter.clsGetParameterLinkingBizAction";
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

        public long ID { get; set; }
        public long UnitID { get; set; }
        public bool GetList { get; set; }

        //public string Description { get; set; }
        //public string MachineName { get; set; }
        public long MachineParaID { get; set; }
        public long ParameterID { get; set; }
        public long MachineID { get; set; }

        private clsParameterLinkingVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsParameterLinkingVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<clsParameterLinkingVO> objDetailsList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsParameterLinkingVO> DetailsList
        {
            get { return objDetailsList; }
            set { objDetailsList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

    }

    public class clsAddUpdateParameterLinkingBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MachineParameter.clsAddUpdateParameterLinkingBizAction";
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

        public long ID { get; set; }
        public long UnitID { get; set; }

        private clsParameterLinkingVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsParameterLinkingVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    public class clsUpdateStatusParameterLinkingBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MachineParameter.clsUpdateStatusParameterLinkingBizAction";
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

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long AppParameterID { get; set; }
        public long MachineParameterID { get; set; }
        public bool Status { get; set; }
    }

    //by rohini dated 18.1.2016
     public class clsGetParaByParaAndMachineBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MachineParameter.clsGetParaByParaAndMachineBizAction";
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

       

        //public string Description { get; set; }
        //public string MachineName { get; set; }
        public long MachineParaID { get; set; }
        public long ParameterID { get; set; }
        // public int UserType { get; set; }

        private clsParameterLinkingVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsParameterLinkingVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<clsParameterLinkingVO> objDetailsList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsParameterLinkingVO> DetailsList
        {
            get { return objDetailsList; }
            set { objDetailsList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

    }
    //
}
