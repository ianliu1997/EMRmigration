using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.CompoundDrug
{
    public class clsGetPatietPrescriptionCompoundDrugBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CompoundDrug.clsGetPatietPrescriptionCompoundDrugBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        private int _SuccessStatus;
        public int SuccessStatus
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

        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionCompoundDrugList { get; set; }

        public clsCompoundDrugMasterVO CompoundDrugMaster { get; set; }
        public int TotalRowCount { get; set; }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    
                }
            }
        }

    }
    public class clsGetPatientPrescriptionCompoundDrugVO : INotifyPropertyChanged, IValueObject
    {

        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
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

        # region Properties

        private long _DrugID;
        public long DrugID
        {
            get { return _DrugID; }
            set
            {
                if (_DrugID != value)
                {
                    _DrugID = value;
                    OnPropertyChanged("DrugID");
                }
            }
        }

        private string _DrugName;
        public string DrugName
        {
            get { return _DrugName; }
            set
            {
                if (_DrugName != value)
                {
                    _DrugName = value;
                    OnPropertyChanged("DrugName");
                }
            }
        }

        private float _Quantity;
        public float Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        FrequencyMaster _Frequency = new FrequencyMaster { ID = 0, Description = "--Select--" };
        public FrequencyMaster Frequency
        {
            get
            {
                return _Frequency;
            }
            set
            {
                if (value != _Frequency)
                {
                    _Frequency = value;
                    OnPropertyChanged("SelectedFrequency");
                }
            }


        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }
        #endregion Properties
    }
}
