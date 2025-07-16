using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsBankVO : INotifyPropertyChanged, IValueObject
    {
        public clsBankVO()
        {

        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private string _Specialization;
        public string SpecializationDis
        {
            get { return _Specialization; }
            set
            {
                if (value != _Specialization)
                {
                    _Specialization = value;
                    OnPropertyChanged("SpecializationDis");
                }
            }
        }
    }
}
