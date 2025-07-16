using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
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
        public bool AllParameter { get; set; }

        List<clsPathoParameterMasterVO> _ParameterList = new List<clsPathoParameterMasterVO>();

        /// <summary>
        /// Output Property.
        /// Get Property To Access And Modify RoleList
        /// </summary>
        public List<clsPathoParameterMasterVO> ParameterList
        {
            get { return _ParameterList; }
            set { _ParameterList = value; }
        }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.PathologyMaster.clsGetPathoParameterBizAction";
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
            return "PalashDynamics.BusinessLayer.Pathology.PathologyMaster.clsGetPathoParameterByIDBizAction";
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

    //added by rohini dated 19.2.16

    public class clsGetPathoServicesByIDBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.PathologyMaster.clsGetPathoServicesByIDBizAction";
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
        public long ParaID { get; set; }
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


        //added by rohini dated 19.2.16
        private MasterListItem _ServiceDetails = null;
        public MasterListItem ServiceDetails
        {
            get { return _ServiceDetails; }
            set { _ServiceDetails = value; }
        }

        private List<MasterListItem> _ServiceDetailsList = null;
        public List<MasterListItem> ServiceDetailsList
        {
            get { return _ServiceDetailsList; }
            set { _ServiceDetailsList = value; }
        }


    }
}
