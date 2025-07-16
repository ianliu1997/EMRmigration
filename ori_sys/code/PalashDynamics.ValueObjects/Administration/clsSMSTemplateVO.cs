using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{
   public class clsSMSTemplateVO : IValueObject, INotifyPropertyChanged
    {
        private long _id;
        public long ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Code { get; set; }

        public string Description { get; set; }

        public string EnglishText { get; set; }

        public string LocalText { get; set; }

        public bool Status { get; set; }
 
        public long CreatedUnitID { get; set;}

        public long UpdatedUnitId { get; set; }

        public long AddedBy { get; set; }

        public string AddedOn { get; set; }

        public DateTime AddedDateTime { get; set; }

        public long UpdatedBy { get; set; }

        public string UpdatedOn { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public string AddedWindowsLoginName { get; set; }

        public string UpdateWindowsLoginName { get; set; }

        #region IValueObject 
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion


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

    }
}
