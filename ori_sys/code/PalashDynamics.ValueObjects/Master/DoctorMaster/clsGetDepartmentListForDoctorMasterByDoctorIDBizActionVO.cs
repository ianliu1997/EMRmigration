using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
   public class clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO:IBizActionValueObject
    {
       private List<clsDoctorVO> objDetails = null;
        public List<clsDoctorVO> DoctorDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<MasterListItem> _MasterListItem;

        public List<MasterListItem> MasterListItem
        {
            get { return _MasterListItem; }
            set { _MasterListItem = value; }
        }

        private long _ID;
        public long DoctorID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private long _Status;
        public long Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private bool _IsUnitWise;

        public bool IsUnitWise
        {
            get { return _IsUnitWise; }
            set { _IsUnitWise = value; }
        }

        public bool IsClinical { get; set; }    // flag use to Show/not Clinical Departments  02032017
        public long UnitID { get; set; }    // fill Unitwise Departments  02032017

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDepartmentListForDoctorMasterByDoctorIDBizAction";
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
