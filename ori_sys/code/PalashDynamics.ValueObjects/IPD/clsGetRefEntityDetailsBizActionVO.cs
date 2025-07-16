using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsGetRefEntityDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            // throw new NotImplementedException();

            return "PalashDynamics.BusinessLayer.IPD.clsGetRefEntityDetailsBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsRefEntityDetailsVO _Details = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsRefEntityDetailsVO Details
        {
            get { return _Details; }
            set { _Details = value; }

        }
        private List<clsRefEntityDetailsVO> _List = null;
        public List<clsRefEntityDetailsVO> List
        {
            get { return _List; }
            set { _List = value; }
        }

    }
    public class clsRefEntityDetailsVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
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

        #region Common Properties


        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }
        private long _AdmissionUnitID;
        public long AdmissionUnitID
        {
            get { return _AdmissionUnitID; }
            set
            {
                if (_AdmissionUnitID != value)
                {
                    _AdmissionUnitID = value;
                    OnPropertyChanged("AdmissionUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion
        private long _AdmID;
        public long AdmID
        {
            get { return _AdmID; }
            set
            {
                if (_AdmID != value)
                {
                    _AdmID = value;
                    OnPropertyChanged("AdmID");
                }
            }
        }
        private long _RefEntityID;
        public long RefEntityID
        {
            get { return _RefEntityID; }
            set
            {
                if (_RefEntityID != value)
                {
                    _RefEntityID = value;
                    OnPropertyChanged("RefEntityID");
                }
            }
        }
        private long _RefEntityTypeID;
        public long RefEntityTypeID
        {
            get { return _RefEntityTypeID; }
            set
            {
                if (_RefEntityTypeID != value)
                {
                    _RefEntityTypeID = value;
                    OnPropertyChanged("RefEntityTypeID");
                }
            }
        }
        private string _RefEntityIDDesc;
        public string RefEntityIDDesc
        {
            get { return _RefEntityIDDesc; }
            set
            {
                if (_RefEntityIDDesc != value)
                {
                    _RefEntityIDDesc = value;
                    OnPropertyChanged("RefEntityIDDesc");
                }
            }
        }
        private string _RefEntityTypeIDDesc;
        public string RefEntityTypeIDDesc
        {
            get { return _RefEntityTypeIDDesc; }
            set
            {
                if (_RefEntityTypeIDDesc != value)
                {
                    _RefEntityTypeIDDesc = value;
                    OnPropertyChanged("RefEntityTypeIDDesc");
                }
            }
        }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

    }

    public class clsAddRefEntityDetailsBizActionVO : IBizActionValueObject 
    {
        public string GetBizAction()
        {
            // throw new NotImplementedException();

            return "PalashDynamics.BusinessLayer.IPD.clsAddRefEntityDetailsBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsRefEntityDetailsVO _Details = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsRefEntityDetailsVO Details
        {
            get { return _Details; }
            set { _Details = value; }

        }
        private List<clsRefEntityDetailsVO> _List = null;
        public List<clsRefEntityDetailsVO> List
        {
            get { return _List; }
            set { _List = value; }
        }
    }
}
