using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{

    public class clsMenuVO : IValueObject, INotifyPropertyChanged
    {        
        #region IValueObject Members

            public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (null != handler)
                {
                    handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        
        #endregion

        public long ID { get; set; }

        public string Tooltip { get; set; }

        public string Title { get; set; }

        public string ImagePath { get; set; }

        public string Parent { get; set; }

        public string Module { get; set; }

        public string Action { get; set; }

        public string Header { get; set; }

        public string Configuration { get; set; }

        public string Mode { get; set; }

        public bool Active { get; set; }

        public long? ParentId { get; set; }

        public int? MenuOrder { get; set; }

        private bool? boolStatus = null;
        public bool? Status
        {
            get { return boolStatus; }
            set
            {
                if (value != boolStatus)
                {
                    boolStatus = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public Boolean ISFemale { get; set; }

        public bool IsCreate { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRead { get; set; }
        public bool IsPrint { get; set; }

        
        public List<clsMenuVO> ChildMenuList { get; set; }

        //added by neena
        public bool IsArt { get; set; }
        public long PlanTherapyId { get; set; }
        public long PlanTherapyUnitId { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long GridIndex { get; set; }
        public PalashDynamics.ValueObjects.OutPatientDepartment.clsVisitVO CurrentVisit { get; set; }
        //

        //Added by Ramesh
        public string SOPFileName { get; set; }
        //


    }
}
