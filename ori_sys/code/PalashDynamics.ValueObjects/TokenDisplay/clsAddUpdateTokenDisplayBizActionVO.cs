using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.TokenDisplay
{
    public class clsAddUpdateTokenDisplayBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.TokenDisplay.clsAddUpdateTokenDisplayBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long Id { get; set; }
        public long UnitId { get; set; }
        public long PatientId { get; set; }
        public long VisitId { get; set; }
        public long DoctorID { get; set; }
        public DateTime VisitDate { get; set; }
        public bool IsDisplay { get; set; }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>

        //private clsLocalExaminationVO _objLocalExamination;
        //public clsLocalExaminationVO ObjLocalExamination
        //{
        //    get { return _objLocalExamination; }
        //    set
        //    {
        //        _objLocalExamination = value;
        //    }
        //}
        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private long _SuccessStatus;
        public long SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }



    }

    public class clsGetTokenDisplayBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.TokenDisplay.clsGetTokenDisplayBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        //public long Id { get; set; }
        public long UnitId { get; set; }
        //public long PatientId { get; set; }
        //public long VisitId { get; set; }
        //public long DoctorID { get; set; }
        public DateTime VisitDate { get; set; }
        public bool IsDisplay { get; set; }

        #endregion

        private List<clsTokenDisplayVO> objTokenDisplay = new List<clsTokenDisplayVO>();
        public List<clsTokenDisplayVO> ListTokenDisplay
        {
            get
            {
                return objTokenDisplay;
            }
            set
            {
                objTokenDisplay = value;

            }
           
        }

        private long _SuccessStatus;
        public long SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private long _ResultStatus;
        public long ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }
        //added by akshays
        public KeyValue Parent { get; set; }
        public class KeyValue
        {
            public object Key { get; set; }
            public string Value { get; set; }
        }

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }
        //closed by akshays

    }

    public class clsUpdateTokenDisplayStatusBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.TokenDisplay.clsUpdateTokenDisplayStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long Id { get; set; }
        public long UnitId { get; set; }
        public long PatientId { get; set; }
        public long VisitId { get; set; }
        public long DoctorID { get; set; }
        public DateTime VisitDate { get; set; }
        public bool IsDisplay { get; set; }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>

        //private clsLocalExaminationVO _objLocalExamination;
        //public clsLocalExaminationVO ObjLocalExamination
        //{
        //    get { return _objLocalExamination; }
        //    set
        //    {
        //        _objLocalExamination = value;
        //    }
        //}
        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private long _SuccessStatus;
        public long SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }



    }

    public class clsTokenDisplayVO : IValueObject, INotifyPropertyChanged
    {
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

        private string _TokenNo;
        public string TokenNo
        {
            get { return _TokenNo; }
            set
            {
                if (_TokenNo != value)
                {
                    _TokenNo = value;
                    OnPropertyChanged("TokenNo");
                }
            }
        }

        private string _MrNo;
        public string MrNo
        {
            get { return _MrNo; }
            set
            {
                if (value != _MrNo)
                {
                    _MrNo = value;
                    OnPropertyChanged("MrNo");
                }
            }
        }

        private string _Cabin;
        public string Cabin
        {
            get { return _Cabin; }
            set
            {
                if (value != _Cabin)
                {
                    _Cabin = value;
                    OnPropertyChanged("Cabin");
                }
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (value != _PatientName)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (value != _DoctorName)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        public bool IsDisplay { get; set; }

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
