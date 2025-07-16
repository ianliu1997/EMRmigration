using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace PalashDynamics.ValueObjects.Master
{
    public class clsPrintMasterVO : IValueObject, INotifyPropertyChanged
    {
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

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }


        private string _ViewName;
        public string ViewName
        {
            get
            {
                return _ViewName;
            }

            set
            {
                if (value != _ViewName)
                {
                    _ViewName = value;
                    OnPropertyChanged("ViewName");
                }
            }
        }

        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }

            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }


        #endregion

        #region INotifyPropertyChanged Members

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

    public class clsGetMasterNamesBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetMasterNamesBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsPrintMasterVO objPrintMaster = null;
        public List<clsPrintMasterVO> objPrintMasterList { get; set; }
        public List<MasterListItem> MasterList { get; set; }

    }
    public class clsGetDatatoPrintMasterBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDatatoPrintMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public string ViewName { get; set; }
        public long id { get; set; }

        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public List<MasterListItem> MasterList { get; set; }

    }
    public class clsMasterDataVO : IValueObject, INotifyPropertyChanged
    {
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

        private string _Code;
        public string Code
        {
            get
            {
                return _Code;
            }

            set
            {
                if (value != _Code)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }


        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private decimal _Rate;
        public decimal Rate
        {
            get
            {
                return _Rate;
            }

            set
            {
                if (value != _Rate)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }
        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }

            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        private string _column2;
        public string column2
        {
            get
            {
                return _column2;
            }

            set
            {
                if (value != _column2)
                {
                    _column2 = value;
                    OnPropertyChanged("column2");
                }
            }
        }
        private string _column0;
        public string column0
        {
            get
            {
                return _column0;
            }

            set
            {
                if (value != _column0)
                {
                    _column0 = value;
                    OnPropertyChanged("column0");
                }
            }
        }
        private string _column1;
        public string column1
        {
            get
            {
                return _column1;
            }

            set
            {
                if (value != _column1)
                {
                    _column1 = value;
                    OnPropertyChanged("column1");
                }
            }
        }

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }


        #endregion

        #region INotifyPropertyChanged Members

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
