using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.OutPatientDepartment
{
    public class clsPatientKinDetailsVO : IValueObject, INotifyPropertyChanged
    {        
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
 
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Property Declaration Section

        private long _KinId;
        public long KinId { get; set; }

        private long _PatientId;
        public long PatientId { get; set; }

        private string _Name;
        public string Name { get; set; }

        private long _RelationId;
        public long RelationId { get; set; }

        private string _ContactNo1;
        public string ContactNo1 { get; set; }

        private string _ContactNo2;
        public string ContactNo2 { get; set; }

        private string _AddressLine1;
        public string AddressLine1 { get; set; }

        private string _AddressLine2;
        public string AddressLine2 { get; set; }

        private string _AddressLine3;
        public string AddressLine3 { get; set; }

        private long _CountryId;
        public long CountryId { get; set; }

        private long _StateId;
        public long StateId { get; set; }

        private long _DistrictID;
        public long DistrictID { get; set; }

        private long _TalukaID;
        public long TalukaID { get; set; }

        private long _CityId;
        public long CityId { get; set; }

        private long _AreaId;
        public long AreaId { get; set; }

        private string _Pincode;
        public string Pincode { get; set; }

        #region Temp. Variables
        //Temp. variables - Later needs to delete
        private string _District;
        public string District { get; set; }

        private string _Taluka;
        public string Taluka { get; set; }

        #endregion

        #endregion

        #region Common Properties

        private long _UnitId;
        public long UnitId { get; set; }

        private bool _Status = true;
        public bool Status { get; set; }

        private long _AddedBy;
        public long AddedBy { get; set; }

        private string _AddedOn;
        public string AddedOn { get; set; }

        private DateTime _AddedDateTime;
        public DateTime AddedDateTime { get; set; }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName { get; set; }

        private long _UpdatedBy;
        public long UpdatedBy { get; set; }

        private string _UpdatedOn;
        public string UpdatedOn { get; set; }

        private DateTime _UpdatedDateTime;
        public DateTime? UpdatedDateTime { get; set; }

        private string _UpdatedWindowsLoginName;
        public string UpdatedWindowsLoginName { get; set; }

        #endregion
    }
}
