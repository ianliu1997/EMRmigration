using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
   public class clsMISConfigurationVO:IValueObject, INotifyPropertyChanged 
    {
        public string ToXml()
        {
            return this.ToString();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private long _id;
        public long ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public long ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string ClinicCode{ get; set; }
        public string MISTypeName { get; set; }
        public DateTime? ConfigDate { get; set; }
       
        public string ScheduleName { get; set; }

        public long MISTypeId { get; set; }

        public long MISReportFormatId { get; set; }

        public long ScheduleOn { get; set; }

        public string ScheduleDetails { get; set; }

        public DateTime? ScheduleTime { get; set; }

        public DateTime? ScheduleStartDate { get; set; }

        public DateTime? ScheduleEndDate { get; set; }

        public bool Status { get; set; }

        public long CreatedUnitID { get; set; }

        public long UpdatedUnitId { get; set; }

        public long AddedBy { get; set; }

        public string AddedOn { get; set; }

        public DateTime AddedDateTime { get; set; }

        public long UpdatedBy { get; set; }

        public string UpdatedOn { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public string AddedWindowsLoginName { get; set; }

        public string UpdateWindowsLoginName { get; set; }

        public string ReportDetails { get; set; }
        public string StaffDetails { get; set; }

        private List<clsGetMISReportTypeVO> _ReportList;
        public List<clsGetMISReportTypeVO> ReportList
        {
            get
            {
                if (_ReportList == null)
                    _ReportList = new List<clsGetMISReportTypeVO>();
                return _ReportList;
            }
            set { _ReportList = value; }
        }

        private List<clsGetMISStaffVO> _StaffList;
        public List<clsGetMISStaffVO> StaffList
        {
            get
            {
                if (_StaffList == null)
                    _StaffList = new List<clsGetMISStaffVO>();
                return _StaffList;
            }
            set { _StaffList = value; }
        }
    }
}
