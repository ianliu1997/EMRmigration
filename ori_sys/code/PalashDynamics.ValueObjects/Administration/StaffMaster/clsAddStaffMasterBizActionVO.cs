using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.StaffMaster
{

    public class clsGetStaffBizActionVO : IBizActionValueObject
    {

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }


        private List<clsStaffMasterVO> myVar = new List<clsStaffMasterVO>();
        public List<clsStaffMasterVO> StaffMasterList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private clsStaffMasterVO _StaffDetails;
        public clsStaffMasterVO StaffDetails
        {
            get { return _StaffDetails; }
            set { _StaffDetails = value; }
        }

        private bool _IsForGRO = false;
        public bool IsForGRO
        {
            get { return _IsForGRO; }
            set { _IsForGRO = value; }
        }

        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RadiologyDesignationID { get; set; }
        public string PathologyDesignationID { get; set; }
        public string GRODesignationID { get; set; }
        public long DesignationID { get; set; }
        public long UnitID { get; set; }

        private bool _IsForRadiology = false;
        public bool IsForRadiology
        {
            get { return _IsForRadiology; }
            set { _IsForRadiology = value; }
        }

        private bool _IsForPathology = false;
        public bool IsForPathology
        {
            get { return _IsForPathology; }
            set { _IsForPathology = value; }
        }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsGetStaffMasterBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

    }

   public class clsAddStaffMasterBizActionVO:IBizActionValueObject
    {

        private clsStaffMasterVO _StaffDetails;
        public clsStaffMasterVO StaffDetails
        {
            get { return _StaffDetails; }
            set { _StaffDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsAddStaffMasterBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }
}
