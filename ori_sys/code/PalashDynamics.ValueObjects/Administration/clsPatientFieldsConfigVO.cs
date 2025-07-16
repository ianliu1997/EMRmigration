using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsPatientFieldsConfigVO : IValueObject, INotifyPropertyChanged
    {
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
        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }

            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private string _TableName;
        public string TableName
        {
            get
            {
                return _TableName;
            }

            set
            {
                if (value != _TableName)
                {
                    _TableName = value;
                    OnPropertyChanged("TableName");
                }
            }
        }

        private string _FieldName;
        public string FieldName
        {
            get
            {
                return _FieldName;
            }

            set
            {
                if (value != _FieldName)
                {
                    _FieldName = value;
                    OnPropertyChanged("FieldName");
                }
            }
        }

        private string _FieldColumn;
        public string FieldColumn
        {
            get
            {
                return _FieldColumn;
            }

            set
            {
                if (value != _FieldColumn)
                {
                    _FieldColumn = value;
                    OnPropertyChanged("FieldColumn");
                }
            }
        }

        public override string ToString()
        {
            return this.FieldName;
        }
    }
}
