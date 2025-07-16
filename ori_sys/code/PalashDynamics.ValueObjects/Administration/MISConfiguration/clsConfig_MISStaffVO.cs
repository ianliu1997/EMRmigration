using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
    public class clsConfig_MISStaffVO : IValueObject, INotifyPropertyChanged
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

        public long Config_MISId { get; set; }

        public long StaffTypeId{get;set;}

        public long StaffId{get;set;}

        public bool Status{get;set;}

        public long CreatedUnitID{get;set;}

        public long UpdatedUnitId{get;set;}

        public long AddedBy{get;set;}

        public string AddedOn{get;set;}

        public DateTime AddedDateTime{get;set;}

        public long UpdatedBy{get;set;}

        public string UpdatedOn{get;set;}

        public DateTime UpdatedDateTime{get;set;}

        public string AddedWindowsLoginName{get;set;}

        public string UpdatedWindowsLoginName{get;set;}


     }
}


